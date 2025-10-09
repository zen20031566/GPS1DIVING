using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> playerItems = new List<ItemData>();

    public ItemGrid CurrentItemGrid;
    private ItemGrid selectedItemGrid;
    [SerializeField] private LayerMask inventoryLayer;

    [SerializeField] private InventoryItem selectedItem;

    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private Transform canvasTransform;

    public bool IsPointerOnInventory;

    private int totalSlots;
    private int occupiedSlots = 0;
    [SerializeField] TMP_Text slotsText;

    public ItemGrid InventoryGrid;
    public ItemGrid WeaponSlot1;
    public ItemGrid WeaponSlot2;
    public ItemGrid ConsumablesSlots;

    [SerializeField] Vector2Int inventorySize = new Vector2Int(8, 10);
    [SerializeField] Vector2Int weaponSlotSize = new Vector2Int(3, 2); //Maybe change in the future cause u dont want limit weapon size? new class of innventory slot idk
    [SerializeField] Vector2Int consumablesSlotsSize = new Vector2Int(3, 1);

    private Player player;

    private void Start()
    {
        InventoryGrid.InitializeGrid(inventorySize.x, inventorySize.y);
        WeaponSlot1.InitializeGrid(weaponSlotSize.x, weaponSlotSize.y);
        WeaponSlot2.InitializeGrid(weaponSlotSize.x, weaponSlotSize.y);
        ConsumablesSlots.InitializeGrid(consumablesSlotsSize.x, consumablesSlotsSize.y);

        totalSlots = inventorySize.x * inventorySize.y;

        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        DragItem();

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameEventsManager.Instance.GameUIEvents.OpenMenu("Inventory");
        }

        if (player.PlayerStateMachine.CurrentState != player.OnUIOrDialog) return;

        if (InputManager.LeftClickPressed)
        {
            HandleLeftClick();
        } 
    }

    public void AddItem(ItemData itemData)
    {
        ItemDataSO itemDataSO = itemData.ItemDataSO;
        if (InventoryGrid.CheckHasEmptySlot(itemDataSO))
        {
            
            playerItems.Add(itemData);

            InventoryItem inventoryItem = Instantiate(inventoryItemPrefab, canvasTransform);
            inventoryItem.InitializeItem(itemData);

            Vector2Int? emptySlot = InventoryGrid.GetEmptySlot(inventoryItem);

            if (emptySlot == null) return;

            InventoryGrid.PlaceItem(inventoryItem, emptySlot.Value.x, emptySlot.Value.y);

            occupiedSlots += itemDataSO.Width * itemDataSO.Height;
            UpdateSlotsCounter();
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        selectedItemGrid = CurrentItemGrid;
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem == null) return;

        selectedItem.RectTransform.parent.SetAsLastSibling();
        selectedItem.RectTransform.SetAsLastSibling();
    }

    private void PlaceItem(Vector2Int tileGridPosition)
    {
        if (CurrentItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y))
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

        occupiedSlots -= selectedItem.ItemDataSO.Width * selectedItem.ItemDataSO.Height;
        UpdateSlotsCounter();

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
            Vector2Int tileGridPosition = GetTileGridPosition();
            if (selectedItem == null)
            {
                PickUpItem(tileGridPosition);
            }
            else
            {
                ItemData itemData = selectedItem.ItemData;

                if (CanPlaceItemInCurrentGrid(itemData))
                {
                    PlaceItem(tileGridPosition);
                }
            }
        }

        Debug.Log(IsPointerOnInventory);
        //If cursor is outside inventory we drop the item
        if (!IsPointerOnInventory)
        {
            if (selectedItem != null)
            {
                RemoveItem(selectedItem);
            }
        }
    }

    private bool CanPlaceItemInCurrentGrid(ItemData itemData)
    {
        if (CurrentItemGrid == WeaponSlot1 || CurrentItemGrid == WeaponSlot2)
            return itemData.HasItemTag(ItemTag.Weapon);

        else if (CurrentItemGrid == ConsumablesSlots)
            return itemData.HasItemTag(ItemTag.Consumable);

        else
            return true;
    }

    private Vector2Int GetTileGridPosition()
    {
        Vector2 position = Input.mousePosition;

        //Offset mouse position by item size so drag and drop feels better
        if (selectedItem != null)
        {
            position.x -= (selectedItem.ItemDataSO.Width - 1) * ItemGrid.TileWidth / 2;
            position.y += (selectedItem.ItemDataSO.Height - 1) * ItemGrid.TileHeight / 2;
        }

        Vector2Int tileGridPosition = CurrentItemGrid.GetTileGridPosition(position);

        return tileGridPosition;
    }

    private void DragItem()
    {
        if (selectedItem != null)
        {
            selectedItem.RectTransform.position = Input.mousePosition;
        }
    }

    private void UpdateSlotsCounter()
    {
        slotsText.text = occupiedSlots + "/" + totalSlots;
    }

}
