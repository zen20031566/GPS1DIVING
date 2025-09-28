using System;

public class QuestStepEvents
{
    public event Action<string> OnQuestStepCreated;
    public event Action<string> OnQuestStepProgressChanged;

    public void QuestStepCreated(string questId)
    {
        OnQuestStepCreated?.Invoke(questId);
    }

    public void QuestStepProgressChanged(string questId)
    {
        OnQuestStepProgressChanged?.Invoke(questId);
    }

}
