using UnityEngine;
using UnityEngine.AI;

public class PatrollPattern : MonoBehaviour, IMonsterPattern
{
    private Vector3 centerPoint;
    private float patrolRadius = 10f;
    private NavMeshAgent agent;
    private bool inBattle = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void ApplyOnStart(NPC npc)
    {   
        npc.SetState(AIState.Patrolling);
        centerPoint = npc.transform.position;
        MoveToNewPatrolPoint(npc);
    }

    public void ApplyOnUpdate(NPC npc)
    {
        if (!inBattle)
        {
            if (agent.remainingDistance < 0.1f)
            {
                MoveToNewPatrolPoint(npc);
            }

            float playerDistance = Vector3.Distance(npc.transform.position, CharacterManager.Instance.Player.transform.position);
            if (npc.IsPlayerInFieldOfView() && playerDistance < npc.detectDistance)
            {   
                inBattle = true;
                npc.SetState(AIState.Attacking);
            }
        }
    }

    public void ApplyOnTakeDamage(NPC npc)
    {
        
    }

    private void MoveToNewPatrolPoint(NPC npc)
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += centerPoint;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas);
        agent.SetDestination(hit.position);
    }
}