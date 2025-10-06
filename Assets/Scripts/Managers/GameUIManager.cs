using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    private bool menuIsOpen = false;
    private GameObject currentMenu = null;

    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject inventoryUI;

    private void OnEnable()
    {
        GameEventsManager.Instance.GameUIEvents.OnOpenMenu += OpenMenu;
        GameEventsManager.Instance.GameUIEvents.OnCloseMenu += CloseMenu;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.GameUIEvents.OnOpenMenu -= OpenMenu;
        GameEventsManager.Instance.GameUIEvents.OnCloseMenu -= CloseMenu;
    }

    private void Update()
    {
        if (menuIsOpen)
        {
            if (InputManager.EscPressed)
            {
                CloseMenu();
            }
        }
    }

    private void OpenMenu(string menuName)
    {
        if (menuIsOpen) return;

        menuIsOpen = true;

        switch (menuName)
        {
            case "Shop":
                shopUI.SetActive(true); 
                currentMenu = shopUI;
                break;

            case "Inventory":
                inventoryUI.SetActive(true);
                currentMenu = inventoryUI;
                break;

            default:
                Debug.Log("Failed to open menu " + menuName);
                menuIsOpen = false;
                currentMenu = null;
                break;
        }
    }

    private void CloseMenu()
    {
        if (currentMenu != null)
        {
            menuIsOpen = false;
            currentMenu.SetActive(false);
            currentMenu = null;
        }
    } 
}
