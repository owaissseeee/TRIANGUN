using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject basicEnemyPrefab;
    public GameObject shotgunEnemyPrefab;

    [Header("Difficulty Scaling")]
    public float timeToMaxDifficulty = 180f;

    [Header("Spawn Rates")]
    public float startSpawnRate = 3f;
    public float maxSpawnRate = 0.35f;

    [Header("Shotgun Enemy Odds")]
    public float initialShotgunChance = 0.0f;
    public float maxShotgunChance = 0.4f;

    [Header("Enemy Stat Buffs")]
    public float maxSpeedMultiplier = 1.5f;

    private float nextSpawnTime;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();

            float difficultyPercent = Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);

            float currentSpawnRate = Mathf.Lerp(startSpawnRate, maxSpawnRate, difficultyPercent);

            nextSpawnTime = Time.time + currentSpawnRate;
        }
    }

    void SpawnEnemy()
    {
        float difficultyPercent = Mathf.Clamp01(Time.timeSinceLevelLoad / timeToMaxDifficulty);
        float currentShotgunChance = Mathf.Lerp(initialShotgunChance, maxShotgunChance, difficultyPercent);

        GameObject prefabToSpawn = basicEnemyPrefab;
        if (Random.value < currentShotgunChance)
        {
            prefabToSpawn = shotgunEnemyPrefab;
        }

        Vector2 spawnPosition = GetRandomPointOutsideCamera();
        GameObject spawnedEnemy = Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);

        EnemyController enemyScript = spawnedEnemy.GetComponent<EnemyController>();
        if (enemyScript != null)
        {
            float speedBoost = Mathf.Lerp(1f, maxSpeedMultiplier, difficultyPercent);
            enemyScript.moveSpeed *= speedBoost;
        }
    }

    Vector2 GetRandomPointOutsideCamera()
    {
        int edge = Random.Range(0, 4);
        float spawnMargin = 0.1f;
        Vector3 viewportPoint = Vector3.zero;

        switch (edge)
        {
            case 0: viewportPoint = new Vector3(Random.value, 1f + spawnMargin, Mathf.Abs(cam.transform.position.z)); break;
            case 1: viewportPoint = new Vector3(Random.value, 0f - spawnMargin, Mathf.Abs(cam.transform.position.z)); break;
            case 2: viewportPoint = new Vector3(0f - spawnMargin, Random.value, Mathf.Abs(cam.transform.position.z)); break;
            case 3: viewportPoint = new Vector3(1f + spawnMargin, Random.value, Mathf.Abs(cam.transform.position.z)); break;
        }

        return cam.ViewportToWorldPoint(viewportPoint);
    }
}