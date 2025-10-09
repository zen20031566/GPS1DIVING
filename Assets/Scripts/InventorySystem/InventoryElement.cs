using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    protected InventoryManager inventoryManager;

    private void OnDisable()
    {
        inventoryManager.IsPointerOnInventory = false;
    }

    protected virtual void Awake()
    {
        inventoryManager = Object.FindFirstObjectByType(typeof(InventoryManager)) as InventoryManager;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointed enter");
        inventoryManager.IsPointerOnInventory = true;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointed exit");
        inventoryManager.IsPointerOnInventory = false;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {

    }
}
