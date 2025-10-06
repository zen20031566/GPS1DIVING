using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    public ItemData ItemData;
    public ItemDataSO ItemDataSO;

    public int PositionOnGridX;
    public int PositionOnGridY;

    public void InitializeItem(ItemData itemData)
    {
        this.ItemData = itemData;
        this.ItemDataSO = itemData.ItemDataSO;
        image.sprite = ItemDataSO.DisplaySprite;
        image.SetNativeSize();
    }

}
