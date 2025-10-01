using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentSO", menuName = "Scriptable Objects/EquipmentSO")]
public class EquipmentSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    public Transform Prefab;
    public string DisplayName;
    public Sprite Sprite;
    public Sprite Icon;
    public float BaseCost;
    public string Description;
    public EquipmentUpgradeSO[] EquipmentUpgrades;
}
