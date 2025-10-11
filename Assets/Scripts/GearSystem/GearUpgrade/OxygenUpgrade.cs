
public class OxygenUpgrade : GearUpgrade
{
    private float increaseOxygenAmount = 300;

    public override void ApplyUpgrade(Player player)
    {
        if (Level <= GearUpgradeData.MaxLevel)
        {
            Level++;
            Cost = GearUpgradeData.BaseCost + (GearUpgradeData.CostIncrease * Level);

            player.PlayerOxygen.UpgradeOxygen(increaseOxygenAmount);    
        }
    }
}
