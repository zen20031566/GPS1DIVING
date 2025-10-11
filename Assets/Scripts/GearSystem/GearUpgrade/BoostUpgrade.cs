
public class BoostUpgrade : GearUpgrade
{

    public override void ApplyUpgrade(Player player)
    {
        if (level <= gearUpgradeData.MaxLevel)
        {
            level++;
            cost = gearUpgradeData.BaseCost + (gearUpgradeData.CostIncrease * level);
        }
    }
}
