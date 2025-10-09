using UnityEngine;
using System.Collections;

public class AirPocketRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float activeDuration = 10f;  //how long it stays active after player enters
    public float respawnDelay = 20f;    //how long before it reappears

    private Collider2D pocketCollider;
    private SpriteRenderer pocketSprite;
    private bool isDeactivating = false; //prevents multiple timers

    private void Awake()
    {
        pocketCollider = GetComponent<Collider2D>();
        pocketSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isDeactivating)
        {
            StartCoroutine(DisableAfterTime());
        }
    }

    private IEnumerator DisableAfterTime()
    {
        isDeactivating = true;

        //wait for the active duration (while player can use it)
        yield return new WaitForSeconds(activeDuration);

        //disable collider and visuals
        pocketCollider.enabled = false;
        if (pocketSprite != null)
            pocketSprite.enabled = false;

        //wait before reactivating
        yield return new WaitForSeconds(respawnDelay);

        //reactivate collider and visuals
        pocketCollider.enabled = true;
        if (pocketSprite != null)
            pocketSprite.enabled = true;

        isDeactivating = false;
    }
}
