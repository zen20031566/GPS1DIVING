using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ItemGrid))]

public class GridInteract : InventoryElement, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private ItemGrid itemGrid;

    protected override void Awake()  
    { 
        base.Awake();   
        itemGrid = GetComponent<ItemGrid>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        inventoryManager.CurrentItemGrid = itemGrid;
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        inventoryManager.CurrentItemGrid = null;
    }

}
