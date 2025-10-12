using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGrid : MonoBehaviour
{
    private InventoryManager inventoryManager;  

    public const float BaseTileWidth = 76f;
    public const float BaseTileHeight = 76f;
    public Vector2 ReferenceResolution = new Vector2(1920, 1080); // Base resolution for scaling

    public float TileWidth => BaseTileWidth;
    public float TileHeight => BaseTileHeight;

    public int GridWidth = 10;
    public int GridHeight = 10;

    private InventoryItem[,] gridItems;
    public InventoryItem[,] GridItems => gridItems;

    private List<Vector2Int> emptySlots = new List<Vector2Int>();

    private RectTransform rectTransform;

    private Vector2 positionOnGrid = new Vector2();
    private Vector2Int tileGridPosition = new Vector2Int();

    public int TotalSlots = 0;
    public int OccupiedSlots = 0;

    public float GetScaleFactor()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Calculate the scale factor based on width (or height) ratio compared to the reference resolution
        float scaleFactor = Mathf.Min(screenWidth / ReferenceResolution.x, screenHeight / ReferenceResolution.y);

        return scaleFactor;
    }

    public void InitializeGrid(int width, int height, InventoryManager inventoryManager)
    {
        this.inventoryManager = inventoryManager;
        GridWidth = width;
        GridHeight = height;
        rectTransform = GetComponent<RectTransform>();
        gridItems = new InventoryItem[GridWidth, GridHeight];

        Vector2 size = new Vector2(GridWidth * TileWidth, GridHeight * TileHeight);
        rectTransform.sizeDelta = size;

        TotalSlots = GridWidth * GridHeight;
        InitializeEmptySlots();
    }

    public void ChangeGridSize(int width, int height)
    {
        int newWidth = GridWidth + width;
        int newHeight = GridHeight + height;


        InventoryItem[,] newGridItems = new InventoryItem[newWidth, newHeight];

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if (gridItems[x, y] != null) //Only copy if there is an item
                {
                    newGridItems[x, y] = gridItems[x, y];
                }
            }
        }

        gridItems = newGridItems;

        GridWidth = newWidth;
        GridHeight = newHeight;

        Vector2 size = new Vector2(GridWidth * TileWidth, GridHeight * TileHeight);
        rectTransform.sizeDelta = size;

        TotalSlots = GridWidth * GridHeight;
        InitializeEmptySlots();
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
        if (OverlapCheck(posX, posY, item.ItemDataSO.Width, item.ItemDataSO.Height) == false) return false;
        if (BoundaryCheck(posX, posY, item.ItemDataSO.Width, item.ItemDataSO.Height) == false) return false;

        RectTransform itemRectTransform = item.RectTransform;
        itemRectTransform.SetParent(this.rectTransform);

        //Make item occupy the correct number of slots
        for (int x = 0; x < item.ItemDataSO.Width; x++)
        {
            for (int y = 0; y < item.ItemDataSO.Height; y++)
            {
                gridItems[posX + x, posY + y] = item;
                emptySlots.Remove(new Vector2Int(posX + x, posY + y));
            }
        }

        item.PositionOnGridX = posX;
        item.PositionOnGridY = posY;
        item.CurrentGrid = this;

        Vector2 position = new Vector2();
        position.x = posX * TileWidth + TileWidth * item.ItemDataSO.Width / 2;
        position.y = -(posY * TileHeight + TileHeight * item.ItemDataSO.Height / 2);

        itemRectTransform.localPosition = position;
        OccupiedSlots += item.ItemDataSO.Width * item.ItemDataSO.Height;
        return true;
    }

    public InventoryItem PickUpItem(int posX, int posY)
    {
        Debug.Log(posX);
        Debug.Log(posY);
        InventoryItem item = gridItems[posX, posY];

        if (item == null) return null;

        OccupiedSlots -= item.ItemDataSO.Width * item.ItemDataSO.Height;
        RemoveItem(item);

        return item;    
    }

    public void RemoveItem(InventoryItem item)
    {
        //Remove the correct number of slots
        for (int x = 0; x < item.ItemDataSO.Width; x++)
        {
            for (int y = 0; y < item.ItemDataSO.Height; y++)
            {
                gridItems[item.PositionOnGridX + x, item.PositionOnGridY + y] = null;
                emptySlots.Add(new Vector2Int(item.PositionOnGridX + x, item.PositionOnGridY + y));
            }
        }

    }

    bool OverlapCheck(int posX, int posY, int itemWidth, int itemHeight)
    {
        for (int x = 0; x < itemWidth; x++)
        {
            for (int y = 0; y < itemHeight; y++)
            {
                if (posX + x >= gridItems.GetLength(0) || posY + y >= gridItems.GetLength(1))
                {
                    return false; //Out of bounds
                }

                if (gridItems[posX + x, posY + y] != null)
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

        if (posX > GridWidth || posY > GridHeight) return false;

        return true;
    }

    bool BoundaryCheck(int posX, int posY, int itemWidth, int itemHeight)
    {
        posX += itemWidth - 1;
        posY += itemHeight - 1;

        if (PositionCheck(posX, posY) == false) return false;

        return true;
    }

    public bool CheckHasEmptySlot(ItemDataSO itemData)
    {
        SortEmptySlots();
        if (emptySlots.Count == 0) return false;

        for (int i = 0; i < emptySlots.Count; i++)
        {
            if (BoundaryCheck(emptySlots[i].x, emptySlots[i].y, itemData.Width, itemData.Height) == true)
            {
                if (OverlapCheck(emptySlots[i].x, emptySlots[i].y, itemData.Width, itemData.Height) == true)
                {
                    return true;
                }
            }
            
        }

        return false;
    }

    public Vector2Int? GetEmptySlot(InventoryItem item)
    {
        SortEmptySlots();
        for (int i = 0; i < emptySlots.Count; i++)
        {
            if (BoundaryCheck(emptySlots[i].x, emptySlots[i].y, item.ItemDataSO.Width, item.ItemDataSO.Height) == true)
            {
                if (OverlapCheck(emptySlots[i].x, emptySlots[i].y, item.ItemDataSO.Width, item.ItemDataSO.Height) == true)
                {
                    return emptySlots[i];
                }
            }   
        }

        Debug.LogError("Somehow adding item to full inventory");
        return null;
    }

    private void SortEmptySlots()
    {
        emptySlots.Sort((a, b) =>
        {
            int yComparison = a.y.CompareTo(b.y);
            if (yComparison != 0) return yComparison;  

            return a.x.CompareTo(b.x); 
        });
    }

    private void InitializeEmptySlots()
    {
        emptySlots.Clear();

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if (gridItems[x, y] == null)
                {
                    emptySlots.Add(new Vector2Int(x, y)); //Add empty slot to the list
                }
            }
        }
    }
}
