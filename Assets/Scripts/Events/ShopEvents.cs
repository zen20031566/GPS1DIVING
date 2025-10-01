using System;

public class ShopEvents 
{
    public event Action OnShopOpen;
    public event Action OnShopClose;
    public event Action OnShopBuy;
    public event Action OnShopSell;
}
