using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    public ItemData ItemData;
    public ItemDataSO ItemDataSO;
    public RectTransform RectTransform;

    public int PositionOnGridX;
    public int PositionOnGridY;
    public ItemGrid CurrentGrid;

    public void InitializeItem(ItemData itemData)
    {
        RectTransform = GetComponent<RectTransform>();
        this.ItemData = itemData;
        this.ItemDataSO = itemData.ItemDataSO;
        image.sprite = ItemDataSO.DisplaySprite;
        image.SetNativeSize();
    }

}
