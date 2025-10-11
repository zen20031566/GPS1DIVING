using System;

public class ShopEvents 
{
    public event Action<Player> OnShopOpen;
    public event Action OnShopBuy;
    public event Action OnShopSell;

    public void ShopOpen(Player player)
    {
        OnShopOpen?.Invoke(player); 
    }
}
