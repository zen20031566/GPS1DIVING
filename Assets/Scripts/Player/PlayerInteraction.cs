using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private IInteractable closestInteractable = null;
    [SerializeField] private GameObject interactUI;

    private void Update()
    {
        if (InputManager.InteractPressed && closestInteractable != null)
        {
            closestInteractable.Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (closestInteractable != null) return;

        IInteractable interactable = collision.GetComponent<IInteractable>();
        if (interactable != null)
        {
            closestInteractable = interactable;
            interactUI.SetActive(true);

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        closestInteractable = null;
        interactUI.SetActive(false);
    }
}
