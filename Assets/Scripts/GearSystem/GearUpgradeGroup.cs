using UnityEngine;

public class GearUpgradeGroup : MonoBehaviour
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private GearUpgradeTab tabPrefab;
    private Player player;

    private void OnEnable()
    {
        player = shopManager.Player;

        if (player != null)
        {
            ResetTabs();

            foreach (GearUpgrade gearUpgrade in player.GearManager.GearUpgradesList)
            {
                InstiantiateUpgradeTab(gearUpgrade);
            }
        }  
    }

    private void InstiantiateUpgradeTab(GearUpgrade gearUpgrade)
    {
        GearUpgradeTab gearUpgradeTab = Instantiate(tabPrefab, gameObject.transform);
        gearUpgradeTab.Initialize(gearUpgrade, shopManager);
    }
    
    private void ResetTabs() 
    {
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
