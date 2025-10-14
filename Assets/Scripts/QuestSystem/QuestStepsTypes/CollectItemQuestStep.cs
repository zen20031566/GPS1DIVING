using System.Collections.Generic;
using UnityEngine;

public class CollectItemQuestStep : QuestStep
{
    private Dictionary<int, int> itemsCollectedMap;
    private Dictionary<int, int> amountToCompleteMap;

    private bool collectByItemTag = false;
    private Dictionary<ItemTag, int> itemsCollectedByTagMap;
    private Dictionary<ItemTag, int> amountToCompleteByTagMap;

    private void OnEnable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemAdded += ItemCollected;
        GameEventsManager.Instance.InventoryEvents.OnItemDropped += ItemRemoved; 
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemAdded -= ItemCollected;
        GameEventsManager.Instance.InventoryEvents.OnItemDropped -= ItemRemoved;
    }

    public override void Configure(QuestStepConfig config) //Add player reference to check inventory
    {
        collectByItemTag = config.collectByItemTag;

        if (!collectByItemTag)
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
        }
        else
        {
            itemsCollectedByTagMap = new Dictionary<ItemTag, int>();
            amountToCompleteByTagMap = new Dictionary<ItemTag, int>();

            foreach (var pair in config.itemsToCollect)
            {
                amountToCompleteByTagMap.Add(pair.ItemTag, pair.RequiredAmount);
            }

            foreach (var pair in config.itemsToCollect)
            {
                itemsCollectedByTagMap.Add(pair.ItemTag, 0);
            }
        }

            UpdateDescription();
    }

    private void ItemCollected(ItemData itemData)
    {
        if (!collectByItemTag)
        {
            HandleItemCollectedById(itemData);
        }
        else
        {
            HandleItemCollectedByTag(itemData);
        }
    }

    private void ItemRemoved(ItemData itemData)
    {
        if (!collectByItemTag)
        {
            HandleItemRemovedById(itemData);
        }
        else
        {
            HandleItemRemovedByTag(itemData);
        }
    }

    private void HandleItemCollectedById(ItemData itemData)
    {
        if (itemsCollectedMap.ContainsKey(itemData.ItemDataSO.Id))
        {
            itemsCollectedMap[itemData.ItemDataSO.Id]++;
            UpdateDescription();

            GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);

            if (CheckCompletion())
            {
                FinishQuestStep();
            }
        }
    }

    private void HandleItemCollectedByTag(ItemData itemData)
    {
        foreach (var tag in itemData.ItemDataSO.Tags)
        {
            if (itemsCollectedByTagMap.ContainsKey(tag))
            {
                itemsCollectedByTagMap[tag]++;
                UpdateDescription();

                GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);

                if (CheckCompletion())
                {
                    FinishQuestStep();
                }
            }
        }
    }

    private void HandleItemRemovedById(ItemData itemData)
    {
        if (itemsCollectedMap.ContainsKey(itemData.ItemDataSO.Id))
        {
            if (itemsCollectedMap[itemData.ItemDataSO.Id] > 0)
            {
                itemsCollectedMap[itemData.ItemDataSO.Id]--;
                UpdateDescription();

                GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);
            }
        }
    }

    private void HandleItemRemovedByTag(ItemData itemData)
    {
        foreach (var tag in itemData.ItemDataSO.Tags)
        {
            if (itemsCollectedByTagMap.ContainsKey(tag))
            {
                itemsCollectedByTagMap[tag]--;
                UpdateDescription();

                GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);
            }
        }
    }

    private bool CheckCompletion()
    {
        if (!collectByItemTag)
        {
            return CheckCompletionForId();
        }
        else
        {
            return CheckCompletionForTag();
        }
    }

    private bool CheckCompletionForId()
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

    private bool CheckCompletionForTag()
    {
        foreach (var kvp in itemsCollectedByTagMap)
        {
            var tag = kvp.Key;
            int collectedAmount = kvp.Value;

           var amountToComplete = amountToCompleteByTagMap[tag];

            if (collectedAmount < amountToComplete)
            {
                return false;
            }
        }
        return true;
    }

    private void UpdateDescription()
    {
        if (!collectByItemTag)
        {
            HandleUpdateDescriptionForId();
        }
        else
        {
            HandleUpdateDescriptionForTag();
        }
    } 

    private void HandleUpdateDescriptionForId()
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

    private void HandleUpdateDescriptionForTag()
    {
        description = "";
        foreach (var kvp in itemsCollectedByTagMap)
        {
            var tag = kvp.Key;
            int collectedAmount = kvp.Value;
            int amountToComplete = amountToCompleteByTagMap[tag];

            if (collectedAmount >= amountToComplete)
            {
                collectedAmount = amountToComplete;
            }

            string collectionDescription = "Collect " + tag.ToString() + " items";
            string progressDescription = " (" + collectedAmount + "/" + amountToComplete + ")\n";

            description += collectionDescription + progressDescription;
        }
    }
}
