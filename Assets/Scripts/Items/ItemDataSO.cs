using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public int Id;
    public string DisplayName;
    public Sprite DisplaySprite;
    public Sprite InventorySprite;
    public int Width = 1;
    public int Height = 1;
    public Item Prefab;
    public int Value;
    public ItemWeight Weight;
    public ItemRarity Rarity;
    public List<ItemTag> Tags;
}
