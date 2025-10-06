using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class QuestPoint : MonoBehaviour, IInteractable
{
    [SerializeField] private QuestInfoSO questInfoForPoint;
    [SerializeField] private QuestIcon questIcon;
    public bool CanInteract { get; set; } = true;

    private string questId;

    private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.Id;
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
    }

    public void Interact(Player player)
    {
        if (currentQuestState.Equals(QuestState.CAN_START))
        {
            GameEventsManager.Instance.QuestEvents.StartQuest(questId);
        }
        else if (currentQuestState.Equals(QuestState.CAN_FINISH))
        {
            GameEventsManager.Instance.QuestEvents.FinishQuest(questId);
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.Info.Id.Equals(questId))
        {
            currentQuestState = quest.State;
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);

            questIcon.SetState(currentQuestState);
        }
    }
}
