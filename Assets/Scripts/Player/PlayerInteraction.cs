using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Player player;
    private IInteractable closestInteractable = null;
    [SerializeField] private GameObject interactUI;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (InputManager.InteractPressed && closestInteractable != null)
        {
            closestInteractable.Interact(player);
            closestInteractable = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable))
        {
            closestInteractable = interactable;
            interactUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == closestInteractable)
        {
            closestInteractable = interactable;
            interactUI.SetActive(false);
        }
    }

}
