using UnityEngine;

public abstract class GearUpgrade : MonoBehaviour
{
    protected GearUpgradeSO gearUpgradeData;
    protected int level = 0;
    protected int cost;
    protected string description;

    //Properties
    public GearUpgradeSO GearUpgradeData => gearUpgradeData;

    public void Initialiize(GearUpgradeSO gearUpgradeData)
    {
        this.gearUpgradeData = gearUpgradeData; 
        this.description = gearUpgradeData.Description;
        cost = gearUpgradeData.BaseCost;
    }

    public abstract void ApplyUpgrade(Player player);
}
