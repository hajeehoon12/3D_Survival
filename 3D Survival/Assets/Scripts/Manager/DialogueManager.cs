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

    private Queue<string> sentences;
    private DialogueData currentDialogue;
    private Player player;
    private QuestController questController;

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
        sentences = new Queue<string>();
        UIDialogue.gameObject.SetActive(false);
        player = CharacterManager.Instance.Player;
    }

    public void StartDialogue(DialogueData dialogue)
    {   
        UIDialogue.gameObject.SetActive(true);
        currentDialogue = dialogue;
        // nextButton.gameObject.SetActive(true);
        // acceptButton.gameObject.SetActive(false);
        // declineButton.gameObject.SetActive(false);

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }

        if (sentences.Count == 0)
        {
            nextButton.gameObject.SetActive(false);
        }
    }

    // void UpdateButtonStates()
    // {
    //     bool isLastSentence = sentences.Count == 0;

    //     if (isLastSentence)
    //     {
    //         nextButton.gameObject.SetActive(false);
    //         if (player.GetCurrentQuestStatus() == QuestStatus.NotStarted)
    //         {
    //             acceptButton.gameObject.SetActive(true);
    //             declineButton.gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             declineButton.gameObject.SetActive(true);
    //         }
    //     }
    //     else
    //     {
    //         nextButton.gameObject.SetActive(true);
    //         declineButton.gameObject.SetActive(false);
    //         acceptButton.gameObject.SetActive(false);
    //     }
    // }


    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        sentences.Clear();
    }

    void ActivateDialogueUI()
    {
        UIDialogue.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(false);
    }

    void UpdateButtons()
    {
        //check nextBotton, AcceptButton, DeclineButton
        //if !=sentence.Length nextBotton On & DeclineButton On
    }

    public void AcceptQuest()
    {
        if (currentDialogue != null)
        {
            QuestData quest = FindQuestDataByDialogue(currentDialogue);
            if (quest != null)
            {
                questController.AddQuest(quest);
            }
        }
        EndDialogue();
    }

    public void DeclineQuest()
    {
        EndDialogue();
    }

    private QuestData FindQuestDataByDialogue(DialogueData dialogue)
    {
        foreach (var quest in questController.acceptedQuests)
        {
            if (quest.dialogueData == dialogue)
            {
                return quest;
            }
        }
        return null;
    }
}
