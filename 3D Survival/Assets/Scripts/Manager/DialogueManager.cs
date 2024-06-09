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

    private Queue<string> sentences;
    private DialogueData currentDialogue;
    private Player player;

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
        dialoguePanel.SetActive(true);
        nextButton.gameObject.SetActive(true);

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

}
