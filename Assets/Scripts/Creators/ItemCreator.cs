using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    [SerializeField] private Item ItemPrefab;

    private Dictionary<int, ItemDataSO> allItemsMap;

    public void SpawnItem(int id, Vector3 position)
    {
        ItemDataSO itemData = GetItemById(id);
        Item item = Instantiate(ItemPrefab, position, Quaternion.identity);
        item.InitializeItem(itemData);
    }

    public ItemDataSO GetItemById(int id)
    {
        ItemDataSO itemData = allItemsMap[id];

        if (itemData == null)
        {
            Debug.LogError("id not found in the item list: " + id);
        }

        return itemData;
    }
}
