using UnityEngine;
using UnityEngine.UI;

public class EquipmentTabSlot : MonoBehaviour
{
    private GearSO data;
    [SerializeField] private Image icon;
    
    public void InitializeEquipmentSlot(GearSO equipmentData)
    {
        data = equipmentData;
        icon.sprite = data.Icon;
    }  
}
