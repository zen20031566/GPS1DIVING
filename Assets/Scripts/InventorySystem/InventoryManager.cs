using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemGrid SelectedItemGrid { get; set; }
    public ItemGrid InventoryGrid;
    public ItemGrid WeaponSlot1;
    public ItemGrid WeaponSlot2;
    public ItemGrid ConsumablesSlots;

    private InventoryItem selectedItem;
    private RectTransform selectedItemRectTransform;
    
    [SerializeField] private List<ItemDataSO> playerItems;
    [SerializeField] private InventoryItem inventoryItemPrefab;

    [SerializeField] private Transform canvasTransform;

    [SerializeField] private int inventoryWidth = 8;
    [SerializeField] private int inventoryHeight = 10;

    private void Start()
    {
        InventoryGrid.InitializeGrid(inventoryWidth, inventoryHeight);
    }

    private void Update()
    {
        DragItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameEventsManager.Instance.GameUIEvents.OpenMenu("Inventory");
        }

        if (SelectedItemGrid == null) return;

        if (InputManager.LeftClickPressed)
        {
            Debug.Log("MousePressedd");
            HandleLeftClick();
        } 
    }

    public void AddItem(Item item)
    {
        if (InventoryGrid.CheckHasEmptySlot(item.ItemData))
        {
            InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, canvasTransform);
            inventoryItem.InitializeItem(item.ItemData);
            Vector2Int? emptySlot = InventoryGrid.GetEmptySlot(inventoryItem);

            if (emptySlot == null) return;

            Debug.Log(emptySlot.Value.x + emptySlot.Value.y);

            InventoryGrid.PlaceItem(inventoryItem, emptySlot.Value.x, emptySlot.Value.y);
        }
    }

    private void CreateRandomItem()
    {
        Debug.Log("Created");
        InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, canvasTransform);
        selectedItemRectTransform = inventoryItem.GetComponent<RectTransform>();
        int selectedItemID = UnityEngine.Random.Range(0, playerItems.Count);
        inventoryItem.InitializeItem(playerItems[selectedItemID]);
        selectedItem = inventoryItem;
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItem = SelectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem != null)
        {
            selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        if (SelectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y))
        {
            selectedItem = null;
        }
    }

    private void HandleLeftClick()
    {
        Vector2 position = Input.mousePosition;

        //Offset mouse position by item size so drag and drop feels better
        if (selectedItem != null)
        {
            position.x -= (selectedItem.ItemData.Width - 1) * ItemGrid.TileWidth / 2;
            position.y += (selectedItem.ItemData.Height - 1) * ItemGrid.TileHeight / 2;
        }

        Vector2Int tileGridPosition = SelectedItemGrid.GetTileGridPosition(position);

        if (selectedItem == null)
        {
            PickUpItem(tileGridPosition);
        }
        else
        {
            PlaceItem(tileGridPosition);
        }
    }

    private void DragItem()
    {
        if (selectedItem != null)
        {
            selectedItemRectTransform.position = Input.mousePosition;
        }
    }
}
