using UnityEngine;
using System.Collections.Generic;

public class GearManager : MonoBehaviour
{
    [SerializeField] private List<GearUpgradeSO> gearUpgradesSOList = new List<GearUpgradeSO>();

    private List<GearUpgrade> gearUpgradesList = new List<GearUpgrade>();

    private void InitializeGear()
    {
        foreach (GearUpgradeSO gearUpgradeData in gearUpgradesSOList)
        {
            GearUpgrade gearUpgrade = Instantiate(gearUpgradeData.Prefab, transform);
            gearUpgrade.Initialiize(gearUpgradeData);
            gearUpgradesList.Add(gearUpgrade);
        }
    }

}
