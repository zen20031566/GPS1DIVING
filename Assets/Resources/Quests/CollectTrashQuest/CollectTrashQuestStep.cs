using UnityEngine;
using static UnityEngine.Rendering.STP;

public class CollectTrashQuestStep : QuestStep
{
    private int trashCollected = 0;
    private int amountToComplete = 5;

    private string baseDescription;
    private string progressDescription;

    private void OnEnable()
    {
        GameEventsManager.Instance.TrashEvents.OnTrashCollected += TrashCollected;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.TrashEvents.OnTrashCollected -= TrashCollected;
    }

    public override void Configure(QuestStepConfig config)
    {
        amountToComplete = config.amountToComplete;
        progressDescription = " (" + trashCollected + "/" + amountToComplete + ")";

        if (config.description != string.Empty)
        {
            baseDescription = config.description;
        }
        else
        {
            baseDescription = "Collect trash ";
        }

        description = baseDescription + progressDescription;
    }

    private void TrashCollected(TrashSO trashData)
    {
        if (trashCollected < amountToComplete)
        {
            trashCollected++;
            progressDescription = " (" + trashCollected + "/" + amountToComplete + ")";
            description = baseDescription + progressDescription;
            GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);
        }

        if (trashCollected >= amountToComplete)
        {
            FinishQuestStep();
        }
    }
}
