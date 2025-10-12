using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GearUpgradeInfoUI : MonoBehaviour
{
    public Image image;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text descriptionText;

    public void Initialize(GearUpgrade gearUpgrade)
    {
        nameText.text = gearUpgrade.GearUpgradeData.DisplayName;
        lvlText.text = "Lvl " + gearUpgrade.Level;
        descriptionText.text = gearUpgrade.Description;
    }

    public void InitializeNextUpgrade(GearUpgrade gearUpgrade)
    {
        nameText.text = gearUpgrade.GearUpgradeData.DisplayName;
        lvlText.text = "Lvl " + (gearUpgrade.Level + 1);
        descriptionText.text = gearUpgrade.Description;
    }

}
