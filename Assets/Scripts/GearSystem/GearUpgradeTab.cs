using UnityEngine;
using UnityEngine.EventSystems;

public class GearUpgradeTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private GearUpgrade upgrade;

    public void Instantiate(GearUpgrade upgrade)
    {
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
