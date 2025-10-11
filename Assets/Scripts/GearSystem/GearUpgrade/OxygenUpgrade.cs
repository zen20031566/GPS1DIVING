
public class OxygenUpgrade : GearUpgrade
{
    private float increaseOxygenAmount = 300;

    public override void ApplyUpgrade(Player player)
    {
        if (level <= gearUpgradeData.MaxLevel)
        {
            level++;
            cost = gearUpgradeData.BaseCost + (gearUpgradeData.CostIncrease * level);

            player.PlayerOxygen.UpgradeOxygen(increaseOxygenAmount);    
        }
    }
}
