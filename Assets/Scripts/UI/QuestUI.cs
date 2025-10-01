using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] QuestManager questManager;
    [SerializeField] private TMP_Text questDisplayNameText;
    [SerializeField] private TMP_Text questStepText;

    private void OnEnable()
    {
        //GameEventsManager.Instance.QuestEvents.OnStartQuest += StartQuest;
        //GameEventsManager.Instance.QuestEvents.OnAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.QuestEvents.OnFinishQuest += FinishQuest;
        GameEventsManager.Instance.QuestEvents.OnQuestStateChange += QuestStateChange;
        GameEventsManager.Instance.QuestStepEvents.OnQuestStepCreated += QuestStepCreated;
        GameEventsManager.Instance.QuestStepEvents.OnQuestStepProgressChanged += QuestStepProgressChanged;

    }

    private void OnDisable()
    {
        //GameEventsManager.Instance.QuestEvents.OnStartQuest -= StartQuest;
        //GameEventsManager.Instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;
        GameEventsManager.Instance.QuestEvents.OnQuestStateChange -= QuestStateChange;
        GameEventsManager.Instance.QuestStepEvents.OnQuestStepCreated -= QuestStepCreated;
        GameEventsManager.Instance.QuestStepEvents.OnQuestStepProgressChanged -= QuestStepProgressChanged;
    }

    private void Start()
    {
        questDisplayNameText.text = "";
        questStepText.text = "";
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.State.Equals(QuestState.IN_PROGRESS))
        {
            UpdateQuestDisplay(quest.Info.Id);
        }
        else if (quest.State.Equals(QuestState.CAN_FINISH))
        {
            questStepText.text = "Quest complete turn in quest";
        }
    }

    //private void StartQuest(string questId)
    //{
    //    UpdateQuestDisplay(questId);
    //}

    //private void AdvanceQuest(string questId)
    //{
    //    UpdateQuestDisplay(questId);
    //}

    private void QuestStepCreated(string questId)
    {
        UpdateQuestDisplay(questId);
    }

    private void FinishQuest(string questId)
    {
        questDisplayNameText.text = "";
        questStepText.text = "";
    }

    private void QuestStepProgressChanged(string questId)
    {
        UpdateQuestDisplay(questId);
    }

    private void UpdateQuestDisplay(string questId)
    {
        Quest quest = questManager.GetQuestById(questId);
        questDisplayNameText.text = quest.Info.DisplayName;

        QuestStep questStep = quest.CurrentQuestStep;
        if (questStep != null)
        {
            questStepText.text = questStep.Description;
        }
    }
}
