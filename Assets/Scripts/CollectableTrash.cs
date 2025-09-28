using UnityEngine;

public class CollectableTrash : MonoBehaviour
{
    [SerializeField] private TrashSO trashData;

    private void CollectTrash()
    {
        //Informs trash collected and sends trash data to game events manager 
        GameEventsManager.Instance.TrashEvents.TrashCollected(trashData);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CollectTrash();
        }
    }
}