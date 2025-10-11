using UnityEngine;

public class GearUpgradeGroup : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private GearUpgradeTab tabPrefab;
    private Player player;

    private void Initialize(Player player)
    {
        this.player = player;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    private void InstiantiateUpgradeTab(GearUpgrade gearUpgrade)
    {
        GearUpgradeTab gearUpgradeTab = Instantiate(tabPrefab, gameObject.transform);
        gearUpgradeTab.Initialize(gearUpgrade, shopManager);
    }
}
