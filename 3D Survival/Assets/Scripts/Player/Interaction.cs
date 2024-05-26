using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Interaction : MonoBehaviour
{
    public float checkRate = 0.05f;
    private float lastCheckTime;
    public float maxCheckDistance;
    public LayerMask layerMask;

    public GameObject curInteractGameObject; // ���� RayCast�� �ɸ� ���� ��ü
    private IInteractable curInteractable; // ������ ��ȣ�ۿ�� �������̽�

    public TextMeshProUGUI promptText; // ������ ��ȣ�ۿ� �ؽ�Ʈ
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
                promptText.gameObject.SetActive(false);
                //
                //Debug.Log("��ü���ƴ�");
            }
        }

    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = curInteractable.GetInteractPrompt();
    }

    public void OnInteractInput(InputAction.CallbackContext context) // E Ű ������ ����
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null) // 
        {
            curInteractable.OnInteract();
            curInteractGameObject = null;
            curInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }



}
