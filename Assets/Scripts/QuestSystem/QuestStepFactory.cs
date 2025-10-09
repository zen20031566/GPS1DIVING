using UnityEngine;

public static class QuestStepFactory 
{
    public static QuestStep CreateQuestStep(QuestStepConfig config, string questId, Transform parent)
    {
        GameObject questStepObject = new GameObject($"{config.stepType}_{questId}");
        questStepObject.transform.SetParent(parent);

        QuestStep questStep = config.stepType switch
        {
            QuestStepType.COLLECT_ITEM => questStepObject.AddComponent<CollectItemQuestStep>(),
            QuestStepType.GO_TO_LOCATION => questStepObject.AddComponent<GoToLocationQuestStep>(),

            //default
            _ => questStepObject.AddComponent<QuestStep>()
        };

        questStep.InitializeQuestStep(questId);

        // Configure the specific step type
        ConfigureQuestStep(questStep, config);

        return questStep;
    }

    private static void ConfigureQuestStep(QuestStep questStep, QuestStepConfig config)
    {
        questStep.Configure(config);

        switch (questStep)
        {
            case CollectItemQuestStep collectionStep:
                collectionStep.Configure(config);
                break;
            case GoToLocationQuestStep locationStep:
                locationStep.Configure(config);
                break;
        }
    }
}

