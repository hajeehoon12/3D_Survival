using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{   
    public static QuestUIManager instance;
    public static QuestUIManager Instance
    {
        get 
        {
            if (instance == null)
            { 
                instance = new GameObject("QuestUIManager").AddComponent<QuestUIManager>();
            }
            return instance;
        }
    }

    public GameObject questUI;
    public GameObject questListPanel;
    public GameObject questDetailPanel;
    public GameObject questListItemPrefab;
    public Transform questListContent;
    public TextMeshProUGUI noQuestsMessage;

    public TextMeshProUGUI questTitleText;
    public TextMeshProUGUI questDescriptionText;
    public TextMeshProUGUI questConditionText;
    public TextMeshProUGUI questRewardText;
    public TextMeshProUGUI questStatusText;

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
        questController = CharacterManager.Instance.Player.GetComponent<QuestController>();
        Debug.Log(questController == null);
        Debug.Log("??");
    }

    void PopulateQuestList()
    {
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }

        bool hasQuests = false;

        foreach (QuestData quest in questController.acceptedQuests)
        {
            if (quest.questStatus != QuestStatus.NotStarted)
            {
                hasQuests = true;
                GameObject questListItem = Instantiate(questListItemPrefab, questListContent);
                questListItem.GetComponentInChildren<TextMeshProUGUI>().text = quest.title;
                Button questButton = questListItem.GetComponent<Button>();
                questButton.onClick.AddListener(() => DisplayQuestDetails(quest));
            }
        }

        noQuestsMessage.gameObject.SetActive(!hasQuests);
    }

    void DisplayQuestDetails(QuestData quest)
    {
        questDetailPanel.SetActive(true);
        questListPanel.SetActive(false);

        questTitleText.text = quest.title;
        questDescriptionText.text = String.Format($"{quest.descriptionText.Replace("\\n", "\n")}");
        questConditionText.text = String.Format($"클리어 조건: {quest.conditionText.Replace("\\n", "\n")}");
        questRewardText.text = String.Format($"보상: {quest.rewardText}");
        questStatusText.text = StatusToString(quest.questStatus);

    }

    public string StatusToString(QuestStatus Status)
    {   
        string str="default";

        switch(Status)
        {
            case QuestStatus.NotStarted:
            case QuestStatus.InProgress:
            str = "(진행중)";
            break;
            case QuestStatus.Completed:
            str = "(완료됨)";
            break;
            
        }
        return str;
    }

    public void CloseQuestDetail()
    {
        questDetailPanel.SetActive(false);
        questListPanel.SetActive(true);
    }

    public void ToggleQuestList()
    {
        if (!questListPanel.activeSelf && questDetailPanel.activeSelf)
        {
            questListPanel.SetActive(true);
            questDetailPanel.SetActive(false);
            PopulateQuestList();
        }
        else
        {
            questListPanel.SetActive(false);
            questDetailPanel.SetActive(true);
            
        }
    }

    public void ToggleUI()
    {
        if (questUI.gameObject.activeSelf)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
            questListPanel.SetActive(true);
            PopulateQuestList();
        }
    }

    public void TurnOn()
    {   
        questUI.gameObject.SetActive(true);
        questDetailPanel.SetActive(false);
        AudioManager.instance.PlaySFX("Inventory");
        Cursor.lockState = CursorLockMode.None;
        CharacterManager.Instance.Player.controller.canLook = false;
    }

    public void TurnOff()
    {   
        questUI.gameObject.SetActive(false);
        questListPanel.SetActive(false);
        questDetailPanel.SetActive(false);
        AudioManager.instance.PlaySFX("InventoryOff");
        Cursor.lockState = CursorLockMode.Locked;
        CharacterManager.Instance.Player.controller.canLook = true;
    }

}