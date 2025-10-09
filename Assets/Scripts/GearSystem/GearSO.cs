using UnityEngine;

[CreateAssetMenu(fileName = "GearSO", menuName = "Scriptable Objects/GearSO")]
public class GearSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    public Transform Prefab;
    public string DisplayName;
    public Sprite Sprite;
    public Sprite Icon;
    public float BaseCost;
    public string Description;
    public GearUpgrades[] EquipmentUpgrades;
}
