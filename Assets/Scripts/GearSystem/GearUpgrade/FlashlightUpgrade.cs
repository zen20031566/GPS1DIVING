
public class FlashlightUpgrade : GearUpgrade
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
