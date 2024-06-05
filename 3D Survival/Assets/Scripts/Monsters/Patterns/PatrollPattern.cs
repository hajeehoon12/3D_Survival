using UnityEngine;

public class PatrollPattern : MonoBehaviour, IMonsterPattern
{   

    //Since this script will be added with add component, the A and B coordinates should be reserved by specifying random values. ( scheduled )

    public Vector3 pointA;
    public Vector3 pointB;
    private bool movingToB = true;

    public PatrollPattern(Vector3 pointA, Vector3 pointB)
    {
        this.pointA = pointA;
        this.pointB = pointB;
    } 

    public void ApplyOnStart(NPC npc)
    {

    }


    public void ApplyOnUpdate(NPC npc)
    {
        Vector3 target = movingToB ? pointB : pointA;
        //npc.MoveTo(target);

        if (Vector3.Distance(npc.transform.position, target) < 0.1f)
        {
            movingToB = !movingToB;
        }
    }

    public void ApplyOnTakeDamage(NPC npc)
    {

    }

}