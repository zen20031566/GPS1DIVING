using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuestStep : QuestStep
{
    private Dictionary<int, int> itemsCollectedMap;
    private Dictionary<int, int> amountToCompleteMap;

    private bool collectByItemTag = false;

    private void OnEnable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemAdded += ItemCollected;
        GameEventsManager.Instance.InventoryEvents.OnItemRemoved += ItemRemoved; 
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemAdded -= ItemCollected;
        GameEventsManager.Instance.InventoryEvents.OnItemRemoved -= ItemRemoved;
    }

    public override void Configure(QuestStepConfig config) //Add player reference to check inventory
    {
        itemsCollectedMap = new Dictionary<int, int>();
        amountToCompleteMap = new Dictionary<int, int>();

        foreach (var pair in config.itemsToCollect)
        {
            amountToCompleteMap.Add(pair.Id, pair.RequiredAmount);
        }

        foreach (var pair in config.itemsToCollect)
        {
            itemsCollectedMap.Add(pair.Id, 0);
        }

        UpdateDescription();
    }

    private void ItemCollected(int id)
    {
        if (itemsCollectedMap.ContainsKey(id))
        {
            itemsCollectedMap[id]++;
            UpdateDescription();

            GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);

            if (CheckCompletion())
            {
                FinishQuestStep();
            }
        }
    }

    private void ItemRemoved(int id)
    {
        if (itemsCollectedMap.ContainsKey(id))
        {
            if (itemsCollectedMap[id] > 0)
            {
                itemsCollectedMap[id]--;
                UpdateDescription();

                GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);
            }
        }
    }

    private bool CheckCompletion()
    {
        foreach (var kvp in itemsCollectedMap)
        {
            int id = kvp.Key;
            int collectedAmount = kvp.Value;

            int amountToComplete = amountToCompleteMap[id];

            if (collectedAmount < amountToComplete)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateDescription()
    {
        description = "";
        foreach (var kvp in itemsCollectedMap)
        {
            int id = kvp.Key;
            int collectedAmount = kvp.Value;
            int amountToComplete = amountToCompleteMap[id];

            if (collectedAmount >= amountToComplete)
            {
                collectedAmount = amountToComplete;
            }

            ItemDataSO itemDataSO = ItemCreator.GetItemById(id);
            string collectionDescription = "Collect " + itemDataSO.DisplayName;
            string progressDescription = " (" + collectedAmount + "/" + amountToComplete + ")\n";

            description += collectionDescription + progressDescription;
        }
    } 
}
