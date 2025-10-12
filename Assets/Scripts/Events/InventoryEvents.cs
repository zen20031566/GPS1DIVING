using System;

public class InventoryEvents
{
    public event Action<int> OnItemAdded;
    public event Action<int> OnItemRemoved;
    public event Action OnInventoryClosed;

    public void ItemAdded(int id)
    {
        OnItemAdded?.Invoke(id);
    }

    public void ItemRemoved(int id)
    {
        OnItemRemoved?.Invoke(id);
    }

    public void InventoryClose()
    {
        OnInventoryClosed?.Invoke();
    }
}
