using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "New Dialogue")]
public class DialogueData : ScriptableObject
{
    [TextArea(1, 10)] public string[] sentences;
    [TextArea(1, 10)] public string[] sentenceAccepted;
    [TextArea(1, 10)] public string[] sentenceCleared;

}
