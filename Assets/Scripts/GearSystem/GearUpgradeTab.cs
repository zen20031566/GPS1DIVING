using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class GearUpgradeTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private GearUpgradeInfoUI currentGearUpgrade;
    [SerializeField] private GearUpgradeInfoUI nextGearUpgrade;
    [SerializeField] private Image tabBackground;
    [SerializeField] private Color tabIdleColor = new Color(41f / 255f, 41f / 255f, 41f / 255f);
    [SerializeField] private Color tabHoverColor = Color.gray;
    private GearUpgrade gearUpgrade;

    public void Initialize(GearUpgrade gearUpgrade, ShopManager shopManager)
    {
        tabBackground.color = tabIdleColor;
        this.shopManager = shopManager;
        this.gearUpgrade = gearUpgrade;
        UpdateTab(gearUpgrade);
    }

    public void UpdateTab(GearUpgrade gearUpgrade)
    {
        this.gearUpgrade = gearUpgrade;
        currentGearUpgrade.Initialize(gearUpgrade);
        goldText.text = $"{gearUpgrade.Cost}";
        nextGearUpgrade.InitializeNextUpgrade(gearUpgrade);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabBackground.color = tabHoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabBackground.color = tabIdleColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shopManager.TryBuyGearUpgrade(gearUpgrade, this);
    }

}
