using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Gear : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public GearSO Data;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void InitializeEquipment(GearSO equipmentData)
    {
        Data = equipmentData;
        spriteRenderer.sprite = Data.Sprite;
    }
}
