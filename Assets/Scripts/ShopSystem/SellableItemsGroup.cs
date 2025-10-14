using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SellableItemsGroup : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] SellItemInfoUI sellItemPrefab;
    private Player player;
    private int totalSaleValue = 0;
    [SerializeField] private TMP_Text totalSaleValueText;

    private Dictionary<ItemDataSO, int> sellableQuantityMap = new Dictionary<ItemDataSO, int>();

    private void OnEnable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemDropped += UpdateSellItems;
        UpdateSellItems();
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.InventoryEvents.OnItemDropped -= UpdateSellItems;
    }

    public void InitializeGroup(ShopManager shopManager)
    {
        this.shopManager = shopManager;
        player = shopManager.Player;

        UpdateSellItems();
    }

    private void UpdateSellItems(ItemData itemData = null) //for future use maybe if only want to sell a specific item
    {
        sellableQuantityMap.Clear();
        totalSaleValue = 0;
        ResetSellItems();

        if (player != null)
        {
            GetQuantityOfSellables();
            
            foreach (var kvp in sellableQuantityMap)
            {
                ItemDataSO itemDataSO = kvp.Key;
                int quantity = kvp.Value;

                InstiantiateSellItem(itemDataSO, quantity);

                totalSaleValue += (itemDataSO.Value * quantity);
            }
        }
        totalSaleValueText.text = $"{totalSaleValue}";
    }

    public void GetQuantityOfSellables()
    {
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
