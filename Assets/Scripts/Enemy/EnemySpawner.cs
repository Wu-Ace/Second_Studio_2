using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public  GameObject enemyPrefab;
    public  float      spawnRadius  ;
    public  float      minSpawnTime ;
    public  float      maxSpawnTime ;
    private float      EnemyMaxNumber;
    public  float      EnemyNumber;
    public  int        EnemyTrueMaxNumber;


    private float nextSpawnTime;

    [SerializeField] private bool isCircle;

    private PlayerController PlayerController;
    public int              PlayerKillEnemyNum;
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
        // Debug.Log(EnemyMaxNumber);
        if (PlayerKillEnemyNum<2)
        {
            EnemyMaxNumber = 1;
        }
        else if (PlayerKillEnemyNum<4)
        {
            EnemyMaxNumber = 2;
        }
        else
        {
            EnemyMaxNumber = EnemyTrueMaxNumber;
        }

        // 如果当前时间大于等于下一次生成时间
        if(EnemyNumber==0)
        {
            SpawnEnemy();
            EnemyNumber++;
            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
        else if (Time.time >= nextSpawnTime&&EnemyNumber<EnemyMaxNumber)
        {
            // 生成敌人
            SpawnEnemy();
            EnemyNumber++;
            Debug.Log(EnemyNumber);
            // 计算下一次生成时间
            nextSpawnTime = Time.time + Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    void SpawnEnemy()
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
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
}