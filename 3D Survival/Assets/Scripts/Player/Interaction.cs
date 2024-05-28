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

    public Text promptText; // ������ ��ȣ�ۿ� �ؽ�Ʈ
    public Text promptDes;
    public GameObject promptObj;

    private Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update() // ��� üũ�ϸ鼭 ������ ����� ��ü�� ������ �˻�
    {

        if (Time.time - lastCheckTime > checkRate) // �ð��˻�
        {
            lastCheckTime = Time.time;

            Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2)); // ȭ�� �߾� ���� Ray�߻�
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask)) // �浹 ���� �� hit�� �����ѱ�
            {
                if (hit.collider.gameObject != curInteractGameObject) // ���� ������ �ִ°� �־����� �ƴҶ� �� ���� ���� Update ����ȭ
                {
                    curInteractGameObject = hit.collider.gameObject;
                    curInteractable = hit.collider.GetComponent<IInteractable>();
                    SetPromptText();
                    // ������Ʈ�� ������ٲ���
                }

            }
            else
            {
                curInteractGameObject = null;
                curInteractable = null;
                promptObj.SetActive(false);
                //
                //Debug.Log("��ü���ƴ�");
            }
        }

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
        if (context.phase == InputActionPhase.Started && curInteractable != null) // 
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptObj.SetActive(false);
        }
    }



}
