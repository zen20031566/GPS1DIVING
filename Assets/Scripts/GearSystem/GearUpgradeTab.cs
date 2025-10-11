using UnityEngine;
using UnityEngine.EventSystems;

public class GearUpgradeTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private ShopManager shopManager;
    private GearUpgrade upgrade;

    public void Initialize(GearUpgrade upgrade, ShopManager shopManager)
    {
        this.shopManager = shopManager;
        this.upgrade = upgrade;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       //Ping shop try get upgrade
    }

}
