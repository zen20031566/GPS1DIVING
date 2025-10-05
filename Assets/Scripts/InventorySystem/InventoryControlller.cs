using UnityEngine;

public class InventoryControlller : MonoBehaviour
{
    public ItemGrid selectedItemGrid { get; set; }

    private InventoryItem selectedItem;

    private void Update()
    {
        if (selectedItemGrid == null) return;

        if (Input.GetMouseButtonDown(0))
        {
            Vector2Int tileGridPosition = selectedItemGrid.GetTileGridPosition(Input.mousePosition);

            if (selectedItem == null)
            {
                selectedItem = selectedItemGrid.PickUpItem(tileGridPosition.x, tileGridPosition.y);
            }
            else
            {
                selectedItemGrid.PlaceItem(selectedItem, tileGridPosition.x, tileGridPosition.y);
                selectedItem = null;
            }
        }
        
    }
}
