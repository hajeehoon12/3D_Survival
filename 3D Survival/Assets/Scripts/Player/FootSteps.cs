using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;

    private PlayerController playerController;

    private Rigidbody _rigidbody;

    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerController.IsGrounded()) // 높이 변화가 거의없다시피 할 때
        {
            if (!playerController.isRunning)
            {
                if (_rigidbody.velocity.magnitude > footstepThreshold)
                {
                    if (Time.time - footStepTime > footstepRate)
                    {
                        footStepTime = Time.time;
                        AudioManager.instance.PlaySFX("FootStep", 0.3f);
                    }
                }
            }
            else
            {
                if (_rigidbody.velocity.magnitude > footstepThreshold/1.5f)
                {
                    if (Time.time - footStepTime > footstepRate/2f)
                    {
                        footStepTime = Time.time;
                        AudioManager.instance.PlaySFX("FootStep", 0.2f);
                    }
                }
            }
        }
    }

}
