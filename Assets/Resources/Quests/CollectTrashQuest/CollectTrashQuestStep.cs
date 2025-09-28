using UnityEngine;

public class CollectTrashQuestStep : QuestStep
{
    private int trashCollected = 0;
    private int amountToComplete = 5;

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
    }

    private void TrashCollected(TrashSO trashData)
    {
        if (trashCollected < amountToComplete)
        {
            trashCollected++;
        }

        if (trashCollected >= amountToComplete)
        {
            FinishQuestStep();
        }
    }
}
