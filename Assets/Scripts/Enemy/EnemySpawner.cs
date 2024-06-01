using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public  GameObject[] enemyPrefabs = new GameObject[4];
    public  GameObject[]   enemyPrefab = new GameObject[4];
    public  float        spawnRadius  ;
    public  float        minSpawnTime ;
    public  float        maxSpawnTime ;
    private float        EnemyMaxNumber;
    public  float        EnemyNumber;
    public  int          EnemyTrueMaxNumber;


    private float nextSpawnTime;

    [SerializeField] private bool isCircle;

    private PlayerController PlayerController;
    public int              PlayerKillEnemyNum;
    public bool isSpawnEnemy;
    private void Awake()
    {
        PlayerController   = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        PlayerKillEnemyNum = PlayerController.PlayerKillEnemyNum;
    }
    void Start()
    {
        // 初始化下一次生成时间
        nextSpawnTime  = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        EnemyNumber    = 0;
        EnemyMaxNumber = 1;
        enemyPrefab[0]   = enemyPrefabs[0];
    }

    void Update()
    {
        if (isSpawnEnemy)
        {
            // 如果当前时间大于等于下一次生成时间
            if (EnemyNumber == 0)
            {
                SpawnEnemy(enemyPrefab[0], transform.position + Random.onUnitSphere * spawnRadius);
                EnemyNumber++;
                enemyType0Count++;
                nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
            }
            else if (Time.time >= nextSpawnTime && EnemyNumber < EnemyMaxNumber)
            {
                // 生成敌人
                SpawnEnemy(enemyPrefab[0], transform.position + Random.onUnitSphere * spawnRadius);
                EnemyNumber++;
                enemyType0Count++;
                // 计算下一次生成时间
                nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
            }
        }
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPosition;
        // 随机生成位置
        if (isCircle)
        {
            spawnPosition   = transform.position + Random.onUnitSphere * spawnRadius;
            spawnPosition.y = 0;
            // 生成敌人
        }
        else
        {
            spawnPosition = transform.position + Random.onUnitSphere * spawnRadius;
        }
        // 生成敌人
        Instantiate(enemyPrefab[0], spawnPosition, Quaternion.identity);
        EnemyNumber++;
    }
    public void SpawnEnemy(Vector3 spawnPosition)
        {
            Instantiate(enemyPrefab[0], spawnPosition, Quaternion.identity);
            EnemyNumber++;
        }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        EnemyNumber++;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public int enemyType0Count = 0;
    public int enemyType0CountMax = 4;
    public IEnumerator SpawnEnemies0()
    {
        while (true)
        {
            if (enemyType0Count < enemyType0CountMax)
            {
                SpawnEnemy(enemyPrefab[0], transform.position + Random.onUnitSphere * spawnRadius);
                enemyType0Count++;
            }
            yield return new WaitForSeconds(Random.Range(2,5));
        }
    }
    public int enemyType1Count = 0;
    public int enemyType1CountMax = 1;
    public IEnumerator SpawnEnemies1()
    {
        while (true)
        {
            if (enemyType1Count < enemyType1CountMax)
            {
                SpawnEnemy(enemyPrefab[1], transform.position + Random.onUnitSphere * spawnRadius);
                enemyType1Count++;
            }
            yield return new WaitForSeconds (Random.Range(6,10));
        }
    }
}