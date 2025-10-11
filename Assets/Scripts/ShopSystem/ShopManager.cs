using UnityEngine;

public class ShopManager : MonoBehaviour 
{
    public Player Player;

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
        Player = GameManager.Instance.Player;
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
        this.Player = player;   
    }

    private void SellAllTrash()
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

    }

    public void TryBuyGearUpgrade(GearUpgrade gearUpgrade, GearUpgradeTab gearUpgradeTab)
    {
        Debug.Log("Try buy upgrade " + gearUpgrade.GearUpgradeData.DisplayName);
        gearUpgrade.ApplyUpgrade(Player);
        gearUpgradeTab.UpdateTab(gearUpgrade);
        if (Player.GoldManager.GoldAmount >= gearUpgrade.Cost)
        {
            
        }
    }


}
