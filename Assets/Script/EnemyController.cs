using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public int health = 3;
    public int scoreValue = 10;
    public float moveSpeed = 2f;

    [Header("Shooting Settings")]
    public float fireRate = 2f;
    public float projectileSpeed = 8f;
    public GameObject enemyProjectilePrefab;
    public Transform firePoint;

    [Header("Shotgun Settings")]
    public int projectilesPerShot = 1;
    public float spreadAngle = 30f;

    [Header("Juice Events")]
    public UnityEvent onHitJuice;
    public UnityEvent onDeathJuice;
    public UnityEvent onShootJuice;

    [Header("Juice Settings (Visuals)")]
    public GameObject bloodSplashPrefab;
    public float bloodRotationOffset = 90f;

    private Transform player;
    private Rigidbody2D rb;
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + dir * moveSpeed * Time.fixedDeltaTime);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void Update()
    {
        if (player == null) return;

        if (Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        if (enemyProjectilePrefab != null && firePoint != null)
        {
            float startAngle = projectilesPerShot > 1 ? -spreadAngle / 2f : 0f;
            float angleStep = projectilesPerShot > 1 ? spreadAngle / (projectilesPerShot - 1) : 0f;

            for (int i = 0; i < projectilesPerShot; i++)
            {
                float currentAngle = startAngle + (angleStep * i);

                Quaternion spreadRotation = Quaternion.Euler(0, 0, currentAngle);
                Quaternion finalRotation = firePoint.rotation * spreadRotation;

                GameObject proj = Instantiate(enemyProjectilePrefab, firePoint.position, finalRotation);
                Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();

                if (projRb != null)
                {
                    projRb.linearVelocity = finalRotation * Vector3.up * projectileSpeed;
                }
            }

            onShootJuice?.Invoke();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (AudioManager.Instance != null) AudioManager.Instance.PlayEnemyHit();
        onHitJuice?.Invoke();

        if (health <= 0) Die();
    }

    void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
            GameManager.Instance.TriggerHitStop();
        }

        if (CameraController.Instance != null) CameraController.Instance.TriggerDeathShake();
        if (AudioManager.Instance != null) AudioManager.Instance.PlayEnemyDie();

        onDeathJuice?.Invoke();

        if (bloodSplashPrefab != null)
        {
            Quaternion splashRotation = transform.rotation * Quaternion.Euler(0f, bloodRotationOffset, 0f);
            Instantiate(bloodSplashPrefab, transform.position, splashRotation);
        }

        Destroy(gameObject);
    }
}