using UnityEngine;

[CreateAssetMenu(fileName = "GearUpgradeSO", menuName = "Scriptable Objects/GearUpgradeSO")]
public class GearUpgradeSO : ScriptableObject
{
    public string DisplayName;
    public Sprite Icon;
    public int MaxLevel;
    public int BaseCost;
    public int CostIncrease;
    public string Description;
    public GearUpgrade Prefab;
}
