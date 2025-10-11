using UnityEngine;

public class ShopManager : MonoBehaviour 
{
    private Player player;

    private void OnEnable()
    {
        GameEventsManager.Instance.ShopEvents.OnShopOpen += OpenShop;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.ShopEvents.OnShopOpen -= OpenShop;
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            SellAllTrash();
        }

    }
    private void OpenShop(Player player)
    {
        this.player = player;   
    }

    private void SellAllTrash()
    {
        InventoryManager playerInventory = player.inventoryManager;
        var playerInventoryItems = playerInventory.InventoryGrid.GridItems;

        foreach (InventoryItem item in playerInventoryItems)
        {
            if (item == null) continue;

            ItemData itemData = item.ItemData;
            if (itemData.HasItemTag(ItemTag.Trash) && !itemData.HasItemTag(ItemTag.NotSellable))
            {
                GameEventsManager.Instance.GoldEvents.AddGold(item.ItemDataSO.Value);
                playerInventory.RemoveItem(item, playerInventory.InventoryGrid);
            }
        }
    }

    private void TryBuyItem(ItemDataSO itemDataSO)
    {

    }

    private void TryBuyGearUpgrade(GearUpgrade gearUpgrade)
    {

    }


}
