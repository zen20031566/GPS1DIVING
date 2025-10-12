using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    private bool menuIsOpen = false;
    private GameObject currentMenu = null;

    [SerializeField] private GameObject gameplayUI;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject pauseUI;

    private Player player;

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

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (InputManager.EscPressed)
        {
            if (menuIsOpen)
            {
                CloseMenu();
            }
            else
            {
                OpenMenu("Pause", player);
            }    
        }
    }

    private void OpenMenu(string menuName, Player player)
    {
        this.player = player;
        if (menuIsOpen) return;

        menuIsOpen = true;
        player.PlayerStateMachine.ChangeState(player.OnUIOrDialog);

        switch (menuName)
        {
            case "Shop":
                GameEventsManager.Instance.ShopEvents.ShopOpen(player);
                shopUI.SetActive(true);
                currentMenu = shopUI;
                break;

            case "Inventory":
                inventoryUI.SetActive(true);
                currentMenu = inventoryUI;
                break;

            case "Pause":
                pauseUI.SetActive(true);
                currentMenu = pauseUI;
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
            player.PlayerStateMachine.ChangeState(player.OnLandState);
        }
    } 
}
