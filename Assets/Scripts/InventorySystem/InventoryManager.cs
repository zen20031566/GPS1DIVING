using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public ItemGrid SelectedItemGrid { get; set; }
    public ItemGrid InventoryGrid;

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

    public void AddItem(ItemDataSO itemData)
    {
        if (InventoryGrid.CheckHasEmptySlot())
        {
            InventoryItem item = Instantiate(inventoryItemPrefab, canvasTransform);
            item.InitializeItem(itemData);
            Vector2Int emptySlot = InventoryGrid.GetEmptySlot();
            InventoryGrid.PlaceItem(item, emptySlot.x, emptySlot.y);
        }
    }

    private void CreateRandomItem()
    {
        Debug.Log("Created");
        InventoryItem item = Instantiate(inventoryItemPrefab, canvasTransform);
        selectedItemRectTransform = item.GetComponent<RectTransform>();
        int selectedItemID = UnityEngine.Random.Range(0, playerItems.Count);
        item.InitializeItem(playerItems[selectedItemID]);
        selectedItem = item;
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
