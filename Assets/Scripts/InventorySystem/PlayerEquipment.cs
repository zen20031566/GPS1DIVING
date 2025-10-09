using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [SerializeField] private Transform equipedItemTransform;

    public List<ItemData> EquipedItems = new List<ItemData>();

    public void UpdateEquipmentSlot()
    {
        ClearEquipment();

        foreach (ItemData itemData in EquipedItems)
        { 
            Item item = Instantiate(itemData.ItemDataSO.prefab, equipedItemTransform.position, Quaternion.identity, equipedItemTransform);
            item.InitializeItem(itemData.ItemDataSO);
            item.rb.simulated = false;

            if (item is IEquipable equipableItem)
            {
                equipableItem.Equip();
            }
        }
    }

    public void ClearEquipment()
    {
        foreach (Transform item in equipedItemTransform)
        {
            Destroy(item.gameObject);
        }
    }
}
