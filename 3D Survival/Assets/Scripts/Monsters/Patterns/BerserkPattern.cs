using UnityEngine;

public class BerserkPattern : MonoBehaviour, IMonsterPattern
{
    private bool isApplied = false;
    private float attackMultiplier = 1.2f;
    private float speedMultiplier = 0.8f;
    
    public void ApplyOnStart(NPC npc)
    {

    }


    public void ApplyOnUpdate(NPC npc)
    {

    }

    public void ApplyOnTakeDamage(NPC npc)
    {
        if (!isApplied && npc.health <= npc.maxHealth * 0.5f)
        {
            npc.damage = Mathf.CeilToInt(npc.damage * attackMultiplier);
            npc.attackRate *= speedMultiplier;
            isApplied = true;
        }
    }
}
