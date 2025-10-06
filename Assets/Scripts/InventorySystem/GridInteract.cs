using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]

public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    InventoryManager inventoryManager;
    ItemGrid itemGrid;

    private void Awake()
    {
        inventoryManager = Object.FindFirstObjectByType(typeof(InventoryManager)) as InventoryManager;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointed enter");
        inventoryManager.CurrentItemGrid = itemGrid;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointed exit");
        inventoryManager.CurrentItemGrid = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
