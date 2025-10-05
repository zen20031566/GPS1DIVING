using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public ItemGrid SelectedItemGrid { get; set; }

    private InventoryItem selectedItem;
    private RectTransform selectedItemRectTransform;
    
    [SerializeField] private List<ItemDataSO> playerItems;
    [SerializeField] private InventoryItem inventoryItemPrefab;

    [SerializeField] private Transform canvasTransform;

    private void Update()
    {
        DragItem();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CreateRandomItem();
        }

        if (SelectedItemGrid == null) return;

        if (InputManager.LeftClickPressed)
        {
            Debug.Log("MousePressedd");
            HandleLeftClick();
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
        SelectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
        selectedItem = null;
    }

    private void HandleLeftClick()
    {
        Vector2Int tileGridPosition = SelectedItemGrid.GetTileGridPosition(Input.mousePosition);

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
