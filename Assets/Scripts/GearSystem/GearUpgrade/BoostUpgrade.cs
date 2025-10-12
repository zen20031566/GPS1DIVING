
public class BoostUpgrade : GearUpgrade
{

    public override void ApplyUpgrade()
    {
        if (Level < GearUpgradeData.MaxLevel)
        {
            Level++;
            Cost += GearUpgradeData.CostIncrease;
        }
    }
}
