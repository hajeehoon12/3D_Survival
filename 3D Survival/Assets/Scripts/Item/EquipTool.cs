using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    [Header("Resource Gathering")] // 자원 캐기 가능
    public bool doesGatherResources;

    [Header("Combat")] // 전투
    public bool doesDamage;
    public int damage;

    private Animator animator;
    private Camera camera;

    private void Start()
    {
        animator = GetComponent<Animator>();
        camera = Camera.main;
    }


    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    void OnCanAttack()
    {
        attacking = false;
    }

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource)) // 장비가 자원캐기용인지와 해당 레이캐스트가 자원인지 확인
            {
                resource.Gather(hit.point, hit.normal);
            }

            if (doesDamage && hit.collider.TryGetComponent(out NPC npc))
            {
                npc.TakePhysicalDamage(damage);
            }
        }
    }

        

}
