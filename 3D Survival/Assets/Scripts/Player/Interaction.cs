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

    public GameObject curInteractGameObject; // ���� RayCast�� �ɸ� ���� ��ü
    private IInteractable curInteractable; // ������ ��ȣ�ۿ�� �������̽�
    private UpgradeHousing upgradeHousing;

    public Text promptText; // ������ ��ȣ�ۿ� �ؽ�Ʈ
    public Text promptDes;
    public GameObject promptObj;

    public Camera interactionCamera;

    private bool isUpgrading = false;

    private void Start()
    {
        interactionCamera = Camera.main;
    }

    private void Update() // ��� üũ�ϸ鼭 ������ ����� ��ü�� ������ �˻�
    {

        if (Time.time - lastCheckTime > checkRate) // �ð��˻�
        {
            lastCheckTime = Time.time;

            Ray ray = interactionCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // ȭ�� �߾� ���� Ray�߻�
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance + CameraManager.instance.addDistance, layerMask)) // �浹 ���� �� hit�� �����ѱ�
            {
                if (hit.collider.gameObject != curInteractGameObject ) // ���� ������ �ִ°� �־����� �ƴҶ� �� ���� ���� Update ����ȭ
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
                    // ������Ʈ�� ������ٲ���
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
                //Debug.Log("��ü���ƴ�");
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

    public void OnInteractInput(InputAction.CallbackContext context) // E Ű ������ ����
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
