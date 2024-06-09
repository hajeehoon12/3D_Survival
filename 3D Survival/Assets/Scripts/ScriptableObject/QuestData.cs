using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]


[CreateAssetMenu(fileName = "Quest" , menuName = "New Quest")]
public class QuestData : ScriptableObject
{
    [Header("Info")]
    public string title;
    [TextArea(1,10)] public string descriptionText;
    [TextArea(1,10)] public string conditionText;
    [TextArea(1,10)] public string rewardText;
    public bool isCleared;

    [NonSerialized]
    public Func<Player, bool> checkCondition;

    [NonSerialized]
    public Action<Player> Reward;

}
