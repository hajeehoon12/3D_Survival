using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    public bool isAttacking = false;

    [Header("Resource Gathering")] // �ڿ� ĳ�� ����
    public bool doesGatherResources;

    [Header("Combat")] // ����
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
                StartCoroutine(OnCanAttack());
            }
        }
    }

    IEnumerator OnCanAttack()
    {
        yield return new WaitForSeconds(attackRate);
        attacking = false;
    }

    

    public void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        isAttacking = true;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource)) // ��� �ڿ�ĳ��������� �ش� ����ĳ��Ʈ�� �ڿ����� Ȯ��
            {
                resource.Gather(hit.point, hit.normal);
            }

            if (doesDamage && hit.collider.TryGetComponent(out NPC npc))
            {
                npc.TakePhysicalDamage(damage);
                AudioManager.instance.PlaySFX("SwordAttack2", 0.8f);
                isAttacking = false;
            }
            else
            {
                
            }
            
        }
        if (isAttacking)
        {
            AudioManager.instance.PlaySFX("SwordAttack", 0.8f);
            isAttacking = false;
        }
    }

        

}
