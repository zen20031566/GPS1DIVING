using UnityEngine;

public abstract class GearUpgrade : MonoBehaviour
{
    public GearUpgradeSO GearUpgradeData;
    public int Level = 0;
    public int Price;
    public string Description;
    public string UpgradedDescription;
    public Player Player;

    public virtual void Initialiize(GearUpgradeSO gearUpgradeData, Player player)
    {
        this.GearUpgradeData = gearUpgradeData; 
        Price = gearUpgradeData.BaseCost;
        this.Player = player;
    }

    public abstract void ApplyUpgrade();
}
