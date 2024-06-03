using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MovingPlatform : MonoBehaviour
{
    Rigidbody rigid;

    Rigidbody _player;


    [SerializeField] float moveSpeed;
    Vector3 moveVec;
    Vector3 dirVec;

    Coroutine repeatCoroutine;

    public enum MoveDirection
    {
        Vertical,
        Horizontal,
        UpperLeft_LowerRight,
        UpperRight_LowerLeft
    }
    [SerializeField] MoveDirection myDir;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        myDir = 0;
    }

    void WhenPlayerGetIn()
    {

        
        switch (myDir)
        {
            case MoveDirection.Vertical:
                dirVec = Vector3.forward;
                break;
            case MoveDirection.Horizontal:
                dirVec = Vector3.right;
                break;
            case MoveDirection.UpperLeft_LowerRight:
                dirVec = new Vector3(0, 0, -1).normalized;
                break;
            case MoveDirection.UpperRight_LowerLeft:
                dirVec = new Vector3(-1, 0, 0).normalized;
                break;
            default:
                break;
        }


        moveVec = dirVec * moveSpeed * Time.fixedDeltaTime;
        rigid.MovePosition(transform.position + moveVec);

        if (_player != null)
        {
            _player.MovePosition(_player.position + moveVec);
        }
    }



    IEnumerator RepeatCycle()
    {
        while(true)
        {
            myDir += 1;
            if ((int)myDir == 4) myDir = 0;
            yield return new WaitForSeconds(5f);
            
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        AudioManager.instance.PlayBGM2("Macha", 0.5f);
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerIn");
            repeatCoroutine = StartCoroutine(RepeatCycle());
            _player = collision.gameObject.GetComponent<Rigidbody>();
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            WhenPlayerGetIn();
        }
    }



    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.StopBGM2();
            Debug.Log("PlayerOut");
            _player = null;
            StopCoroutine(repeatCoroutine);
        }
    }



}