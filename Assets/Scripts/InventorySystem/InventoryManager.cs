using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemGrid CurrentItemGrid;
    private ItemGrid selectedItemGrid;

    public ItemGrid InventoryGrid;

    public ItemGrid WeaponSlot1;
    public ItemGrid WeaponSlot2;
    public ItemGrid ConsumablesSlots;

    [SerializeField] private InventoryItem selectedItem;
    private RectTransform selectedItemRectTransform;
    
    [SerializeField] private List<Item> playerItems;

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

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameEventsManager.Instance.GameUIEvents.OpenMenu("Inventory");
        }

        if (InputManager.LeftClickPressed)
        {
            HandleLeftClick();
        } 
    }

    public void AddItem(Item item)
    {
        if (InventoryGrid.CheckHasEmptySlot(item.ItemData))
        {
            playerItems.Add(item);

            InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, canvasTransform);
            inventoryItem.InitializeItem(item);

            Vector2Int? emptySlot = InventoryGrid.GetEmptySlot(inventoryItem);

            if (emptySlot == null) return;

            InventoryGrid.PlaceItem(inventoryItem, emptySlot.Value.x, emptySlot.Value.y);
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItemGrid = CurrentItemGrid;
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem != null)
        {
            selectedItemRectTransform = selectedItem.GetComponent<RectTransform>();
        }
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        if (selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y))
        {
            selectedItem = null;
            selectedItemGrid = null;
        }
    }

    private void DropItem(InventoryItem selectedItem)
    {
        playerItems.Remove(selectedItem.Item);
        selectedItemGrid.RemoveItem(selectedItem);
        GameEventsManager.Instance.InventoryEvents.ItemDropped(selectedItem.Item);
        Destroy(selectedItem.gameObject);
        selectedItem = null;
        selectedItemGrid = null;

        Debug.Log("Dropped item");
    }

    private void HandleLeftClick()
    {
        if (CurrentItemGrid != null)
        {
            Vector2 position = Input.mousePosition;

            //Offset mouse position by item size so drag and drop feels better
            if (selectedItem != null)
            {
                position.x -= (selectedItem.ItemData.Width - 1) * ItemGrid.TileWidth / 2;
                position.y += (selectedItem.ItemData.Height - 1) * ItemGrid.TileHeight / 2;
            }

            Vector2Int tileGridPosition = CurrentItemGrid.GetTileGridPosition(position);

            if (selectedItem == null)
            {
                PickUpItem(tileGridPosition);
            }
            else
            {
                PlaceItem(tileGridPosition);
            }
        }

        //If cursor is outside inventory we drop the item
        else if (CurrentItemGrid == null)
        {
            if (selectedItem != null)
            {
                DropItem(selectedItem);
            }
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
