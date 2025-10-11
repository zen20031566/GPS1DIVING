using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;

    private void Awake()
    {
        questMap = CreateQuestMap();
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuest += StartQuest;
        GameEventsManager.Instance.QuestEvents.OnAdvanceQuest += AdvanceQuest;
        GameEventsManager.Instance.QuestEvents.OnFinishQuest += FinishQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.QuestEvents.OnStartQuest -= StartQuest;
        GameEventsManager.Instance.QuestEvents.OnAdvanceQuest -= AdvanceQuest;
        GameEventsManager.Instance.QuestEvents.OnFinishQuest -= FinishQuest;
    }

    private void Start()
    {
        //Broadcast initial state of all quest on start
        foreach (Quest quest in questMap.Values)
        {
            GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
        }
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.State == QuestState.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.Info.Id, QuestState.CAN_START);
            }
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.State = state;
        GameEventsManager.Instance.QuestEvents.QuestStateChange(quest);
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        //Check if prerequisite quests are done
        foreach (QuestInfoSO prerequisiteQuestInfo in quest.Info.QuestPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.Id).State != QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }

        return meetsRequirements;
    }

    private void StartQuest(string id)
    {
        Debug.Log("Start Quest: " + id);

        Quest quest = GetQuestById(id);

        quest.InstantiateCurrentQuestStep(this.transform);
        ChangeQuestState(quest.Info.Id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Debug.Log("Advance Quest: " + id);

        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();

        //If there are more quest steps
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(this.transform);
        }
        //No more steps
        else
        {
            ChangeQuestState(quest.Info.Id, QuestState.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Debug.Log("Finish Quest: " + id);

        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.Info.Id, QuestState.FINISHED);

    }

    private void ClaimRewards(Quest quest)
    {
        GameEventsManager.Instance.GoldEvents.AddGold(quest.Info.GoldReward);
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuest = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();

        foreach (QuestInfoSO questInfo in allQuest)
        {
            if (idToQuestMap.ContainsKey(questInfo.Id))
            {
                Debug.LogWarning("Duplicate id found when creating quest map: " + questInfo.Id);
                continue; //Skip this quest to avoid duplicates
            }

            idToQuestMap.Add(questInfo.Id, new Quest(questInfo));
        }

        return idToQuestMap;
    }

    public Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        
        if(quest == null)
        {
            Debug.LogError("id not found in the quest map: " + id);
        }

        return quest;
    }

}
    