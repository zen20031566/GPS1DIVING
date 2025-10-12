using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;
    [SerializeField] private GearUpgradeGroup gearUpgradeGroup;
    [SerializeField] private SellableItemsGroup sellableItemsGroup;
    [SerializeField] private ShopItemGroup shopItemGroup;

    private void OnEnable()
    {
        gearUpgradeGroup.InitializeGroup(shopManager);
        sellableItemsGroup.InitializeGroup(shopManager);
        
    }

    private void OnDisable()
    {
        
    }

    private void Start()
    {
        shopItemGroup.InitializeGroup(shopManager);
    }
}
