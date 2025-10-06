using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataSO", menuName = "Scriptable Objects/ItemDataSO")]
public class ItemDataSO : ScriptableObject
{
    public int Id;
    public string DisplayName;
    public Sprite DisplaySprite;
    public Sprite InventorySprite;
    public Sprite Icon;
    public int Width = 1;
    public int Height = 1;
    public Item prefab;
    public ItemWeight weight;
    public ItemRarity rarity;

    private void OnValidate()
    {
        #if UNITY_EDITOR
                DisplayName = this.name;
                UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }
}
