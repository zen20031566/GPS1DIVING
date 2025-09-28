using UnityEngine;

public class Quest
{
    public QuestInfoSO info;

    public QuestState state;

    public QuestStep currentQuestStep = null;

    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepConfigs.Length);

    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        QuestStepConfig config = GetCurrentQuestStepConfig();
        if (config != null)
        {
            // Use factory to create the appropriate quest step
            currentQuestStep = QuestStepFactory.CreateQuestStep(config, info.id, parentTransform);
            GameEventsManager.Instance.QuestStepEvents.QuestStepCreated(info.id);
        }
    }

    public QuestStepConfig GetCurrentQuestStepConfig()
    {
        if (CurrentStepExists())
        {
            return info.questStepConfigs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step but stepIndex was out of range indicating that there is no current step: QuestID=" + info.id);
        }
        return null;
    }
}

