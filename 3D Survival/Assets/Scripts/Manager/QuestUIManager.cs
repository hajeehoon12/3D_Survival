using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIManager : MonoBehaviour
{
    public GameObject questListPanel; // 할당: QuestListPanel
    public GameObject questDetailPanel; // 할당: QuestDetailPanel
    public GameObject questListItemPrefab; // 할당: QuestListItem 프리팹
    public Transform questListContent; // 할당: QuestListPanel/Viewport/Content
    public TextMeshProUGUI noQuestsMessage; // 할당: QuestListPanel/NoQuestsMessage

    public TextMeshProUGUI questTitleText; // 할당: QuestDetailPanel/QuestTitleText
    public TextMeshProUGUI questDescriptionText; // 할당: QuestDetailPanel/QuestDescriptionText
    public TextMeshProUGUI questConditionText; // 할당: QuestDetailPanel/QuestConditionText
    public TextMeshProUGUI questRewardText; // 할당: QuestDetailPanel/QuestRewardText
    public TextMeshProUGUI questStatusText; // 할당: QuestDetailPanel/QuestStatusText

    private QuestController questController;
    
    void Start()
    {
        questController = CharacterManager.Instance.Player.GetComponent<QuestController>();
        questListPanel.SetActive(false);
        questDetailPanel.SetActive(false);
        PopulateQuestList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleQuestList();
        }
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
                questListItem.GetComponentInChildren<Text>().text = quest.title;
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
        questDescriptionText.text = quest.dialogueData.sentencesNotStarted.Length > 0 ? quest.dialogueData.sentencesNotStarted[0] : "No Description";
        questConditionText.text = quest.conditionText;
        questRewardText.text = quest.rewardText;
        questStatusText.text = quest.questStatus.ToString();
    }

    public void CloseQuestDetail()
    {
        questDetailPanel.SetActive(false);
        questListPanel.SetActive(true);
    }

    public void ToggleQuestList()
    {
        if (questListPanel.activeSelf || questDetailPanel.activeSelf)
        {
            questListPanel.SetActive(false);
            questDetailPanel.SetActive(false);
        }
        else
        {
            questListPanel.SetActive(true);
            PopulateQuestList();
        }
    }
}