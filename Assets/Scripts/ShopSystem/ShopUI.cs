using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;
    [SerializeField] private GearUpgradeGroup gearUpgradeGroup;
    [SerializeField] private SellableItemsGroup sellableItemsGroup;

    private void OnEnable()
    {
        gearUpgradeGroup.InitializeGroup(shopManager);
        sellableItemsGroup.InitializeGroup(shopManager);
    }

    private void OnDisable()
    {
        
    }
}
