using UnityEngine;

public class HeartSpawner : MonoBehaviour
{
    public GameObject heartPrefab;
    public float      spawnRadius;
    public float      minSpawnTime;
    public float      maxSpawnTime;
    public int        HeartMaxNumber;

    private float nextSpawnTime;
    private int   HeartNumber;

    void Start()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        HeartNumber   = 0;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && HeartNumber < HeartMaxNumber)
        {
            SpawnHeart();
            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    public void SpawnHeart()
    {
        Vector3 spawnPosition = transform.position + Random.onUnitSphere * spawnRadius;
        spawnPosition.y = 0;
        Instantiate(heartPrefab, spawnPosition, Quaternion.identity);
        HeartNumber++;
    }
}