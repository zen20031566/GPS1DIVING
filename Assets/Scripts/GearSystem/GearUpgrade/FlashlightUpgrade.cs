
public class FlashlightUpgrade : GearUpgrade
{

    public override void ApplyUpgrade(Player player)
    {
        if (Level <= GearUpgradeData.MaxLevel)
        {
            Level++;
            Cost = GearUpgradeData.BaseCost + (GearUpgradeData.CostIncrease * Level);

          
        }
    }
}
