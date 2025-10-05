using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]

public class GridInteract : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    InventoryControlller inventoryControlller;
    ItemGrid itemGrid;

    private void Awake()
    {
        inventoryControlller = FindObjectOfType(typeof(InventoryControlller)) as InventoryControlller;
        itemGrid = GetComponent<ItemGrid>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointed enter");
        inventoryControlller.selectedItemGrid = itemGrid;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointed exit");
        inventoryControlller.selectedItemGrid = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
