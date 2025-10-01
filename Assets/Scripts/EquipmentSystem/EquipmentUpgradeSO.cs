using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentUpgradeSO", menuName = "Scriptable Objects/EquipmentUpgradeSO")]
public class EquipmentUpgradeSO : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    public string DisplayName;
    public Sprite Icon;
    public float BaseCost;
    public string Description;
}
