using UnityEngine;

public class Quest
{
    public QuestInfoSO info;

    public QuestState state;

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
        //GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        //if(questStepPrefab != null)
        //{
        //    QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();

        //    //Tells the queststep what quest it belongs to
        //    questStep.InitializeQuestStep(info.id);
        //}

        QuestStepConfig config = GetCurrentQuestStepConfig();
        if (config != null)
        {
            // Use factory to create the appropriate quest step
            QuestStep questStep = QuestStepFactory.CreateQuestStep(config, info.id, parentTransform);
        }
    }

    private QuestStepConfig GetCurrentQuestStepConfig()
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

    //private GameObject GetCurrentQuestStepPrefab()
    //{
    //    GameObject questStepPrefab = null;
    //    if (CurrentStepExists())
    //    {
    //        questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Tried to get quest step prefab but stepIndex was out of range indicating that there is no current step: QuestID=" + info.id);
    //    }
    //    return questStepPrefab;
    //}
}

