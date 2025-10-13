using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    Player player;

    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;

    [Header("UI")]
    [SerializeField] private Image healthBar_Filled;

    [Header("Damage Effect")]
    [SerializeField] private Image damageOverlay; // Red flash overlay
    [SerializeField] private float damageFlashDuration = 0.3f;
    private bool isDamageFlashing = false;

    [Header("Fade Effect")]
    [SerializeField] private Image blackFade;
    [SerializeField] private float fadeSpeed = 2f;
    private bool isFadingOut = false;

    [Header("Invincibility")]
    [SerializeField] private float invincibilityDuration = 1.5f;
    private float invincibilityTimer = 0f;
    public bool IsInvincible => invincibilityTimer > 0f;

    // Properties
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;

    private void Start()
    {
        player = GetComponent<Player>();
        currentHealth = maxHealth;
        UpdateHealthUI();

        if (damageOverlay != null)
        {
            Color c = damageOverlay.color;
            c.a = 0f;
            damageOverlay.color = c;
        }
    }

    private void Update()
    {
        // Update invincibility timer
        if (invincibilityTimer > 0f)
        {
            invincibilityTimer -= Time.deltaTime;
        }
    }

    public void TakeDamage(float damage)
    {
        // Optional: Remove invincibility check for testing
        // if (IsInvincible) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        UpdateHealthUI();
        StartCoroutine(DamageFlashEffect());

        // Start invincibility period (comment out to disable)
        invincibilityTimer = invincibilityDuration;

        if (currentHealth <= 0 && !isFadingOut)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthBar_Filled != null)
        {
            healthBar_Filled.fillAmount = currentHealth / maxHealth;
        }
    }

    private IEnumerator DamageFlashEffect()
    {
        if (damageOverlay == null || isDamageFlashing) yield break;

        isDamageFlashing = true;

        // Flash red
        float elapsed = 0f;
        while (elapsed < damageFlashDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0.5f, 0f, elapsed / damageFlashDuration);
            damageOverlay.color = new Color(1f, 0f, 0f, alpha);
            yield return null;
        }

        // Ensure it's fully transparent
        damageOverlay.color = new Color(1f, 0f, 0f, 0f);
        isDamageFlashing = false;
    }

    private void Die()
    {
        StartCoroutine(FadeOutAndRespawn());
    }

    private IEnumerator FadeOutAndRespawn()
    {
        isFadingOut = true;
        float alpha = 0f;

        // Fade to black
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * fadeSpeed;
            if (blackFade != null)
                blackFade.color = new Color(0f, 0f, 0f, alpha);
            yield return null;
        }

        // Respawn player
        RespawnPlayer();

        // Small delay before fade back in
        yield return new WaitForSeconds(1f);

        // Fade back to normal
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
        Debug.Log("Player died. Respawning...");
        currentHealth = maxHealth;
        UpdateHealthUI();

        // Move player to respawn point
        if (respawnPoint != null)
            transform.position = respawnPoint.position;
        else
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Reset invincibility
        invincibilityTimer = invincibilityDuration;
    }

    public void UpgradeMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth; // Refill on upgrade
        UpdateHealthUI();
    }
}
