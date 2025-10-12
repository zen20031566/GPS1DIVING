using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopSellAllButton : MonoBehaviour
{
    [SerializeField] ShopManager shopManager;
    [SerializeField] private Color tabIdleColor = new Color(41f / 255f, 41f / 255f, 41f / 255f);
    [SerializeField] private Color tabHoverColor = Color.gray;
    public Image Background;

    //public void OnPointerEnter(PointerEventData eventData)
    //{
    //    Background.color = tabHoverColor;
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    Background.color = tabIdleColor;
    //}

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    shopManager.SellAllSellables();
    //}

    public void OnClick()
    {
        shopManager.SellAllSellables();
    }


}
