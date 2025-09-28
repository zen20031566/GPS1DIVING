using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    protected string questId;
    protected string description;

    //Properties
    public string Description => description;

    //Properties
    public string QuestId => questId;

    //Configure atributes of queststep if any
    public virtual void Configure(QuestStepConfig config)
    {
        description = config.description;
    }

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
