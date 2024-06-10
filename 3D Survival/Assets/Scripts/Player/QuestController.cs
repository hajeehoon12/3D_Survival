using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public List<QuestData> acceptedQuests = new List<QuestData>();

    public void AddQuest(QuestData quest)
    {
        if (!acceptedQuests.Contains(quest))
        {
            acceptedQuests.Add(quest);
            quest.questStatus = QuestStatus.InProgress;
            Debug.Log($"Quest '{quest.title}' 추가됨.");
        }
    }

    // public void ClearQuest(QuestData quest)
    // {
    //     if (acceptedQuests.Contains(quest))
    //     {
    //         quest.questStatus = QuestStatus.Completed;
    //         quest.GiveClearReward(gameObject);
    //         Debug.Log($"Quest '{quest.title}' 클리어됨.");
    //     }
    // }

    public QuestStatus GetQuestStatus(QuestData quest)
    {
        if (acceptedQuests.Contains(quest))
        {
            return quest.questStatus;
        }
        return QuestStatus.NotStarted;
    }


}
