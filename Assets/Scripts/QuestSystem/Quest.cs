using UnityEngine;

public class Quest
{
    public QuestInfoSO Info;

    public QuestState State;

    public QuestStep CurrentQuestStep = null;

    private int CurrentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.Info = questInfo;
        this.State = QuestState.REQUIREMENTS_NOT_MET;
        this.CurrentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        CurrentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (CurrentQuestStepIndex < Info.QuestStepConfigs.Length);

    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        QuestStepConfig config = GetCurrentQuestStepConfig();
        if (config != null)
        {
            // Use factory to create the appropriate quest step
            CurrentQuestStep = QuestStepFactory.CreateQuestStep(config, Info.Id, parentTransform);
            GameEventsManager.Instance.QuestStepEvents.QuestStepCreated(Info.Id);
        }
    }

    public QuestStepConfig GetCurrentQuestStepConfig()
    {
        if (CurrentStepExists())
        {
            return Info.QuestStepConfigs[CurrentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step but stepIndex was out of range indicating that there is no current step: QuestID=" + Info.Id);
        }
        return null;
    }
}

