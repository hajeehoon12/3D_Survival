using UnityEngine;

public class CowardPattern : MonoBehaviour, IMonsterPattern
{

    public void ApplyOnStart(NPC npc)
    {

    }


    public void ApplyOnUpdate(NPC npc)
    {

    }

    public void ApplyOnTakeDamage(NPC npc)
    {
        if (npc.health <= npc.maxHealth * 0.3f)
        {
            npc.SetState(AIState.Fleeing);
        }
    }


}
