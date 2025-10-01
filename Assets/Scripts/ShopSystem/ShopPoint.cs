using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class ShopPoint : MonoBehaviour
{
    private bool playerIsNear = false;

    private void Update()
    {
        if (playerIsNear)
        {
            if (InputManager.InteractPressed)
            {

            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
