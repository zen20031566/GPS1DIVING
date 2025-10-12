using System;

public class GameUIEvents 
{
    public event Action<string, Player> OnOpenMenu;
    public event Action OnCloseMenu;

    public void OpenMenu(string menuName, Player player)
    {
        OnOpenMenu?.Invoke(menuName, player);
    }

    public void CloseMenu()
    {
        OnCloseMenu?.Invoke();
    }
}
