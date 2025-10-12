using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearUpgradeInfoUI : MonoBehaviour
{
    public Image Icon;
    [SerializeField] private TMP_Text displayNameText;
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text descriptionText;

    public void Initialize(GearUpgrade gearUpgrade)
    {
        displayNameText.text = gearUpgrade.GearUpgradeData.DisplayName;
        lvlText.text = "Lvl " + (gearUpgrade.Level + 1);
        descriptionText.text = gearUpgrade.Description;
    }

    public void InitializeNextUpgrade(GearUpgrade gearUpgrade)
    {
        displayNameText.text = gearUpgrade.GearUpgradeData.DisplayName;
        lvlText.text = "Lvl " + (gearUpgrade.Level + 2);
        descriptionText.text = gearUpgrade.UpgradedDescription;
    }

}
