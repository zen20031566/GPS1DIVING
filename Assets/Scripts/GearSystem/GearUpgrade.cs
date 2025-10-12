using UnityEngine;

public abstract class GearUpgrade : MonoBehaviour
{
    public GearUpgradeSO GearUpgradeData;
    public int Level = 0;
    public int Cost;
    public string Description;
    public string UpgradedDescription;
    public Player Player;

    public virtual void Initialiize(GearUpgradeSO gearUpgradeData, Player player)
    {
        this.GearUpgradeData = gearUpgradeData; 
        Cost = gearUpgradeData.BaseCost;
        this.Player = player;
    }

    public abstract void ApplyUpgrade();
}
