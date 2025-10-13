
public class OxygenUpgrade : GearUpgrade
{
    private float increaseOxygenAmount = 300;
    private float maxOxygenTime;

    public override void Initialiize(GearUpgradeSO gearUpgradeData, Player player)
    {
        base.Initialiize(gearUpgradeData, player);
        this.maxOxygenTime = Player.PlayerOxygen.MaxOxygenTime;
        Description = "Oxygen: " + maxOxygenTime + "s";
        UpgradedDescription = "Oxygen: " + (maxOxygenTime + increaseOxygenAmount) + "s";
    }


    public override void ApplyUpgrade()
    {
        if (Level < GearUpgradeData.MaxLevel)
        {
            Level++;
            Price += GearUpgradeData.CostIncrease;

            Player.PlayerOxygen.UpgradeOxygen(increaseOxygenAmount);

            this.maxOxygenTime = Player.PlayerOxygen.MaxOxygenTime;
            Description = "Oxygen: " + maxOxygenTime + "s";
            UpgradedDescription = "Oxygen: " + (maxOxygenTime + increaseOxygenAmount) + "s";
        }
    }
}
