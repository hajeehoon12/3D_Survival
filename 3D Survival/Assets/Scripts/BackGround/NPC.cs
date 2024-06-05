using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public enum AIState
{ 
    Idle,
    Wandering,
    Attacking,
    Staying,
    Fleeing
}

public enum MonsterPattern
{
    None,
    Coward
}

public class NPC : MonoBehaviour , IDamagable
{

    [Header("Stats")]
    public int health;
    public int maxHealth;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;
    public MonsterPattern pattern;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;
    public float attackSpeed;

    [Header("Sound")]
    public string hitSound;

    public string dieSound;



    private float playerDistance;
    private bool takingDmg = false;
    private bool isDie = false;

    public float fieldOfView = 120f;

    //private bool isAttacking = false;

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    Coroutine attackCoroutine;

    private bool inBattle = false;

    public SkinnedMeshRenderer _body;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }


    void Start()
    {   
        maxHealth = health;
        SetState(AIState.Wandering);
    }

    
    void Update()
    {

        if (isDie) return;
        //if (isAttacking) return;

        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position); // �÷��̾�� �Ÿ����� ���

        animator.SetBool("Moving", aiState != AIState.Idle); // AIState�� ���� Animator Bool�� ����

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:

                if (takingDmg) return;
                PassiveUpdate();

                break;
            case AIState.Attacking:
                if (takingDmg) return;
                AttackingUpdate();

                break;
            case AIState.Fleeing:
                FleeingUpdate();

                break;
            case AIState.Staying:
            default:
                agent.speed = 0;
                agent.isStopped = false;
                break;
        }
    }

    public void SetState(AIState state) // AI ����
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:

                agent.speed = walkSpeed;
                agent.isStopped = true;
                
                break;

            case AIState.Wandering:

                agent.speed = walkSpeed;
                agent.isStopped = false;
                
                break;

            case AIState.Attacking:

                agent.speed = runSpeed;
                agent.isStopped = true;
                

                break;
            case AIState.Fleeing:

                agent.speed = runSpeed;
                agent.isStopped = false;

                break;
        }

        animator.speed = agent.speed / walkSpeed;

    }

    void FleeingUpdate() //Fleeing pattern
    {
        Vector3 directionAwayFromPlayer = transform.position - CharacterManager.Instance.Player.transform.position;
        Vector3 fleeDestination = transform.position + directionAwayFromPlayer.normalized * detectDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeDestination, out hit, maxWanderDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        if (playerDistance > detectDistance * 1.5f)
        {
            SetState(AIState.Wandering);
        }
    }

    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (playerDistance < detectDistance)
        {
            if (!inBattle)
            {
                inBattle = true;
                AudioManager.instance.PlayBGM("Battle", 0.2f);
            }
            SetState(AIState.Attacking);
        }

    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());

    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit; // �ִ� ��� ��ȯ��

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * 
            Random.Range(minWanderDistance, maxWanderDistance)) , out hit, maxWanderDistance, NavMesh.AllAreas);
        // (Vector3 sourcePosition, out NavMeshHit hit, float maxDistance, int areaMask)
        // sourcePosition : ������ ���� ���� , hit: �ִܰ�� ��ȯ��,  maxDistance: �ְ��Ÿ�, areaMask : ���̾� ���͸�
        // 

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < detectDistance) // 30������ Ȯ���۾� �ݺ�����
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere *
            Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;

    }

    IEnumerator AttackDelay()
    {

        //isAttacking = true;

        yield return new WaitForSeconds(attackSpeed);
        AudioManager.instance.PlaySFX(hitSound);
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
            CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
        attackCoroutine = null;
        //isAttacking = false;
    }

    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView()) // When in distance
        {
            
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                
                animator.speed = 1;
                animator.SetTrigger("Attack");


                if (attackCoroutine != null )
                {
                    StopCoroutine(attackCoroutine);
                }
                attackCoroutine = StartCoroutine(AttackDelay());

            }
        }
        else // detect and follow
        {
            if (playerDistance < detectDistance) 
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path)) // Path�� ����Ͽ� path������ �Ѱ���
                { // Path ����� �����ϸ�, Player���� ������ �� �ִ� ��ΰ� �ִٸ�
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    bool IsPlayerInFieldOfView() // if player is in npc fov
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer); // NPC�� ����� �÷��̾��� ����
        return angle < fieldOfView * 0.5f; // �Ѱ����� �����̱⿡ �������� ����
    }

    public void TakePhysicalDamage(int damage) // get damaged
    {
        if (isDie) return;

        takingDmg = true;
        StartCoroutine(TakingDmgMotion());

        animator.SetTrigger("Damaged");
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        else if (pattern == MonsterPattern.Coward && health <= maxHealth * 0.3f)
        {
            SetState(AIState.Fleeing);
        }

        // ������ ȿ��
        StartCoroutine(DamageFlash());

    }

    IEnumerator TakingDmgMotion() // check when getting dmged
    {
        yield return new WaitForSeconds(0.2f);
        takingDmg = false;
    }

    void Die() // when DIed
    {
        AudioManager.instance.PlaySFX(dieSound);
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            GameObject DropItems = Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity );
            DropItems.GetComponent<Rigidbody>().AddForce( (Vector3.up * 5 + Vector3.forward * 3) * DropItems.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            StartCoroutine(DropRotate(DropItems));
        }
        animator.SetTrigger("Dead");
        AudioManager.instance.PlaySFX("MonsterDown");
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ;
        StartCoroutine(SlowDie());
        agent.speed = 0;
        agent.isStopped = true;
        isDie = true;
    }

    IEnumerator DropRotate(GameObject DropItems) // Item Drop get rotation
    {
        float x = 0;
        while (x < 360)
        {
            x += 360 * 0.1f;
            DropItems.transform.eulerAngles = new Vector3(x, 0, 0);
            yield return new WaitForSeconds(0.1f);
        }
        
    }

    IEnumerator SlowDie() // Die Slowly
    {
        
        StartCoroutine(BGM_Change());
        yield return new WaitForSeconds(4f);
        //Debug.Log(isDie);
        Destroy(gameObject);
        

    }

    IEnumerator BGM_Change()
    {
        DOTween.To(() => AudioManager.instance.bgmPlayer.volume, x => AudioManager.instance.bgmPlayer.volume = x, 0f, 2);
        yield return new WaitForSeconds(2.1f);
        AudioManager.instance.StopBGM();
        //
        //AudioManager.instance.bgmPlayer.volume = 0.5f;
        AudioManager.instance.PlayBGM("Peace", 0.3f);
    }




    IEnumerator DamageFlash() // NPC get Damaged
    {
        agent.isStopped = true;
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);

        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
        agent.isStopped = false;
    }

}
