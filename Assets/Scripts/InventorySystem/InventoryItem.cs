using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    public ItemDataSO ItemData;

    public int PositionOnGridX;
    public int PositionOnGridY;

    public void InitializeItem(ItemDataSO itemData)
    {
        this.ItemData = itemData;
        image.sprite = itemData.InventorySprite;
        image.SetNativeSize();
    }

}
