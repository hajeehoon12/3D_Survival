using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject; // 현재 RayCast에 걸린 게임 물체
    private IInteractable curInteractable; // 아이템 상호작용용 인터페이스
    private UpgradeHousing upgradeHousing;

    public Text promptText; // 아이템 상호작용 텍스트
    public Text promptDes;
    public GameObject promptObj;

    public Camera interactionCamera;

    private bool isUpgrading = false;

    private void Start()
    {
        interactionCamera = Camera.main;
    }

    private void Update() // 계속 체크하면서 시점의 가운데에 물체가 오는지 검사
    {

        if (Time.time - lastCheckTime > checkRate) // 시간검사
        {
            lastCheckTime = Time.time;

            Ray ray = interactionCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // 화면 중앙 기준 Ray발사
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance + CameraManager.instance.addDistance, layerMask)) // 충돌 됬을 때 hit에 정보넘김
            {
                if (hit.collider.gameObject != curInteractGameObject ) // 지금 만나고 있는게 넣어진게 아닐때 새 정보 넣음 Update 최적화
                {
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SafeZone"))
                    {
                        isUpgrading = true;
                        curInteractGameObject = hit.collider.gameObject;
                        upgradeHousing = hit.collider.GetComponent<UpgradeHousing>();
                        UpgradeText();
                    }
                    else
                    {
                        isUpgrading = false;
                        curInteractGameObject = hit.collider.gameObject;
                        curInteractable = hit.collider.GetComponent<IInteractable>();
                        SetPromptText();
                    }
                    // 프롬포트에 출력해줄꺼임
                }

            }
            else
            {
                upgradeHousing = null;
                isUpgrading = false;
                curInteractGameObject = null;
                curInteractable = null;
                promptObj.SetActive(false);
                //
                //Debug.Log("물체가아님");
            }
        }

    }

    private void UpgradeText()
    {
        promptObj.SetActive(true);

        promptObj.GetComponent<Image>().DOFade(0, 0f);
        promptText.DOFade(0, 0f);
        promptDes.DOFade(0, 0f);

        (promptText.text, promptDes.text) = ("Can Upgrade","Use 1 Upgrade Scroll to upgrade!!");

        promptObj.GetComponent<Image>().DOFade(1, 2f);
        promptText.DOFade(1, 2f);
        promptDes.DOFade(1, 2f);
    }





    private void SetPromptText()
    {
        promptObj.SetActive(true);
        
        promptObj.GetComponent<Image>().DOFade(0, 0f);
        promptText.DOFade(0, 0f);
        promptDes.DOFade(0, 0f);

        
        (promptText.text,promptDes.text) = curInteractable.GetInteractPrompt();

        promptObj.GetComponent<Image>().DOFade(1, 2f);
        promptText.DOFade(1, 2f);
        promptDes.DOFade(1, 2f);
    }

    public void OnInteractInput(InputAction.CallbackContext context) // E 키 누르면 실행
    {
        if (context.phase == InputActionPhase.Started && (curInteractable != null) || (upgradeHousing != null))// 
        {
            if (!isUpgrading)
            {
                curInteractable.OnInteract();
            }
            else
            {
                upgradeHousing.UpgradeHouse();
                
            }


            curInteractGameObject = null;
            curInteractable = null;
            upgradeHousing = null;
            promptObj.SetActive(false);
            isUpgrading = false;

        }
    }



}
