using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    public bool isFP = true; // is First Person Camera?
    Ray startPoint;

    Camera curCamera;
    public Camera equipCamera;
    public float addDistance = 0;

    private bool isMoveEnd = true;

    private void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        curCamera = Camera.main;       
    }



    public void ChangeCamera()
    {
        if (isFP) CameraTP();
        else CameraFP();
    }


    private void CameraTP()
    {
        if (!isMoveEnd) return;
        equipCamera.enabled = false;
        isMoveEnd = false;
        //curCamera.transform.localPosition = new Vector3(0, 3, -5);
        curCamera.transform.DOLocalRotate(new Vector3(30, 0, 0), 1f);
        //curCamera.transform.localEulerAngles = new Vector3(30, 0, 0);
        curCamera.transform.DOLocalMove(new Vector3(0, 4, -3), 1f).OnComplete(() => 
        {   
            isMoveEnd = true;
            
            
            addDistance = 5f;
            isFP = false;

            CharacterManager.Instance.Player.controller.minXLook = -40f;
            CharacterManager.Instance.Player.controller.maxXLook = 40f;

        });
        
    }
    private void CameraFP()
    {

        if (!isMoveEnd) return;

        isMoveEnd = false;
        curCamera.transform.localEulerAngles = new Vector3(0, 0, 0);
        curCamera.transform.DOLocalMove(new Vector3(0, 1, 0), 1f).OnComplete(() =>
        {
            isMoveEnd = true;
            equipCamera.enabled = true;
            addDistance = 0;
            isFP = true;

            CharacterManager.Instance.Player.controller.minXLook = -80f;
            CharacterManager.Instance.Player.controller.maxXLook = 80f;

        });
        //curCamera.transform.localPosition = new Vector3(0, 1, 0);
    }

    public Ray StartPointofRay()
    {
        return startPoint;
    }



}
