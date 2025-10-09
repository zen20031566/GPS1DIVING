using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GearManager equipmentManager;

    [SerializeField] private Transform equipmentTab;
    [SerializeField] private EquipmentTabSlot equipmentTabSlotPrefab;

    [SerializeField] private Transform upgradeTab;
    [SerializeField] private UpgradeTabSlot upgradeTabSlotPrefab;

    private void OnEnable()
    {
        FillEquipmentTab();
    }

    private void OnDisable()
    {
        ClearEquipmentTab();
    }

    //Equipment Tab
    private void FillEquipmentTab()
    {
        foreach (GearSO equipment in equipmentManager.PlayerEquipment)
        {
            InstantiateEquipmentSlot(equipment);
        }
    }

    private void ClearEquipmentTab()
    {
        foreach (Transform equipmentSlot in equipmentTab)
        {
            Destroy(equipmentSlot.gameObject);
        }
    }

    private void InstantiateEquipmentSlot(GearSO equipmentData)
    {
        EquipmentTabSlot equipmentTabSlot = Instantiate(equipmentTabSlotPrefab, equipmentTab);
        equipmentTabSlot.InitializeEquipmentSlot(equipmentData);
        Debug.Log("SPawn");
    }

    //Upgrades Tab
    private void FillUpgradeTab(GearSO equipmentData)
    {
        foreach (GearUpgrades upgrade in equipmentData.EquipmentUpgrades)
        {
            InstantiateUpgradeSlot(upgrade);
        }
    }

    private void ClearUpgradeTab()
    {
        foreach (Transform upgradeSlot in upgradeTab)
        {
            Destroy(upgradeSlot.gameObject);
        }
    }

    private void InstantiateUpgradeSlot(GearUpgrades upgradeData)
    {
        UpgradeTabSlot upgradeTabSlot;
        upgradeTabSlot = Instantiate(upgradeTabSlotPrefab, upgradeTab);
        //upgradeTabSlot 
    }

   

}
