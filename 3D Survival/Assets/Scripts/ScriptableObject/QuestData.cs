using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QuestStatus
{
    NotStarted,
    InProgress,
    Completed
}

[CreateAssetMenu(fileName = "NewQuestData" , menuName = "QuestSystem/Quest Data")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string title;
    [TextArea(1,10)] public string descriptionText;
    [TextArea(1,10)] public string conditionText;
    [TextArea(1,10)] public string rewardText;

    public QuestStatus questStatus = QuestStatus.NotStarted;

    [NonSerialized]
    public Func<Player, bool> checkCondition;

    [NonSerialized]
    public Action<Player> Reward;

    public virtual bool CheckClearCondition()
    {
        return false;
    }

    public virtual void GiveClearReward()
    {

    }


}
