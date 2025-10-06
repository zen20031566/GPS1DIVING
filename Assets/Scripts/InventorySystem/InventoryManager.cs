using NUnit.Framework.Interfaces;
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
    
    [SerializeField] private List<ItemData> playerItems = new List<ItemData>();

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

    public void AddItem(ItemDataSO itemDataSO)
    {
        if (InventoryGrid.CheckHasEmptySlot(itemDataSO))
        {
            ItemData itemData = new ItemData(itemDataSO);
            playerItems.Add(itemData);

            InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, canvasTransform);
            inventoryItem.InitializeItem(itemData);

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

    private void RemoveItem(InventoryItem selectedItem)
    {
        DropItem(selectedItem.ItemData);

        playerItems.Remove(selectedItem.ItemData);
        selectedItemGrid.RemoveItem(selectedItem);

        Destroy(selectedItem.gameObject);

        selectedItem = null;
        selectedItemGrid = null;

        Debug.Log("Dropped item");
    }

    private void DropItem(ItemData itemData)
    {
        Item item = Instantiate(itemData.ItemDataSO.prefab, GameManager.Instance.PlayerTransform.position, Quaternion.identity);
        item.InitializeItem(itemData.ItemDataSO);
    }

    private void HandleLeftClick()
    {
        if (CurrentItemGrid != null)
        {
            Vector2 position = Input.mousePosition;

            //Offset mouse position by item size so drag and drop feels better
            if (selectedItem != null)
            {
                position.x -= (selectedItem.ItemDataSO.Width - 1) * ItemGrid.TileWidth / 2;
                position.y += (selectedItem.ItemDataSO.Height - 1) * ItemGrid.TileHeight / 2;
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
                RemoveItem(selectedItem);
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
