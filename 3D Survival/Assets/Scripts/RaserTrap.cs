using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RaserTrap : MonoBehaviour
{

    private bool isTrapActivate = false ;
    public GameObject MagicTrap;
    public RaycastHit hit;

    private float lastCheckTime;
    private float checkThreshold = 0.5f;
    Vector3 LaunchPower = new Vector3(0, 20, 0);//Vector3.zero;

    private void Start()
    {
        MagicTrap.SetActive(false);
    }

    private void Update()
    {
        TrapSensor();

    }

    private void TrapSensor()
    {

        if (isTrapActivate) return;

        if (Time.time - lastCheckTime > checkThreshold) // 시간검사
        {
            lastCheckTime = Time.time;

            if (Physics.SphereCast(transform.position - Vector3.up * 4f, 5f, Vector3.up, out hit, 5f, 1 << LayerMask.NameToLayer("Player")))
            {
                TrapOn();

            }

        }
    }

    private void TrapOn()
    {
        Debug.Log("You have just Activated Trap");
        isTrapActivate = true;
        StartCoroutine(TrapActivateMotion());

    }

    IEnumerator TrapActivateMotion()
    {
        yield return new WaitForSeconds(1f);
        MagicTrap.SetActive(true);

        hit.collider.transform.DOMove(transform.position, 5f).onComplete += PlatFormLaunch;
    }

    private void PlatFormLaunch()
    {
        StartCoroutine(PlayerLaunch());
    }

    IEnumerator PlayerLaunch()
    {
        float time = 0f;
        float timeThreshold = 0.5f;
        // Move() hold
        CharacterManager.Instance.Player.controller.canMove = false;
        while (time < 5f)
        {
            time += timeThreshold;
            Debug.Log("Force Restoring!!");

            hit.collider.transform.DOMove(transform.position, timeThreshold);

            if (Input.GetKey(KeyCode.W))
            {
                
                LaunchPower += hit.collider.transform.forward * 10;
            }
            if (Input.GetKey(KeyCode.A))
            {
                LaunchPower += hit.collider.transform.right * -10;
            }
            if (Input.GetKey(KeyCode.S))
            {
                LaunchPower += hit.collider.transform.forward * -10;
            }
            if (Input.GetKey(KeyCode.D))
            {
                LaunchPower += hit.collider.transform.right * 10;
            }

            yield return new WaitForSeconds(timeThreshold);
        }
        
        // Move() free
        hit.collider.GetComponent<Rigidbody>().AddForce(LaunchPower * hit.collider.GetComponent<Rigidbody>().mass , ForceMode.Impulse);

        yield return new WaitForSeconds(5f); // trap activated after 3 sec , reactivate for trap
        CharacterManager.Instance.Player.controller.canMove = true;
        MagicTrap.SetActive(false);
        isTrapActivate = false;
        //NuclearLaunch
        LaunchPower = new Vector3(0, 20, 0);

    }




}
