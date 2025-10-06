using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class ShopPoint : MonoBehaviour, IInteractable
{
    public bool CanInteract { get; set; } = true;

    public void Interact(Player player)
    {
        GameEventsManager.Instance.GameUIEvents.OpenMenu("Shop");
    }
}
