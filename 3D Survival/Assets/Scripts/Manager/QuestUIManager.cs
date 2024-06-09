// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class QuestUIManager : MonoBehaviour
// {
//     public GameObject questListItemPrefab;
//     public Transform questListContent;
//     public Text questTitleText;
//     public Text questDescriptionText;
//     public Text questConditionText;
//     public Text questRewardText;
//     public Text questStatusText;
//     public GameObject noQuestsMessage; // 받은 퀘스트가 없을 때 표시할 메시지

//     private QuestUIManager questManager;

//     void Start()
//     {
//         questManager = FindObjectOfType<QuestUIManager>();
//         PopulateQuestList();
//     }

//     void PopulateQuestList()
//     {
//         bool hasQuests = false;

//         foreach (QuestData quest in questManager.quests)
//         {
//             if (quest.questStatus != QuestStatus.NotStarted)
//             {
//                 hasQuests = true;
//                 GameObject questListItem = Instantiate(questListItemPrefab, questListContent);
//                 questListItem.GetComponentInChildren<Text>().text = quest.questTitle;
//                 Button questButton = questListItem.GetComponent<Button>();
//                 questButton.onClick.AddListener(() => DisplayQuestDetails(quest));
//             }
//         }

//         // 받은 퀘스트가 없다면 메시지 표시
//         noQuestsMessage.SetActive(!hasQuests);
//     }

//     void DisplayQuestDetails(Quest quest)
//     {
//         questTitleText.text = quest.title;
//         questDescriptionText.text = quest.questDescription;
//         questConditionText.text = quest.questClearConditionText;
//         questRewardText.text = quest.questClearRewardText;
//         questStatusText.text = quest.questStatus.ToString();
//     }

//     public void Toggle()
//     {
//         if (inventory.gameObject.activeSelf) inventory.gameObject.SetActive(false);

//         if (craftWindow.activeSelf)
//         {
//             craftWindow.SetActive(false);
//             AudioManager.instance.PlaySFX("InventoryOff");
//             UnityEngine.Cursor.lockState = CursorLockMode.Locked;
//             controller.canLook = true;
//         }
//         else
//         {
//             AudioManager.instance.PlaySFX("Inventory");
//             craftWindow.SetActive(true);
//             UnityEngine.Cursor.lockState = CursorLockMode.None;

//             controller.canLook = false;
//         }
//     }

// }
