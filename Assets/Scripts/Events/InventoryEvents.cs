using System;

public class InventoryEvents
{
    public event Action<Item> OnTryAddItem;
    public event Action<Item> OnItemAdded;
    public event Action<Item> OnItemDropped;

    public void TryAddItem(Item item)
    {
        OnTryAddItem?.Invoke(item);
    }

    public void ItemAdded(Item item)
    {
        OnItemAdded?.Invoke(item);
    }

    public void ItemDropped(Item item)
    {
        OnItemDropped?.Invoke(item);
    }
}
