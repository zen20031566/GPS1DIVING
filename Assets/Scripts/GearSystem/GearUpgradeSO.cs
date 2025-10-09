using UnityEngine;

[CreateAssetMenu(fileName = "GearUpgradesSO", menuName = "Scriptable Objects/GearUpgradesSO")]
public class GearUpgrades : ScriptableObject
{
    [field: SerializeField] public string Id { get; private set; }

    public string DisplayName;
    public Sprite Icon;
    public float BaseCost;
    public string Description;
}
