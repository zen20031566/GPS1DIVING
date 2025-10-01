using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Equipment : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public EquipmentSO Data;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void InitializeEquipment(EquipmentSO equipmentData)
    {
        Data = equipmentData;
        spriteRenderer.sprite = Data.Sprite;
    }
}
