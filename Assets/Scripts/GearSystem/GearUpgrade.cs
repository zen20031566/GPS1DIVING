using UnityEngine;

public abstract class GearUpgrade : MonoBehaviour
{
    public GearUpgradeSO GearUpgradeData;
    public int Level = 0;
    public int Cost;
    public string Description;

    public void Initialiize(GearUpgradeSO gearUpgradeData)
    {
        this.GearUpgradeData = gearUpgradeData; 
        this.Description = gearUpgradeData.Description;
        Cost = gearUpgradeData.BaseCost;
    }

    public abstract void ApplyUpgrade(Player player);
}
