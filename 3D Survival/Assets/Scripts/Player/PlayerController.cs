using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public bool isRunning = false;
    private bool isJumping = false;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    public bool canLook = true;

    public Action inventory;


    private Rigidbody _rigidbody;

    public UIInventory uiInventory;
    public GameObject uiInven;
    

    private void Awake()
    { 
        _rigidbody = GetComponent<Rigidbody>();
    }



    void Start()
    {
        uiInven.SetActive(false);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        if (isJumping)
        {
            if (IsGrounded())
            {
                isJumping = false;
                AudioManager.instance.PlaySFX("JumpToGround");
            }
        }
    }

    private void LateUpdate()
    {
        if(canLook) CameraLook();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jumper"))
        {
            GetComponent<Rigidbody>().AddForce(transform.up * GetComponent<Rigidbody>().mass * 20 , ForceMode.Impulse);
            AudioManager.instance.PlaySFX("Jumper");
            Debug.Log("Meet Jumper");
            isJumping = true;
        }
    }


    void Move()
    { 
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            dir *= moveSpeed * 2f; // run
        }
        else
        {
            isRunning = false;
            dir *= moveSpeed;  // walk
        }
        dir.y = _rigidbody.velocity.y; // gravity value 

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }




    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        { 
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();    
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() )
        {
            if(!isRunning)
                _rigidbody.AddForce(Vector2.up * jumPower * GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            else
                _rigidbody.AddForce(Vector2.up * jumPower * 1.2f *  GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            StartCoroutine(JumpBoolChange());
            AudioManager.instance.PlaySFX("Jump2");
        }
    }

    IEnumerator JumpBoolChange()
    {
        yield return new WaitForSeconds(0.1f);
        if(!IsGrounded())   isJumping = true;
    }


    public bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) // Player 가 걸리는 경우 제외
            {
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context) // 인벤 실행
    {
        if (context.phase == InputActionPhase.Started)
        {
            InvenOn();
        }
    }

    public void InvenOn()
    {
        uiInventory.Toggle();
    }

}
