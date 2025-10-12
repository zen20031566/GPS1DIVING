using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<ItemData> playerItems = new List<ItemData>();
    public List<ItemData> PlayerItems => playerItems;

    public ItemGrid CurrentItemGrid;
    [SerializeField] private ItemGrid selectedItemGrid;
    [SerializeField] private LayerMask inventoryLayer;

    [SerializeField] private InventoryItem selectedItem;

    [SerializeField] private InventoryItem inventoryItemPrefab;
    [SerializeField] private Transform canvasTransform;

    public bool IsPointerOnInventory;

    [SerializeField] TMP_Text slotsText;

    public ItemGrid InventoryGrid;
    public ItemGrid WeaponSlot1;
    public ItemGrid WeaponSlot2;
    public ItemGrid ConsumablesSlots;

    [SerializeField] Vector2Int inventorySize = new Vector2Int(8, 10);
    [SerializeField] Vector2Int weaponSlotSize = new Vector2Int(3, 2); //Maybe change in the future cause u dont want limit weapon size? new class of innventory slot idk
    [SerializeField] Vector2Int consumablesSlotsSize = new Vector2Int(3, 1);

    private Player player;
    private PlayerEquipment playerEquipment;

    private void Start()
    {
        InventoryGrid.InitializeGrid(inventorySize.x, inventorySize.y, this);
        WeaponSlot1.InitializeGrid(weaponSlotSize.x, weaponSlotSize.y, this);
        WeaponSlot2.InitializeGrid(weaponSlotSize.x, weaponSlotSize.y, this);
        ConsumablesSlots.InitializeGrid(consumablesSlotsSize.x, consumablesSlotsSize.y, this);
        player = GameManager.Instance.Player;
        playerEquipment = player.PlayerEquipment;
        UpdateSlotsCounter();
    }

    private void Update()
    {
        //if (CurrentItemGrid != null)
        //{
        //    Debug.Log(GetTileGridPosition());
        //}
        DragItem();

        if (Input.GetKeyDown(KeyCode.I))
        {
            GameEventsManager.Instance.GameUIEvents.OpenMenu("Inventory", player);
        }

        if (player.PlayerStateMachine.CurrentState != player.OnUIOrDialog) return;

        if (InputManager.LeftClickPressed)
        {
            HandleLeftClick();
        }
        else if (InputManager.LeftClickReleased)
        {
            HandleMouseReleased();
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

            UpdateSlotsCounter();

            GameEventsManager.Instance.InventoryEvents.ItemAdded(itemDataSO.Id);
        }
    }

    private void PickUpItem(Vector2Int tileGridPosition)
    {
        if (CurrentItemGrid == null) return;

        selectedItemGrid = CurrentItemGrid;
        selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);

        if (selectedItem == null) return;

        selectedItem.RectTransform.parent.SetAsLastSibling();
        selectedItem.RectTransform.SetAsLastSibling();

        HandleEquipmentRemove(tileGridPosition, CurrentItemGrid);
    }

    public void PlaceItem(Vector2Int tileGridPosition)
    {
        if (CurrentItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y))
        {
            HandleEquipmentPlace(tileGridPosition, CurrentItemGrid);

            selectedItem = null;
            selectedItemGrid = null;
            UpdateSlotsCounter();
        }
        else
        {
            ReturnSelectedItemBack();
        }
    }

    public void ReturnSelectedItemBack()
    {
        Vector2Int initialPos = new Vector2Int(selectedItem.PositionOnGridX, selectedItem.PositionOnGridY);
        selectedItemGrid.PlaceItem(selectedItem, initialPos.x, initialPos.y);
        HandleEquipmentPlace(initialPos, selectedItem.CurrentGrid);
        selectedItem = null;
        selectedItemGrid = null;
        UpdateSlotsCounter();
    }

    private void HandleEquipmentPlace(Vector2Int tileGridPosition, ItemGrid grid)
    {
        if (grid == WeaponSlot1)
        {
            playerEquipment.InstantiateEquipment(selectedItem.ItemData, 0);
        }
        else if (grid == WeaponSlot2)
        {
            playerEquipment.InstantiateEquipment(selectedItem.ItemData, 1);
        }
        else if (grid == ConsumablesSlots)
        {
            int slotIndex = tileGridPosition.x + 2;
            playerEquipment.InstantiateEquipment(selectedItem.ItemData, slotIndex);
        }
    }

    private void HandleEquipmentRemove(Vector2Int tileGridPosition, ItemGrid grid)
    {
        if (grid == WeaponSlot1)
        {
            playerEquipment.RemoveEquipment(0);
        }
        else if (grid == WeaponSlot2)
        {
            playerEquipment.RemoveEquipment(1);
        }
        else if (grid == ConsumablesSlots)
        {
            int slotIndex = tileGridPosition.x + 2;
            playerEquipment.RemoveEquipment(slotIndex);
        }
    }

    public void RemoveItem(InventoryItem inventoryItem, ItemGrid itemGrid)
    {
        playerItems.Remove(inventoryItem.ItemData);
        itemGrid.RemoveItem(inventoryItem);

        UpdateSlotsCounter();

        GameEventsManager.Instance.InventoryEvents.ItemRemoved(inventoryItem.ItemData.ItemDataSO.Id);

        Destroy(inventoryItem.gameObject);

    }

    private void DropItem(InventoryItem selectedItem)
    {
        ItemData itemData = selectedItem.ItemData;
        Item item = Instantiate(itemData.ItemDataSO.Prefab, player.transform.position, Quaternion.identity);
        item.InitializeItem(itemData.ItemDataSO);

        RemoveItem(selectedItem, selectedItemGrid);

        selectedItem = null;
        selectedItemGrid = null;
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
        }
    }

    private void HandleMouseReleased()
    {
        if (selectedItem == null) return;

        if (CurrentItemGrid != null)
        {
            Vector2Int tileGridPosition = GetTileGridPosition();
            ItemData itemData = selectedItem.ItemData;

            if (CanPlaceItemInCurrentGrid(itemData))
            {
                PlaceItem(tileGridPosition);
            }
            else
            {
                ReturnSelectedItemBack();
            }
        }
        else
        {
            Debug.Log(IsPointerOnInventory);
            //If cursor is outside inventory we drop the item
            if (!IsPointerOnInventory)
            {
                DropItem(selectedItem);
            }
            else
            {
                ReturnSelectedItemBack();
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
            position.x -= (selectedItem.ItemDataSO.Width - 1) * CurrentItemGrid.TileWidth / 2;
            position.y += (selectedItem.ItemDataSO.Height - 1) * CurrentItemGrid.TileHeight / 2;
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
        slotsText.text = InventoryGrid.OccupiedSlots + "/" + InventoryGrid.TotalSlots;
    }

    public void UpgradeInventory(int increaseRowAmount)
    {
        InventoryGrid.ChangeGridSize(0, increaseRowAmount);
        UpdateSlotsCounter();
    }

}
