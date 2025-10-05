using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    public const float TileWidth = 76;
    public const float TileHeight = 76;

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
        Vector2 size = new Vector2(width * TileWidth, height * TileHeight);
        rectTransform.sizeDelta = size;
    }

    public Vector2Int GetTileGridPosition(Vector2 mousePosition)
    {
        positionOnGrid.x = mousePosition.x - rectTransform.position.x;
        positionOnGrid.y = rectTransform.position.y - mousePosition.y;

        tileGridPosition.x = (int)(positionOnGrid.x / TileWidth);
        tileGridPosition.y = (int)(positionOnGrid.y / TileHeight);

        return tileGridPosition;    
    }

    public bool PlaceItem(InventoryItem item, int posX, int posY)
    {
        if (OverlapCheck(posX, posY, item.ItemData.Width, item.ItemData.Height) == false) return false;
        if (BoundaryCheck(posX, posY, item.ItemData.Width, item.ItemData.Height) == false) return false;
        
        RectTransform itemRectTransform = item.GetComponent<RectTransform>();
        itemRectTransform.SetParent(this.rectTransform);

        //Make item occupy the correct number of slots
        for (int x = 0; x < item.ItemData.Width; x++)
        {
            for (int y = 0; y < item.ItemData.Height; y++)
            {
                inventoryItemSlot[posX + x, posY + y] = item;
            }
        }

        item.PositionOnGridX = posX;
        item.PositionOnGridY = posY;

        Vector2 position = new Vector2();
        position.x = posX * TileWidth + TileWidth * item.ItemData.Width / 2;
        position.y = -(posY * TileHeight + TileHeight * item.ItemData.Height / 2);

        itemRectTransform.localPosition = position;

        return true;
    }

    public InventoryItem PickUpItem(int posX, int posY)
    {
        InventoryItem item = inventoryItemSlot[posX, posY];

        if (item == null) return null;

        //Remove the correct number of slots
        for (int x = 0; x < item.ItemData.Width; x++)
        {
            for (int y = 0; y < item.ItemData.Height; y++)
            {
                inventoryItemSlot[item.PositionOnGridX + x, item.PositionOnGridY + y] = null;
            }
        }
       
        return item;    
    }

    bool OverlapCheck(int posX, int posY, int itemWidth, int itemHeight)
    {
        for (int x = 0; x < itemWidth; x++)
        {
            for (int y = 0; y < itemHeight; y++)
            {
                if (inventoryItemSlot[posX + x, posY + y] != null)
                {
                    return false;
                }
            }
        }

        return true;
    }

    bool PositionCheck(int posX, int posY)
    {
        if (posX < 0 || posY < 0) return false;

        if (posX >= gridWidth || posY >= gridHeight) return false;

        return true;
    }

    bool BoundaryCheck(int posX, int posY, int itemWidth, int itemHeight)
    {
        posX += itemWidth - 1;
        posY += itemHeight - 1;

        if (PositionCheck(posX, posY) == false) return false;

        return true;
    }
}
