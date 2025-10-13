using UnityEngine;

public class ShopManager : MonoBehaviour 
{
    public Player Player { get; private set; }
    

    private void OnEnable()
    {
        GameEventsManager.Instance.ShopEvents.OnShopOpen += OpenShop;  
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.ShopEvents.OnShopOpen -= OpenShop;
    }

    private void OpenShop(Player player)
    {
        this.Player = player;
    }

    public void SellAllSellables()
    {
        InventoryManager playerInventory = Player.InventoryManager;
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

    public void TryBuyItem(ItemDataSO itemDataSO)
    {
        if (Player.GoldManager.GoldAmount >= itemDataSO.Price)
        {
            Player.InventoryManager.AddItem(itemDataSO);
            GameEventsManager.Instance.GoldEvents.GoldDecrease(itemDataSO.Price);
        }
    }

    public void TryBuyGearUpgrade(GearUpgrade gearUpgrade, GearUpgradeTab gearUpgradeTab)
    {
        if (Player.GoldManager.GoldAmount >= gearUpgrade.Price)
        {
            gearUpgrade.ApplyUpgrade();
            gearUpgradeTab.UpdateTab(gearUpgrade);
            GameEventsManager.Instance.GoldEvents.GoldDecrease(gearUpgrade.Price);
            Debug.Log("Upgraded " + gearUpgrade.GearUpgradeData.DisplayName);
        }
    }
}
