using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
   
    public PlayerController playerController;
    public PlayerCondition condition;

    private float FallSec = 0.0f;

    public void CheckFalling()
    {
        if (playerController.IsGrounded())
        {
            Falldamage();
            FallSec = 0f;
        }
        else if(playerController.IsWallClimbing())
        {
            FallSec = 0f;
        }
        else
        {
            FallSec += Time.deltaTime;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jumper"))
        {
            StartCoroutine(MeetJumper());
        }
    }

    IEnumerator MeetJumper()
    {
        yield return new WaitForSecondsRealtime(3.5f);
        FallSec = 0f;
    }
    public void Falldamage()
    {
        if (FallSec > 2f && FallSec < 3f)
        {
            AudioManager.instance.PlaySFX("SFX_Click_Punch");
            condition.Heal(-30);
            FallSec = 0f;
        }
        else if (FallSec > 3f && FallSec < 4f)
        {
            AudioManager.instance.PlaySFX("SFX_Click_Punch");
            condition.Heal(-50);
            FallSec = 0f;
        }
        else if (FallSec > 4f)
        {
            AudioManager.instance.PlaySFX("SFX_Click_Punch");
            condition.Heal(-80);
            FallSec = 0f;
        }
    }

    private void Update()
    {
        CheckFalling();
        Debug.Log(FallSec);
    }



}
