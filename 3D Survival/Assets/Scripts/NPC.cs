using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum AIState
{ 
    Idle,
    Wandering,
    Attacking,
    Fleeing
}


public class NPC : MonoBehaviour , IDamagable
{

    [Header("Stats")]
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

    public float fieldOfView = 120f; // ���� �þ߰�

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    Coroutine attackCoroutine;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>(); // ���� ���ϰ� �ϱ� ����
    }


    void Start()
    {
        SetState(AIState.Wandering);
    }

    
    void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position); // �÷��̾�� �Ÿ����� ���

        animator.SetBool("Moving", aiState != AIState.Idle); // AIState�� ���� Animator Bool�� ����

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:

                PassiveUpdate();

                break;
            case AIState.Attacking:

                AttackingUpdate();

                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
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
                agent.isStopped = false;

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
        // sourcePosition : ������ ���� ���� , hit: �ִܰ�� ��ȯ��,  maxDistance: �ְ�Ÿ�, areaMask : ���̾� ���͸�
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
        yield return new WaitForSeconds(attackSpeed);
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
            CharacterManager.Instance.Player.controller.GetComponent<IDamagable>().TakePhysicalDamage(damage);
        attackCoroutine = null;
        
    }

    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                
                animator.speed = 1;
                animator.SetTrigger("Attack");
                attackCoroutine = StartCoroutine(AttackDelay());
            }
        }
        else
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

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer); // NPC�� ����� �÷��̾��� ����
        return angle < fieldOfView * 0.5f; // �Ѱ����� �����̱⿡ �������� ����

        
    }

    public void TakePhysicalDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        health -= damage;
        if (health <= 0)
        {
            //�״´�
            Die();
        }

        // ������ ȿ��
        StartCoroutine(DamageFlash());

    }

    void Die()
    { 
        for(int i = 0; i < dropOnDeath.Length; i++)
        {
            GameObject DropItems = Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity );
            StartCoroutine(DropRotate(DropItems));
        }
        animator.SetTrigger("Dead");
        StartCoroutine(SlowDie());
    }

    IEnumerator DropRotate(GameObject DropItems)
    {
        float x = 0;
        while (x < 2160)
        {
            x += 2160 * Time.deltaTime;
            DropItems.transform.eulerAngles = new Vector3(x, 0, 0);
        }
        yield return null;
    }

    IEnumerator SlowDie()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);

        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < meshRenderers.Length; i++)
        {
            meshRenderers[i].material.color = Color.white;
        }
    }

}
