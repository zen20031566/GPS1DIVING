
public class InventoryUpgrade : GearUpgrade
{
    public int increaseRowAmount = 2;
    private int capacity;

    public override void Initialiize(GearUpgradeSO gearUpgradeData, Player player)
    {
        base.Initialiize(gearUpgradeData, player);
        capacity = Player.InventoryManager.InventoryGrid.TotalSlots;
        Description = "Capacity: " + capacity + " slots";
        UpgradedDescription = "Capacity: " + (capacity + (Player.InventoryManager.InventoryGrid.GridWidth * increaseRowAmount)) + " slots";
    }

    public override void ApplyUpgrade()
    {
        if (Level < GearUpgradeData.MaxLevel)
        {
            Level++;
            Cost += GearUpgradeData.CostIncrease;

            Player.InventoryManager.UpgradeInventory(increaseRowAmount);

            capacity = Player.InventoryManager.InventoryGrid.TotalSlots;
            Description = "Capacity: " + capacity + " slots";
            UpgradedDescription = "Capacity: " + (capacity + (Player.InventoryManager.InventoryGrid.GridWidth * increaseRowAmount)) + " slots";
        }
    }
}
