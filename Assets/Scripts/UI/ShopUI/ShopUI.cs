using UnityEngine;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private EquipmentManager equipmentManager;

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
        foreach (EquipmentSO equipment in equipmentManager.PlayerEquipment)
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

    private void InstantiateEquipmentSlot(EquipmentSO equipmentData)
    {
        EquipmentTabSlot equipmentTabSlot = Instantiate(equipmentTabSlotPrefab, equipmentTab);
        equipmentTabSlot.InitializeEquipmentSlot(equipmentData);
        Debug.Log("SPawn");
    }

    //Upgrades Tab
    private void FillUpgradeTab(EquipmentSO equipmentData)
    {
        foreach (EquipmentUpgradeSO upgrade in equipmentData.EquipmentUpgrades)
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

    private void InstantiateUpgradeSlot(EquipmentUpgradeSO upgradeData)
    {
        UpgradeTabSlot upgradeTabSlot;
        upgradeTabSlot = Instantiate(upgradeTabSlotPrefab, upgradeTab);
        //upgradeTabSlot 
    }

   

}
