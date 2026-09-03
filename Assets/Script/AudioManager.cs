using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Source")]
    public AudioSource sfxSource;

    [Header("Player Sounds (Randomized)")]
    public AudioClip[] playerShootSounds;
    public AudioClip[] playerHitSounds;

    [Header("Enemy Sounds (Randomized)")]
    public AudioClip[] enemyHitSounds;
    public AudioClip[] enemyDieSounds;

    [Header("Combo / Streak Sounds")]
    public AudioClip[] streakKillSounds;
    public AudioClip streakAbsorbSound;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (sfxSource == null) sfxSource = GetComponent<AudioSource>();
    }

    private void PlayRandomSound(AudioClip[] clips)
    {
        if (clips == null || clips.Length == 0) return;

        int randomIndex = Random.Range(0, clips.Length);
        sfxSource.PlayOneShot(clips[randomIndex]);
    }

    public void PlayPlayerShoot() => PlayRandomSound(playerShootSounds);

    public void PlayPlayerHit() => PlayRandomSound(playerHitSounds);

    public void PlayEnemyHit() => PlayRandomSound(enemyHitSounds);

    public void PlayEnemyDie() => PlayRandomSound(enemyDieSounds);

    public void PlayStreakKill(int comboCount)
    {
        if (streakKillSounds == null || streakKillSounds.Length == 0) return;

        int index = comboCount - 2;

        index = Mathf.Clamp(index, 0, streakKillSounds.Length - 1);

        sfxSource.PlayOneShot(streakKillSounds[index]);
    }

    public void PlayStreakAbsorb()
    {
        if (streakAbsorbSound != null) sfxSource.PlayOneShot(streakAbsorbSound);
    }
}