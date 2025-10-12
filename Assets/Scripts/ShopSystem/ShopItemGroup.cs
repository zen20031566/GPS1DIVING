using System.Collections.Generic;
using UnityEngine;

public class ShopItemGroup : MonoBehaviour
{
    private Player player;
    private ShopManager shopManager;
    [SerializeField] private ShopItem shopItemPrefab;

    [SerializeField] private List<ItemDataSO> shopItems = new List<ItemDataSO>();

    public void InitializeGroup(ShopManager shopManager)
    {
        this.shopManager = shopManager;
        this.player = shopManager.Player;
        InstiantiateShopItems();
    }

    private void InstiantiateShopItems()
    {
        foreach (ItemDataSO itemDataSO in shopItems)
        {
            ShopItem shopItem = Instantiate(shopItemPrefab, gameObject.transform);
            shopItem.Initialize(itemDataSO, shopManager);
        }
    }
}
