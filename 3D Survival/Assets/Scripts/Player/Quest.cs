using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public List<QuestData> acceptedQuests = new List<QuestData>();

    public void AddQuest(QuestData quest)
    {
        if (!acceptedQuests.Contains(quest))
        {
            acceptedQuests.Add(quest);
            quest.questStatus = QuestStatus.InProgress;
            Debug.Log($"Quest '{quest.title}' added.");
        }
    }

}
