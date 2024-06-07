using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipPickaxe : Equip
{
    public float attackRate;
    private bool attacking;
    public float attackDistance;
    public float useStamina;

    public bool isAttacking = false;

    [Header("Resource Gathering")]
    public bool doesGatherRock;

    [Header("Combat")] // 
    public bool doesDamage;
    public int damage;

    private Animator animator;
    private Camera pickaxeCamera;

    //int playerLayerMaskException = (1 << LayerMask.NameToLayer("Player"));

    private void Start()
    {
        animator = GetComponent<Animator>();
        pickaxeCamera = Camera.main;
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
        Ray ray = pickaxeCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        isAttacking = true;

        if (Physics.Raycast(ray, out hit, attackDistance + CameraManager.instance.addDistance, ~(1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Interactable") )))
        {
            if (doesGatherRock && hit.collider.TryGetComponent(out Resource rockResource) && hit.collider.CompareTag("Rocks")) // Hit Resources
            {
                //Debug.Log("Wood Resourcing");
                rockResource.Gather(hit.point, hit.normal);

                if (gameObject.name == "Equip_Pickaxe(Clone)")
                {
                    AudioManager.instance.PlaySFX("Axe");
                    isAttacking = false;
                    //return;
                }

            }

            if (doesDamage)
            {
                if (hit.collider.TryGetComponent(out NPC npc)) // Hit NPC
                {
                    npc.TakePhysicalDamage(damage);
                    //if (gameObject.name == "Equip_Sword")
                    //{
                    AudioManager.instance.PlaySFX("SwordAttack2", 0.8f);
                    //} 
                    isAttacking = false;
                    //return;
                }
            }

        }
        if (isAttacking) // FFul Swing
        {
            //if (gameObject.name == "Equip_Sword(Clone)")
            //{
            AudioManager.instance.PlaySFX("SwordAttack", 0.8f);
            //}
            isAttacking = false;
            return;
        }
    }



}