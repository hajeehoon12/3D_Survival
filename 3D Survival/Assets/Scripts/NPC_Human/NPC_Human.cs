using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NPC_Human : MonoBehaviour, IInteractable
{   
    [SerializeField] private float distanceFromPlayer;
    [SerializeField] private Transform playerTransform;
    public float sightDistance = 5f;
    public float rotationSpeed = 2f;
    public QuestData questData;

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.rotation;
    }
    void Update()
    {
        playerTransform = CharacterManager.Instance.Player.transform;
        distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);
    }


    void LateUpdate()
    {   
        if (distanceFromPlayer <= sightDistance)
        {   
            Vector3 direction = playerTransform.position - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            // 초기 회전 값으로 돌아가기
            transform.rotation = Quaternion.Slerp(transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public (string, string) GetInteractPrompt()
    {
        string str = $"묘지기";
        string str1 = $"날 좀 도와줄 수 있나?";
        return (str,str1);
    }

    public void OnInteract()
    {
        DialogueManager.Instance.StartDialogue(questData);

    }

}
