using Unity.Jobs;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        inventoryManager.ReturnSelectedItemBack();
        inventoryManager.CurrentItemGrid = null;
    }
}
