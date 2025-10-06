using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    public Item Item;
    public ItemDataSO ItemData;

    public int PositionOnGridX;
    public int PositionOnGridY;

    public void InitializeItem(Item item)
    {
        this.Item = item;
        this.ItemData = item.ItemData;
        image.sprite = ItemData.DisplaySprite;
        image.SetNativeSize();
    }

}
