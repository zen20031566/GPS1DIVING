using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class QuestPoint : MonoBehaviour
{
    [SerializeField] private QuestInfoSO questInfoForPoint;

    private bool playerIsNear = false;

    private string questId;

    private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }


    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
    }

    private void Update()
    {
        if (playerIsNear)
        {
            if (InputManager.InteractPressed)
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
        }
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log("Quest with id: " + questId + " updated to state: " + currentQuestState);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }

   
}
