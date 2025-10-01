using UnityEngine;
using UnityEngine.UI;

public class EquipmentTabSlot : MonoBehaviour
{
    private EquipmentSO data;
    [SerializeField] private Image icon;
    

    public void InitializeEquipmentSlot(EquipmentSO equipmentData)
    {
        data = equipmentData;
        icon.sprite = data.Icon;
    }

    
}
