using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CircleCollider2D))]

public class QuestPoint : MonoBehaviour, IInteractable
{
    private QuestInfoSO questInfoForPoint;
    [SerializeField] private QuestIcon questIcon;
    [SerializeField] private List<QuestInfoSO> allQuestsForPoint = new();

    public bool CanInteract { get; set; } = true;
    private string questId;
    private QuestState currentQuestState;
    private int questIndex = 0;

    private void Awake()
    {
        if (allQuestsForPoint.Count != 0)
        {
            questId = allQuestsForPoint[questIndex].Id;
        }
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

    private void CheckProceedToNextQuest()
    {
        if (currentQuestState.Equals(QuestState.FINISHED))
        {
            questIndex++;
            
            if (questIndex < allQuestsForPoint.Count)
            {
                questId = allQuestsForPoint[questIndex].Id;
            }
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.Info.Id.Equals(questId))
        {
            currentQuestState = quest.State;
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);

            questIcon.SetState(currentQuestState);

            CheckProceedToNextQuest();
        }
    }
}
