using UnityEngine;

public class NotAutoEnemySpawner : MonoBehaviour
{
    public  GameObject[] enemyPrefabs;
    private  GameObject   enemyPrefab;
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
        enemyPrefab   = enemyPrefabs[0];
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
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }
    public void SpawnEnemy(Vector3 spawnPosition)
        {
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }

    public void SpawnEnemy(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

}