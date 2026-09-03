using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Score Settings")]
    public int currentScore = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI mainScoreText;
    public TextMeshProUGUI streakText;

    [Header("Streak Settings")]
    public float streakTimeout = 1.5f;

    private float streakTimer;
    private int pendingScore = 0;
    private int killStreakCount = 0;

    [Header("Juice Events")]
    public UnityEvent onScoreIncreased;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;

    private Coroutine mainScorePopCoroutine;
    private Coroutine streakPopCoroutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateMainScoreUI();
        if (streakText != null) streakText.gameObject.SetActive(false);

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        yield return new WaitForSecondsRealtime(1f);
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (pendingScore > 0)
        {
            streakTimer -= Time.deltaTime;

            if (streakTimer <= 0)
            {
                ApplyStreakToMainScore();
            }
        }
    }

    public void AddScore(int amount)
    {
        pendingScore += amount;
        killStreakCount++;
        streakTimer = streakTimeout;

        if (streakText != null)
        {
            streakText.gameObject.SetActive(true);
            streakText.text = "+" + pendingScore.ToString();

            if (streakPopCoroutine != null) StopCoroutine(streakPopCoroutine);
            streakPopCoroutine = StartCoroutine(PopAnimation(streakText.transform, 1.4f, 0.15f));
        }

        if (AudioManager.Instance != null && killStreakCount >= 2)
        {
            AudioManager.Instance.PlayStreakKill(killStreakCount);
        }
    }

    private void ApplyStreakToMainScore()
    {
        currentScore += pendingScore;
        pendingScore = 0;
        killStreakCount = 0;

        UpdateMainScoreUI();
        onScoreIncreased?.Invoke();

        if (streakText != null)
        {
            streakText.gameObject.SetActive(false);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayStreakAbsorb();
        }

        if (mainScoreText != null)
        {
            if (mainScorePopCoroutine != null) StopCoroutine(mainScorePopCoroutine);
            mainScorePopCoroutine = StartCoroutine(PopAnimation(mainScoreText.transform, 1.6f, 0.25f));
        }
    }

    private void UpdateMainScoreUI()
    {
        if (mainScoreText != null)
        {
            mainScoreText.text = "SCORE:" + currentScore.ToString();
        }
    }

    private IEnumerator PopAnimation(Transform target, float maxScale, float duration)
    {
        Vector3 originalScale = Vector3.one;
        Vector3 targetScale = new Vector3(maxScale, maxScale, maxScale);

        float halfDuration = duration / 2f;
        float elapsed = 0f;

        while (elapsed < halfDuration)
        {
            target.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / halfDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        elapsed = 0f;

        while (elapsed < halfDuration)
        {
            target.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / halfDuration);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        target.localScale = originalScale;
    }

    public void TriggerHitStop()
    {
        StartCoroutine(HitStopCoroutine(0.1f));
    }

    private IEnumerator HitStopCoroutine(float duration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1f;
    }
}