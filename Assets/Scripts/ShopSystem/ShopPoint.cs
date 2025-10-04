using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class ShopPoint : MonoBehaviour, IInteractable
{
    public bool canInteract { get; set; } = true;

    public void Interact()
    {
        GameEventsManager.Instance.GameUIEvents.OpenMenu("UI_SHOP");
    }
}
