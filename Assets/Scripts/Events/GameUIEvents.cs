using System;

public class GameUIEvents 
{
    public event Action<string> OnOpenMenu;
    public event Action OnCloseMenu;

    public void OpenMenu(string menuName)
    {
        OnOpenMenu?.Invoke(menuName);
    }

    public void CloseMenu()
    {
        OnCloseMenu?.Invoke();
    }
}
