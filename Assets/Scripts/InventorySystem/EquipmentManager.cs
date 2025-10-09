using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public List<ItemData> EquipedItems = new List<ItemData>();

    public void InitializeHotbar()
    {

        
    }

    private void InitializeEquipment()
    {
        foreach (ItemData itemData in EquipedItems)
        {
            Item item = Instantiate(itemData.ItemDataSO.prefab, GameManager.Instance.PlayerTransform.position, Quaternion.identity, transform);
            item.InitializeItem(itemData.ItemDataSO);
        }
    }
}
