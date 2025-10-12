
public class OxygenUpgrade : GearUpgrade
{
    private float increaseOxygenAmount = 300;

    public override void ApplyUpgrade()
    {
        if (Level < GearUpgradeData.MaxLevel)
        {
            Level++;
            Cost += GearUpgradeData.CostIncrease;

            Player.PlayerOxygen.UpgradeOxygen(increaseOxygenAmount);    
        }
    }
}
