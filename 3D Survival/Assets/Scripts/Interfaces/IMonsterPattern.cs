public interface IMonsterPattern
{
    void ApplyOnStart(NPC npc);

    void ApplyOnUpdate(NPC npc);

    void ApplyOnTakeDamage(NPC npc);

}