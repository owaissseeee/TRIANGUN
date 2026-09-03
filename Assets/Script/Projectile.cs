using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 2f;
    public int damage = 1;
    public bool isEnemyProjectile = false; 

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isEnemyProjectile)
        {
            
            if (collision.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    Destroy(gameObject);
                }
            }
        }
        else
        {
           
            if (collision.CompareTag("Enemy"))
            {
                EnemyController enemy = collision.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                    Destroy(gameObject); 
                }
            }
        }
    }
}