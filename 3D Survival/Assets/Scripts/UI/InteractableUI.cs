using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUI : MonoBehaviour
{
    [Header("Layer Settings")]
    public LayerMask havingInfoLayerMask;
    public float toCheckThreshold;
    public float checkInfoDistance;

    private float lastCheckTime;

    public RaycastHit hit;

    public bool isTPressed = false;
    public bool doingRoutine = false;

    private void Update()
    {

        CheckingMousePointer();

        if ((Time.time - lastCheckTime > toCheckThreshold) && isTPressed) // 시간검사
        {
            lastCheckTime = Time.time;

            //Vector3 ray = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.Log(ray);
            //Vector3 dir =  (ray - Camera.main.transform.position).normalized;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast( ray,out hit, checkInfoDistance + CameraManager.instance.addDistance, havingInfoLayerMask)) // transform.position, dir
            {
                Debug.Log(hit.collider.gameObject.name);
                ShowSpecialUI(hit.collider.gameObject.tag);
            }

        }
    }

    private void CheckingMousePointer()
    {
        if (Input.GetKey(KeyCode.Y) && !doingRoutine)
        {
            doingRoutine = true;
            isTPressed = true;
            StartCoroutine(ActiveMousePoint());

        }
    }

    IEnumerator ActiveMousePoint()
    {
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(10f);
        Cursor.lockState = CursorLockMode.Locked;
        doingRoutine = false;
        isTPressed= false;
    }


    public void ShowSpecialUI(string tag)
    {
        // if wall // Press Ctrl to Climb // and Press "W" + "S to climb up and down
        // if Trap // Becareful there is a hidden Trap!! // It'll pull you into the Center and shoot like a cannon Press "direction key" to shoot your body
        // if NPC // There is a Scary Bear // Equip your weapon and Press "Left Mouse Button" to fight against it
        // if interactable // There is a item you can achieve // Press "E" to get it
        // if Fire // Becareful you'll get damaged beneath the fire // Runaway from it!!
        // if Resources // You can achieve Items with axe // Equip your axe and Press "Left Mouse Button" to get it
        // if MovingPlatform // You can move automatically in moving platform // You can escape using Jump!!

    }


}
