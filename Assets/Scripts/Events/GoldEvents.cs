using System;

public class GoldEvents 
{
    public event Action<int> OnGoldChange;
    public event Action<int> OnGoldAdd;
    public event Action<int> OnGoldDecrease;

    public void GoldChange(int goldAmount)
    {
        OnGoldChange?.Invoke(goldAmount);
    }

    public void AddGold(int amount)
    {
        OnGoldAdd?.Invoke(amount);
    }

    public void GoldDecrease(int amount)
    {
        OnGoldDecrease?.Invoke(amount);
    }
}


