using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string Text;
    public string SpeakerName;
    public Sprite SpeakerPortrait; // Optional character portrait
}

[System.Serializable]
public class DialogueData
{
    public string DialogueId;
    public DialogueLine[] Lines;

    [Header("Conditions")]
    public bool RequiresQuest;
    public string RequiredQuestId;
    public QuestState RequiredQuestState;

    [Header("Actions")]
    public bool StartsQuest;
    public string QuestToStartId;
    public bool CompletesQuest;
    public string QuestToCompleteId;
}
