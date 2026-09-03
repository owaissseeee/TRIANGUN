using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Stats")]
    public int maxHealth = 10;
    private int currentHealth;

    [Header("UI Settings")]
    public Slider healthSlider;
    public float healthLerpSpeed = 10f;

    [Header("Visuals (Hit Flash)")]
    [Tooltip("Drag all the SpriteRenderers attached to your player here.")]
    public SpriteRenderer[] playerSprites;
    public Color hitColor = Color.red;
    public float flashDuration = 0.1f;
    private Color[] originalColors;

    [Header("Juice Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    [Header("Juice Settings (Visuals)")]
    public GameObject bloodSplashPrefab;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
        }

        originalColors = new Color[playerSprites.Length];
        for (int i = 0; i < playerSprites.Length; i++)
        {
            if (playerSprites[i] != null)
            {
                originalColors[i] = playerSprites[i].color;
            }
        }
    }

    void Update()
    {
        if (healthSlider != null)
        {
            healthSlider.value = Mathf.Lerp(healthSlider.value, currentHealth, Time.deltaTime * healthLerpSpeed);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        StartCoroutine(HitFlashRoutine());

        onTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private IEnumerator HitFlashRoutine()
    {
        for (int i = 0; i < playerSprites.Length; i++)
        {
            if (playerSprites[i] != null) playerSprites[i].color = hitColor;
        }

        yield return new WaitForSecondsRealtime(flashDuration);

        for (int i = 0; i < playerSprites.Length; i++)
        {
            if (playerSprites[i] != null) playerSprites[i].color = originalColors[i];
        }
    }

    void Die()
    {
        onDeath?.Invoke();

        if (CameraController.Instance != null)
        {
            CameraController.Instance.TriggerDeathShake();
        }

        if (bloodSplashPrefab != null)
        {
            Quaternion splashRotation = transform.rotation * Quaternion.Euler(0f, 90f, 0f);
            Instantiate(bloodSplashPrefab, transform.position, splashRotation);
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.GameOver();
        }

        Destroy(gameObject);
    }
}