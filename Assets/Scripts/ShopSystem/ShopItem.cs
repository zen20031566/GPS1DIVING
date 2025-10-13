using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text displayNameText;
    [SerializeField] private TMP_Text priceText;

    private ShopManager shopManager;
    private ItemDataSO itemDataSO;
    
    public void Initialize(ItemDataSO itemDataSO, ShopManager shopManager)
    {
        this.shopManager = shopManager;
        this.itemDataSO = itemDataSO;
        icon.sprite = itemDataSO.Icon;
        displayNameText.text = itemDataSO.DisplayName;
        priceText.text = $"{itemDataSO.Price}";   
    }

    public void OnClick()
    {
        shopManager.TryBuyItem(itemDataSO);
    }
}
