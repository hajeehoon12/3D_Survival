using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{   
    public static DialogueManager instance;
    public static DialogueManager Instance
    {
        get 
        {
            if (instance == null)
            { 
                instance = new GameObject("DialogueManager").AddComponent<DialogueManager>();
            }
            return instance;
        }
    }

    public Transform UIDialogue;
    public TMP_Text dialogueText;
    public GameObject dialoguePanel;
    public Button nextButton;
    public Button acceptButton;
    public Button declineButton;

    private Player player;
    private QuestController questController;

    private Queue<string> currentSentences;
    private DialogueData currentDialogue;
    private QuestData currentQuestData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance == this)
            { 
                Destroy(gameObject);
            }
        }
    }

    void Start()
    {
        player = CharacterManager.Instance.Player;
        questController = player.GetComponent<QuestController>();
        currentSentences = new Queue<string>();
        UIDialogue.gameObject.SetActive(false);
    }

    public void StartDialogue(QuestData questData)
    {   
        TurnOn();
        currentDialogue = questData.dialogueData;
        currentQuestData = questData;

        currentSentences.Clear();

        SelectSentence();
        DisplayNextSentence();
    }

    public void SelectSentence ()
    {
        switch (currentQuestData.questStatus)
        {
            case QuestStatus.NotStarted:
                foreach (string sentence in currentDialogue.sentencesNotStarted)
                {
                    currentSentences.Enqueue(sentence);
                }
                break;
            case QuestStatus.InProgress:
                foreach (string sentence in currentDialogue.sentencesAccepted)
                {
                    currentSentences.Enqueue(sentence);
                }
                break;
            case QuestStatus.Completed:
                foreach (string sentence in currentDialogue.sentencesCleared)
                {
                    currentSentences.Enqueue(sentence);
                }
                Quest.Instance.RewardItem();
                AudioManager.instance.PlaySFX("Success", 0.5f);
                break;
        }
    }
    public void DisplayNextSentence()
    {
        if (currentSentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        UpdateButtonStates();
        string sentence = currentSentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {   
        UpdateButtonStates();
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void UpdateButtonStates()
    {
        bool isLastSentence = currentSentences.Count == 0;

        if (isLastSentence)
        {
            nextButton.gameObject.SetActive(false);
            if (currentQuestData.questStatus == QuestStatus.NotStarted)
            {
                acceptButton.gameObject.SetActive(true);
                declineButton.gameObject.SetActive(true);
            }
            else
            {
                declineButton.gameObject.SetActive(true);
            }
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            declineButton.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(false);
        }
    }


    void EndDialogue()
    {
        TurnOff();
        currentSentences.Clear();
    }

    public void AcceptQuest()
    {   
        questController.AddQuest(currentQuestData);
        EndDialogue();
    }

    public void DeclineQuest()
    {
        EndDialogue();
    }

    public void TurnOn()
    {   
        UIDialogue.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Inventory");
        Cursor.lockState = CursorLockMode.None;
        CharacterManager.Instance.Player.controller.canLook = false;
    }

    public void TurnOff()
    {   
        UIDialogue.gameObject.SetActive(false);
        AudioManager.instance.PlaySFX("InventoryOff");
        Cursor.lockState = CursorLockMode.Locked;
        CharacterManager.Instance.Player.controller.canLook = true;
    }
}
