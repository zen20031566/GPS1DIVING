
public class FlashlightUpgrade : GearUpgrade
{

    public override void ApplyUpgrade()
    {
        if (Level < GearUpgradeData.MaxLevel)
        {
            Level++;
            Price += GearUpgradeData.CostIncrease;


        }
    }
}
