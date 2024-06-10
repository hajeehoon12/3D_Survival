using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDamage : MonoBehaviour
{
   
    public PlayerController playerController;
    public PlayerCondition condition;

    
    public LayerMask wallLayerMask;

    private float FallSec = 0.0f;

    public void CheckFalling()
    {
        if (playerController.IsGrounded())
        {
            Falldamage();
            FallSec = 0f;
        }
        else if(IsWallClimbing())
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

    public bool IsWallClimbing()
    {
        bool isWall = false;
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.1f) + (transform.up * 0.3f), Vector3.left),
            new Ray(transform.position + (transform.forward * 0.1f) + (transform.up * 0.3f), Vector3.right),
            new Ray(transform.position + (transform.forward * 0.1f) + (transform.up * 0.3f), Vector3.back),
            new Ray(transform.position + (transform.forward * 0.1f) + (transform.up * 0.3f), Vector3.forward),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.5f, wallLayerMask))
            {
                isWall = true;
            }
        }
        return isWall;
    }

    private void Update()
    {
        CheckFalling();
    }



}
