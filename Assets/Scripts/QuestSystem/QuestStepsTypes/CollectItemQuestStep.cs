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
        GameEventsManager.Instance.InventoryEvents.OnItemDropped += ItemDropped; 
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemAdded -= ItemCollected;
        GameEventsManager.Instance.InventoryEvents.OnItemDropped -= ItemDropped;
    }

    public override void Configure(QuestStepConfig config)
    {
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
            
            if (CheckCompletion())
            {
                FinishQuestStep();
            }
        }
    }

    private void ItemDropped(int id)
    {
        if (itemsCollectedMap.ContainsKey(id))
        {
            if (itemsCollectedMap[id] >= 0)
            {
                itemsCollectedMap[id]--;
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
        foreach (int Id in itemsCollectedMap.Keys)
        {
            int collectedAmount = itemsCollectedMap[Id];
            int amountToComplete = amountToCompleteMap[Id];

            if (collectedAmount >= amountToComplete)
            {
                collectedAmount = amountToComplete;
            }

            ItemDataSO itemDataSO = ItemCreator.GetItemById(Id);
            string collectionDescription = "Collect " + itemDataSO.DisplayName;
            string progressDescription = " (" + itemsCollectedMap + "/" + amountToComplete + ")\n";

            description += collectionDescription + progressDescription;
        }
    } 
}
