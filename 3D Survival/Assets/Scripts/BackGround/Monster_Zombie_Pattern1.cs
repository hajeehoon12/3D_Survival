using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Runtime.InteropServices;

public class Monster_Zombie_Pattern1 : MonoBehaviour , IDamagable
{

    [Header("Stats")]
    public int maxHealth;
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;

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

    private float playerDistance;

    private bool takingDmg = false;

    private bool isDie = false;

    public float fieldOfView = 120f;

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

        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);
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
            case AIState.Staying:
            default:
                agent.speed = 0;
                agent.isStopped = false;
                break;
        }
    }

    public void SetState(AIState state)
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
        }

        animator.speed = agent.speed / walkSpeed;

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
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * 
            Random.Range(minWanderDistance, maxWanderDistance)) , out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < detectDistance)
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
        yield return new WaitForSeconds(attackSpeed);
        AudioManager.instance.PlaySFX("BearHit");
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
            CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
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
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
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
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;

        
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

        StartCoroutine(DamageFlash());

    }

    IEnumerator TakingDmgMotion() // check when getting dmged
    {
        yield return new WaitForSeconds(0.2f);
        takingDmg = false;
    }

    void Die() // when Died
    {   
        StopCoroutine(attackCoroutine);
        AudioManager.instance.PlaySFX("BearDie");
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            GameObject DropItems = Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity );
            DropItems.GetComponent<Rigidbody>().AddForce( (Vector3.up * 5 + Vector3.forward * 3) * DropItems.GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            StartCoroutine(DropRotate(DropItems));
        }
        animator.SetTrigger("Dead");
        AudioManager.instance.PlaySFX("MonsterDown");
        StartCoroutine(SlowDie());
        agent.speed = 0;
        agent.isStopped = true;
        isDie = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxCollider.enabled = false;
        }
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
        Destroy(gameObject);
        

    }

    IEnumerator BGM_Change()
    {
        DOTween.To(() => AudioManager.instance.bgmPlayer.volume, x => AudioManager.instance.bgmPlayer.volume = x, 0f, 2);
        yield return new WaitForSeconds(2.1f);
        AudioManager.instance.StopBGM();
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
