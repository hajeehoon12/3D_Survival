using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.Actions.MenuPriority;


public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public LayerMask wallLayerMask;
    public bool isRunning = false;
    private bool isJumping = false;
    public bool SpeedBuff = false;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    [Header("Contruct Mode")]
    public GameObject constructPrefab;
    public bool constructMode = false;
    public GameObject spawnConstructPrefab;
    public GameObject virtualSpawn;
    public GameObject virtualConstructGreen;
    public GameObject virtualConstructRed;


    public bool canLook = true;

    public Action inventory;
    public bool canMove = true;
    


    private Rigidbody _rigidbody;

    public UIInventory uiInventory;
    public GameObject uiInven;
    

    private void Awake()
    { 
        _rigidbody = GetComponent<Rigidbody>();
        
    }



    void Start()
    {
        
        uiInven.SetActive(true);
        CharacterManager.Instance.Player.addItem += uiInventory.AddItem;
        Cursor.lockState = CursorLockMode.Locked;
        constructMode = false;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (constructMode)
        {
            DoConstructMode();
            //return; // if Memory Leak go return
        }

        Move();

        if (isJumping)
        {
            if (IsGrounded())
            {
                isJumping = false;
                AudioManager.instance.PlaySFX("JumpToGround");
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Debug.Log("I'm Pressing Ctrl");
            if (IsWallClimbing())
            {
                //Debug.Log(IsWallClimbing());
                Climb();
            }
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            CameraManager.instance.ChangeCamera();
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
            StartCoroutine(JumpBoolChange());
        }
    }


    private void DoConstructMode()
    {

        if (virtualSpawn == null)
        {
            virtualSpawn= Instantiate(virtualConstructGreen);
        }


        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        

        if (Physics.Raycast(ray, out hit, 10f + CameraManager.instance.addDistance, (1 << LayerMask.NameToLayer("Ground"))))
        {
            Debug.Log(hit.point);
            virtualSpawn.transform.position=(hit.point)+ new Vector3(0, 3, 0);
            virtualSpawn.transform.LookAt(transform.position);
            virtualSpawn.transform.localEulerAngles = new Vector3(0, virtualSpawn.transform.localEulerAngles.y, virtualSpawn.transform.localEulerAngles.z);
        }


        if (Input.GetMouseButtonDown(0)) // left mouse button = do construct target object
        {
            // Do construct
            
            virtualSpawn.SetActive(false);
            spawnConstructPrefab = Instantiate(constructPrefab);
            spawnConstructPrefab.transform.position = virtualSpawn.transform.position;
            constructPrefab = null;
            Destroy(virtualSpawn);
            virtualSpawn = null;
            //uiInventory.ConstructCancel();
            constructMode = false;
            
        }
        if (Input.GetKeyDown(KeyCode.Escape)) // Esc button = cancel construct
        {
            // cancel constructmode
            constructPrefab = null;
            virtualSpawn = null;
            uiInventory.ConstructCancel();
            constructMode = false;
            
        }
    }




    private void Climb() // Climbing
    {
        Debug.Log("Climbing");
        float climbDir = Input.GetAxisRaw("Vertical");
        _rigidbody.velocity = new Vector3(0, climbDir * 8 -2f, 0);
        StartCoroutine(ClimbEffect());
    }

    IEnumerator ClimbEffect()
    {
        yield return new WaitForSeconds(0.5f);
        
    }


    void Move()
    {
        if (!canMove) return; // Don't execute Move if player can't move

        float speedVol = 1f;

        if (SpeedBuff) speedVol = 2f;

        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        if (Input.GetKey(KeyCode.LeftShift) && CharacterManager.Instance.Player.condition.UseStamina(0.4f))
        {
            isRunning = true;
            dir *= moveSpeed * 2f * speedVol; // run
        }
        else
        {
            isRunning = false;
            dir *= moveSpeed * speedVol;  // walk
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
        if (context.phase == InputActionPhase.Started && IsGrounded() && (CharacterManager.Instance.Player.condition.UseStamina(10) ) )
        {

            if(!isRunning)
                _rigidbody.AddForce(Vector2.up * jumPower * GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            else
                _rigidbody.AddForce(Vector2.up * jumPower * 1.2f *  GetComponent<Rigidbody>().mass, ForceMode.Impulse);
            StartCoroutine(JumpBoolChange());

            AudioManager.instance.PlaySFX("Jump");
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
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask)) // Player �� �ɸ��� ��� ����
            {
                return true;
            }
        }
        return false;
    }

    public bool IsWallClimbing()
    {
        bool isWall = false;
        Ray[] rays = new Ray[2]
        {
            new Ray(transform.position + (transform.right * 0.01f) + (-transform.up * 0.3f), transform.forward),
            new Ray(transform.position + (-transform.right * 0.01f) + (-transform.up * 0.3f), transform.forward)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            //Debug.Log("Inspect");
            if (Physics.Raycast(rays[i], 0.5f, wallLayerMask)) // Player �� �ɸ��� ��� ����
            {
                isWall = true;
            }
        }
        return isWall;
    }


    public void OnInventory(InputAction.CallbackContext context) // �κ� ����
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
