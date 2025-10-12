
public class InventoryUpgrade : GearUpgrade
{

    public override void ApplyUpgrade(Player player)
    {
        if (Level < GearUpgradeData.MaxLevel)
        {
            Level++;
            Cost += GearUpgradeData.CostIncrease;

            player.InventoryManager.UpgradeInventory();
        }
    }
}
