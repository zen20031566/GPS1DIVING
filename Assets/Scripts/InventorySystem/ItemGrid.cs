using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    const float tileWidth = 76;
    const float tileHeight = 76;

    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;

    InventoryItem[,] inventoryItemSlot;

    private RectTransform rectTransform;

    private Vector2 positionOnGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        InitializeGrid(gridWidth, gridHeight);
    }

    public void InitializeGrid(int width, int height)
    {
       inventoryItemSlot = new InventoryItem[width, height];
        Vector2 size = new Vector2(width * tileWidth, height * tileHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnGrid.x / tileWidth);
        tileGridPosition.y = (int)(positionOnGrid.y / tileHeight);

        return tileGridPosition;    
    }

    public void PlaceItem(InventoryItem item, int posX, int posY)
    {
        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        itemRectTransform.SetParent(this.rectTransform);

        inventoryItemSlot[posX, posY] = item;

        Vector2 position = new Vector2();
        position.x = posX * tileWidth + tileWidth * item.ItemData.Width / 2;
        position.y = -(posY * tileHeight + tileHeight * item.ItemData.Height / 2);

        itemRectTransform.localPosition = position;
    }

    public InventoryItem PickUpItem(int posX, int posY)
    {
        InventoryItem toReturn = inventoryItemSlot[posX, posY];
        inventoryItemSlot[posX, posY] = null;

        return toReturn;    
    }
}
