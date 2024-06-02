using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject StaticEnemy;
    public GameObject NormalEnemy;
    public GameObject ShootToMoveEnemy;

    public  float        spawnRadius  ;
    public  float        minSpawnTime ;
    public  float        maxSpawnTime ;
    private float        EnemyMaxNumber;
    public  float        EnemyNumber;
    public  int          EnemyTrueMaxNumber;


    private float nextSpawnTime = 0;

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
    }

    void Update()
    {
        // if (isSpawnEnemy)
        // {
        //     // 如果当前时间大于等于下一次生成时间
        //     if (EnemyNumber == 0)
        //     {
        //         SpawnEnemy(enemyPrefab[0], transform.position + Random.onUnitSphere * spawnRadius);
        //         EnemyNumber++;
        //         StaticEnemyCount++;
        //         nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        //     }
        //     else if (EnemyNumber!=0&& Time.time >= nextSpawnTime && EnemyNumber < EnemyMaxNumber)
        //     {
        //         SpawnEnemy(enemyPrefab[0], transform.position + Random.onUnitSphere * spawnRadius);
        //         EnemyNumber++;
        //         StaticEnemyCount++;
        //         nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        //     }
        // }
    }

    // public void SpawnEnemy()
    // {
    //     Vector3 spawnPosition;
    //     // 随机生成位置
    //     if (isCircle)
    //     {
    //         spawnPosition   = transform.position + Random.onUnitSphere * spawnRadius;
    //         spawnPosition.y = 0;
    //         // 生成敌人
    //     }
    //     else
    //     {
    //         spawnPosition = transform.position + Random.onUnitSphere * spawnRadius;
    //     }
    //     // 生成敌人
    //     Instantiate(enemyPrefab[0], spawnPosition, Quaternion.identity);
    //     EnemyNumber++;
    // }
    // public void SpawnEnemy(Vector3 spawnPosition)
    //     {
    //         Instantiate(enemyPrefab[0], spawnPosition, Quaternion.identity);
    //         EnemyNumber++;
    //     }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        spawnPosition.y = 0;
        EnemyNumber++;
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    public int StaticEnemyCount = 0;
    public int StaticEnemyCountMax = 4;
    public IEnumerator SpawnStaticEnemy()
    {
        while (true)
        {
            if (StaticEnemyCount < StaticEnemyCountMax&&nextSpawnTime<Time.time)
            {
                SpawnEnemy(StaticEnemy, transform.position + Random.onUnitSphere * spawnRadius);
                StaticEnemyCount++;
                nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
            }
            yield return new WaitForSeconds(Random.Range(2,5));
        }
    }
    public int enemyType1Count = 0;
    public int enemyType1CountMax = 1;
    public IEnumerator SpawnNormalEnemy()
    {
        while (true)
        {
            if (enemyType1Count < enemyType1CountMax)
            {
                SpawnEnemy(NormalEnemy, transform.position + Random.onUnitSphere * spawnRadius);
                enemyType1Count++;
            }
            yield return new WaitForSeconds (Random.Range(6,10));
        }
    }
}