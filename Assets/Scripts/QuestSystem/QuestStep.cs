using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    private string questId;

    //Properties
    public string QuestId => questId;

    //Configure atributes of queststep if any
    public abstract void Configure(QuestStepConfig config);

    //Get what quest this queststep belongs to
    public void InitializeQuestStep(string questId)
    {
        this.questId = questId;
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            GameEventsManager.Instance.QuestEvents.AdvanceQuest(questId);
            Destroy(gameObject);
        }
    }
}
