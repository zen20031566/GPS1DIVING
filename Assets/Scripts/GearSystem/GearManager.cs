using UnityEngine;
using System.Collections.Generic;

public class GearManager : MonoBehaviour
{
    [SerializeField] private List<GearUpgradeSO> gearUpgradesSOList = new List<GearUpgradeSO>();

    public List<GearUpgrade> GearUpgradesList = new List<GearUpgrade>();

    private void Start()
    {
        InitializeGear();
    }

    private void InitializeGear()
    {
        foreach (GearUpgradeSO gearUpgradeData in gearUpgradesSOList)
        {
            GearUpgrade gearUpgrade = Instantiate(gearUpgradeData.Prefab, transform);
            gearUpgrade.Initialiize(gearUpgradeData);
            GearUpgradesList.Add(gearUpgrade);
        }
    }

}
