using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private GearUpgradeInfoUI gearUpgradeInfoPrefab;
    [SerializeField] Transform gearInfoGroup;
    private Player player;
    private void Awake()
    {
        player = inventoryManager.Player; 
    }

    private void UpdateGearInfo()
    {
        GearManager gearManager = player.GearManager;
        foreach (GearUpgrade gearUpgrade in gearManager.GearUpgradesList)
        {
            GearUpgradeInfoUI gearUpgradeInfoUI = Instantiate(gearUpgradeInfoPrefab, gearInfoGroup);
            gearUpgradeInfoUI.UpdateGearInfo(gearUpgrade);
        }
    }

    private void ResetGearInfo()
    {
        foreach (Transform child in gearInfoGroup)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnEnable()
    {
        UpdateGearInfo();
    }

    private void OnDisable()
    {
        ResetGearInfo();
        inventoryManager.ReturnSelectedItemBack();
        inventoryManager.CurrentItemGrid = null;
    }
}
