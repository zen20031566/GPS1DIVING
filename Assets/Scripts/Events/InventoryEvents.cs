using System;

public class InventoryEvents
{
    public event Action<int> OnItemAdded;
    public event Action<int> OnItemDropped;

    public void ItemAdded(int id)
    {
        OnItemAdded?.Invoke(id);
    }

    public void ItemDropped(int id)
    {
        OnItemDropped?.Invoke(id);
    }
}
