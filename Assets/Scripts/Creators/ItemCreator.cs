using System.Collections.Generic;
using UnityEngine;

public class ItemCreator : MonoBehaviour
{
    public static Item ItemPrefab;

    public static Dictionary<int, ItemDataSO> allItemsMap;

    private void Awake()
    {
        allItemsMap = CreateItemMap();
    }

    public static void SpawnItem(int id, Vector3 position)
    {
        ItemDataSO itemDataSO = GetItemById(id);
        Item item = Instantiate(ItemPrefab, position, Quaternion.identity);
        item.InitializeItem(itemDataSO);
    }

    private Dictionary<int, ItemDataSO> CreateItemMap()
    {
        ItemDataSO[] allItems = Resources.LoadAll<ItemDataSO>("Items");

        Dictionary<int, ItemDataSO> idToItemMap = new Dictionary<int, ItemDataSO>();

        foreach (ItemDataSO itemDataSO in allItems)
        {
            if (idToItemMap.ContainsKey(itemDataSO.Id))
            {
                Debug.LogWarning("Duplicate id found when creating item map: " + itemDataSO.Id);
                continue; //Skip this item to avoid duplicates
            }

            idToItemMap.Add(itemDataSO.Id, itemDataSO);
        }

        return idToItemMap;
    }

    public static ItemDataSO GetItemById(int id)
    {
        ItemDataSO itemDataSO = allItemsMap[id];

        if (itemDataSO == null)
        {
            Debug.LogError("id not found in the item list: " + id);
        }

        return itemDataSO;
    }
}
