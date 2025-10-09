using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerOxygen : MonoBehaviour
{
    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygen = 100f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenDecayRate = 5f;     //oxygen lost per second underwater
    [SerializeField] private float oxygenRefillRate = 40f;   //oxygen gained per second at surface or air pocket

    [Header("State")]
    public bool isUnderwater = false;
    public bool isInAirPocket = false;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint; //insert surface spawn point here

    [Header("UI")]
    [SerializeField] private Image oxygenBar_Filled;
    [SerializeField] private Image oxygenBar_Back;

    [Header("Low Oxygen Effect")] //for black overlay when oxygen low
    [SerializeField] private Image lowOxygenOverlay;  // UI overlay here (might change)
    [SerializeField] float lowOxygenThreshold = 35f;  //trigger when below this level
    [SerializeField] float flashSpeed = 3f;  //how fast it flashes
    private bool isLowOxygen = false;
    private float flashTimer = 0f;

    [Header("Fade Effect")]
    public Image blackFade; //UI here
    public float fadeSpeed = 2f; //speed of fade in/out
    private bool isFadingOut = false;

    private void Start()
    {
        currentOxygen = maxOxygen;
    }

    private void Update()
    {
        HandleOxygen();

        if (currentOxygen <= 0 && !isFadingOut)
        {
            StartCoroutine(FadeOutAndRespawn());
        }

        HandleLowOxygenEffect();
    }

    private void HandleOxygen()
    {
        if (isUnderwater && !isInAirPocket) //decrease oxygen
        {
            currentOxygen -= oxygenDecayRate * Time.deltaTime;
        }
        else //increase oxygen
        {
            currentOxygen += oxygenRefillRate * Time.deltaTime;
        }

        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);

        //ui oxygen
        oxygenBar_Filled.fillAmount = currentOxygen / maxOxygen;
        oxygenBar_Back.fillAmount = maxOxygen;
    }

    private void HandleLowOxygenEffect()
    {
        if (lowOxygenOverlay == null) return;

        if (currentOxygen <= lowOxygenThreshold)
        {
            isLowOxygen = true;
            flashTimer += Time.deltaTime * flashSpeed;
            float alpha = (Mathf.Sin(flashTimer) + 1) / 2f; //oscillates between 0 and 1
            lowOxygenOverlay.color = new Color(0f, 0f, 0f, alpha * 0.5f); //up to 50% opacity
        }
        else
        {
            isLowOxygen = false;
            //fade out the overlay when safe
            Color c = lowOxygenOverlay.color;
            c.a = Mathf.Lerp(c.a, 0f, Time.deltaTime * 2f);
            lowOxygenOverlay.color = c;
            flashTimer = 0f;
        }
    }

    private IEnumerator FadeOutAndRespawn()
    {
        isFadingOut = true;
        float alpha = 0f;

        //Fade to black
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (blackFade != null)
                blackFade.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        //Respawn player
        RespawnPlayer();

        //Small delay before fade back in
        yield return new WaitForSeconds(1f);

        //Fade back to normal
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * fadeSpeed;
            if (blackFade != null)
                blackFade.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        isFadingOut = false;
    }


    private void RespawnPlayer()
    {
        Debug.Log("Out of oxygen. Respawning...");
        currentOxygen = maxOxygen;

        //move player to respawn point
        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //set default bool in case
        isUnderwater = false;
        isInAirPocket = false;
    }

    //detect entering an air pocket or surface (surface counts for respawn n checkpoint)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("AirPocket"))
            isInAirPocket = true;

        if (other.CompareTag("Surface"))
            isUnderwater = false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AirPocket"))
            isInAirPocket = false;
    }

    //Upgrade (WIP)
    public void UpgradeOxygen(float amount)
    {
        maxOxygen += amount;
        currentOxygen = maxOxygen; //refill on upgrade
    }
}
