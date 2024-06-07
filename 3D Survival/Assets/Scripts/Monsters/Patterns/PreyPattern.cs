using UnityEngine;

public class PreyPattern : MonoBehaviour, IMonsterPattern
{
    public void ApplyPattern(NPC npc)
    {
        
    }

    public void ApplyOnStart(NPC npc)
    {

    }


    public void ApplyOnUpdate(NPC npc)
    {
        if (npc.playerDistance < npc.detectDistance && npc.IsPlayerInFieldOfView() ||
            npc.playerDistance < npc.detectDistance / 3)
        {
            npc.SetState(AIState.Fleeing);
        }
    }

    public void ApplyOnTakeDamage(NPC npc)
    {

    }

}