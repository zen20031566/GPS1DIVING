using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellItemInfoUI : MonoBehaviour
{
    private int totalSaleValue;
    private int quantity;

    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text displayNameText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text totalSaleValueText;

    public void Initialize(ItemDataSO itemDataSO, int quantity)
    {
        this.quantity = quantity;   
        totalSaleValue = itemDataSO.Value * quantity;

        icon.sprite = itemDataSO.Icon;
        displayNameText.text = itemDataSO.DisplayName;
        quantityText.text = "x " + quantity;
        totalSaleValueText.text = $"{totalSaleValue}";
        
    }


}
