using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerOxygen : MonoBehaviour
{
    Player player;

    [Header("Oxygen Settings")]
    [SerializeField] private float maxOxygenTime = 600f;
    [SerializeField] private float currentOxygen;
    [SerializeField] private float oxygenDecayRate = 1f;     //oxygen lost per second underwater
    [SerializeField] private float oxygenRefillRate = 40f;   //oxygen gained per second at surface or air pocket

    [Header("State")]
    private bool isUnderwater = false;
    private bool isInAirPocket = false;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint; //insert surface spawn point here

    [Header("UI")]
    [SerializeField] private Image oxygenBar_Filled;

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

    //Properties
    public float MaxOxygenTime => maxOxygenTime;

    private void Start()
    {
        player = GetComponent<Player>();
        currentOxygen = maxOxygenTime;
    }

    private void Update()
    {
        if (player.PlayerHead.transform.position.y >= player.PlayerController.WaterLevel)
        {
            player.PlayerOxygen.isUnderwater = false;
        }
        else
        {
            player.PlayerOxygen.isUnderwater = true;
        }

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

        currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygenTime);

        //ui oxygen
        oxygenBar_Filled.fillAmount = currentOxygen / maxOxygenTime;
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
        currentOxygen = maxOxygenTime;

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
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("AirPocket"))
            isInAirPocket = false;
    }

    public void UpgradeOxygen(float amount)
    {
        maxOxygenTime += amount;
        currentOxygen = maxOxygenTime; //refill on upgrade
    }
}
