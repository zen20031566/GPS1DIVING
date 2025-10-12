using System.Collections.Generic;
using UnityEngine;

public class SellableItemsGroup : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] SellItemInfoUI sellItemPrefab;
    private Player player;

    private Dictionary<ItemDataSO, int> sellableQuantityMap = new Dictionary<ItemDataSO, int>();

    private void OnEnable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemRemoved += UpdateSellItems;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemRemoved -= UpdateSellItems;
    }

    public void InitializeGroup(ShopManager shopManager)
    {
        this.shopManager = shopManager;
        player = shopManager.Player;

        if (player != null)
        {
            GetQuantityOfSellables();

            foreach (var kvp in sellableQuantityMap)
            {
                ItemDataSO itemDataSO = kvp.Key;
                int quantity = kvp.Value;

                InstiantiateSellItem(itemDataSO, quantity);
            }
        }
    }

    private void UpdateSellItems(int id) //for future use maybe if only want to sell a specific item
    {
        ResetSellItems();

        if (player != null)
        {
            GetQuantityOfSellables();

            foreach (var kvp in sellableQuantityMap)
            {
                ItemDataSO itemDataSO = kvp.Key;
                int quantity = kvp.Value;

                InstiantiateSellItem(itemDataSO, quantity);
            }
        }
    }

    public void GetQuantityOfSellables()
    {
        sellableQuantityMap.Clear();

        InventoryManager playerInventory = player.InventoryManager;
        var playerInventoryItems = playerInventory.InventoryGrid.GridItems;

        foreach (InventoryItem item in playerInventoryItems)
        {
            if (item == null) continue;

            ItemData itemData = item.ItemData;
            if (itemData.HasItemTag(ItemTag.Trash) && !itemData.HasItemTag(ItemTag.NotSellable))
            {
                ItemDataSO itemDataSO = itemData.ItemDataSO;

                if (sellableQuantityMap.ContainsKey(itemDataSO))
                {
                    sellableQuantityMap[itemDataSO]++;
                }
                else
                {
                    sellableQuantityMap.Add(itemDataSO, 1);
                }
            }
        }
    }

    private void InstiantiateSellItem(ItemDataSO itemDataSO, int quantity)
    {
        SellItemInfoUI sellItem = Instantiate(sellItemPrefab, gameObject.transform);
        sellItem.Initialize(itemDataSO, quantity);
    }

    public void ResetSellItems()
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
