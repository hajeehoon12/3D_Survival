using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public enum LayerNum
{
    Interactable = 7,
    Resource = 9,
    Wall = 10,
    Trap = 11,
    Fire = 12,
    MovingPlatform = 13,
    Jumper =14,
    NPC = 15

}



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

    public GameObject InterfaceUI;

    public TMP_Text LayerName;
    //public TMP_Text LayerDesc;
    public TMP_Text InteractMethod;


    private void Start()
    {
        InterfaceUI.gameObject.SetActive(false);
    }


    private void Update()
    {

        CheckingMousePointer();
        //ActivateMousePointer();

        if ((Time.time - lastCheckTime > toCheckThreshold) && isTPressed) // 시간검사
        {
            lastCheckTime = Time.time;

            //Vector3 ray = (Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Debug.Log(ray);
            //Vector3 dir =  (ray - Camera.main.transform.position).normalized;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            

            if (Physics.Raycast( ray,out hit, checkInfoDistance + CameraManager.instance.addDistance, havingInfoLayerMask)) // transform.position, dir
            {            
                ShowSpecialUI(hit);    
            }

        }
    }

    private void CheckingMousePointer() // mouse pointer activate for 10sec
    {
        if (Input.GetKey(KeyCode.Y) && !doingRoutine)
        {
            doingRoutine = true;
            isTPressed = true;
            StartCoroutine(ActiveMousePointfor10Sec());

        }
    }

    void ActivateMousePointer() // activate not auto ,, trashed it
    {
        if (Input.GetKey(KeyCode.Y) && !isTPressed)
        {
            isTPressed = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            isTPressed = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }



    IEnumerator ActiveMousePointfor10Sec() // activate mouse for 10 sec
    {
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(5f);
        InterfaceUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        doingRoutine = false;
        isTPressed= false;
    }


    private void ShowSpecialUI(RaycastHit hit)
    {
        InterfaceUI.SetActive(true);
        //InterfaceUI.transform.position = hit.collider.gameObject.transform.position;
        InterfaceUI.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + (hit.distance ) * transform.forward ;

        InterfaceUI.transform.rotation = transform.rotation;
        //Debug.Log((LayerNum)hit.collider.gameObject.layer);
        Debug.Log(((LayerNum)(hit.collider.gameObject.layer)).ToString());

        LayerName.text = ((LayerNum)(hit.collider.gameObject.layer)).ToString();

        ShowDataBase( (LayerNum)hit.collider.gameObject.layer );


        // if wall // Press Ctrl to Climb // and Press "W" + "S to climb up and down
        // if Trap // Becareful there is a hidden Trap!! // It'll pull you into the Center and shoot like a cannon Press "direction key" to shoot your body
        // if NPC // There is a Scary Bear // Equip your weapon and Press "Left Mouse Button" to fight against it
        // if interactable // There is a item you can achieve // Press "E" to get it
        // if Fire // Becareful you'll get damaged beneath the fire // Runaway from it!!
        // if Resources // You can achieve Items with axe // Equip your axe and Press "Left Mouse Button" to get it
        // if MovingPlatform // You can move automatically in moving platform // You can escape using Jump!!
        // if Jumper // There is a Jump Zone // You can get forced into sky!!

    }

    private void ShowDataBase(LayerNum layerName)
    {

        


        switch (layerName)
        {
            case LayerNum.Wall:

                //LayerDesc.text = "Press Ctrl to Climb";
                InteractMethod.text = "Press \"W\" + \"S\" to climb up and down";

                break;
            case LayerNum.Trap:

                //LayerDesc.text = "Becareful there is a hidden Trap!!";
                InteractMethod.text = "It'll pull you into the Center and shoot like a cannon Press \"direction key\" to shoot your body";

                break;
            case LayerNum.NPC:

                //LayerDesc.text = "There is a Scary Bear";
                InteractMethod.text = "Equip your weapon and Press \"Left Mouse Button\" to fight against it";

                break;
            case LayerNum.Interactable:

                //LayerDesc.text = "There is a item you can achieve";
                InteractMethod.text = "Press \"E\" to get it!!";

                break;
            case LayerNum.Fire:

                //LayerDesc.text = "Becareful!! you'll get damaged beneath the fire";
                InteractMethod.text = "Get far distance from it!!";

                break;
            case LayerNum.Resource:

                //LayerDesc.text = "You can achieve Resources with axe";
                InteractMethod.text = "Equip your axe and Press \"Left Mouse Button\" to get it";

                break;
            case LayerNum.MovingPlatform:

                //LayerDesc.text = "You can move automatically in moving platform";
                InteractMethod.text = "You can escape using Jump!!";

                break;
            case LayerNum.Jumper:

                //LayerDesc.text = "There is a Jump Zone";
                InteractMethod.text = "You can get forced into sky!!";

                break;
            default:

                //LayerDesc.text = "";
                InteractMethod.text = "";

                break;
        
        }
    }







}
