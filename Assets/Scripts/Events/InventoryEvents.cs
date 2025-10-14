using System;

public class InventoryEvents
{
    public event Action<ItemData> OnItemAdded;
    public event Action<ItemData> OnItemDropped;
    public event Action OnInventoryClosed;

    public void ItemAdded(ItemData itemData)
    {
        OnItemAdded?.Invoke(itemData);
    }

    public void ItemDropped(ItemData itemData)
    {
        OnItemDropped?.Invoke(itemData);
    }

    public void InventoryClose()
    {
        OnInventoryClosed?.Invoke();
    }
}
