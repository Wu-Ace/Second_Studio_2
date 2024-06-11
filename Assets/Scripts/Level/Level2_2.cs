using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level3 : MonoBehaviour
{
    // Start is called before the first frame update
    private enum GameState
    {
        Start,
        NormalWave,
        NewEnemyWave,
        SuperEnemyWave,
        Win,
        Lose,
    }
    private GameState    currentState;
    public  ConeCollider coneCollider;
    private Coroutine    normalEnemyCoroutine;
    public  AudioSource  BackgroundMusicSource;
    public  AudioClip    BackgroundMusicClip;
    void Start()
    {
        if (BackgroundMusic.Instance != null)
        {
            Destroy(BackgroundMusic.Instance.gameObject);
        }
        BackgroundMusicSource.loop         = true;
        BackgroundMusicSource.clip         = BackgroundMusicClip;
        BackgroundMusicSource.volume       = 0.02f;
        currentState                       = GameState.Start;
        enemySpawner                       = GetComponent<EnemySpawner>();
        playerController                   = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.currentBullet     = 7;
        playerController.bulletMag         = 200;
        playerController.Health            = 10;
        coneCollider.isLevel4              = false;
        playerController.needSuccessiveHit = false;
        if (BackgroundMusic.Instance != null)
        {
            Destroy(BackgroundMusic.Instance.gameObject);
        }
        // StartCoroutine(SpawnNormalEnemy());
    }
    public GameObject  LoseText;
    public GameObject  WinText;
    public AudioSource AIVoice;
    public AudioClip   AIVoice_Destroy10NormalEnemy;
    public AudioClip   AIVoice_NewEnemyWave;
    public AudioClip   AIVoice_SuperEnemyWave;
    public AudioClip   AIVoice_Final;
    public bool        AIVoiceHasPlayed   = false;
    public bool        hasStartedSpawning = false;

    private float superWaveStartTime;

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                if(!AIVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoice_Destroy10NormalEnemy);
                    AIVoiceHasPlayed = true;
                }
                if(AIVoice.isPlaying == false)
                {
                    AIVoiceHasPlayed = false;
                    currentState = GameState.NormalWave;
                }

                break;
            case GameState.NormalWave:
                if (!hasStartedSpawning)
                {
                    normalEnemyCoroutine = StartCoroutine(SpawnNormalEnemy());
                    hasStartedSpawning = true;
                }
                Debug.Log(playerController.killCount["NormalEnemy"]);
                if (playerController.killCount.ContainsKey("NormalEnemy")&&playerController.killCount["NormalEnemy"]>=10)
                {
                    hasStartedSpawning    = false;
                    AIVoiceHasPlayed      = false;

                    currentState = GameState.NewEnemyWave;
                }
                if(playerController.Health <= 0)
                {
                    StopCoroutine(normalEnemyCoroutine);
                    currentState = GameState.Lose;
                }

                break;
            case GameState.NewEnemyWave:
                if (!AIVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoice_NewEnemyWave);
                    AIVoiceHasPlayed = true;
                }
                if (AIVoice.isPlaying==false &&!hasStartedSpawning)
                {
                    coneCollider.isLevel4 = false;
                    normalEnemyCoroutine  = StartCoroutine(SpawnNormalEnemy());
                    StartCoroutine(SpawnShootToMoveEnemy());
                    hasStartedSpawning = true;
                }
                if (playerController.killCount.ContainsKey("ShootToMoveEnemy") && playerController.killCount["ShootToMoveEnemy"] >= 5&&playerController.killCount.ContainsKey("NormalEnemy")&&playerController.killCount["NormalEnemy"]>=10)
                {
                    hasStartedSpawning = false;
                    AIVoiceHasPlayed = false;
                    currentState = GameState.SuperEnemyWave;
                }
                if(playerController.Health <= 0)
                {
                    currentState = GameState.Lose;
                }
                break;
            case GameState.SuperEnemyWave:
                if (!AIVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoice_SuperEnemyWave);
                    superWaveStartTime = Time.time; // 记录SuperEnemyWave状态开始的时间
                    AIVoiceHasPlayed = true;
                }
                if (!hasStartedSpawning)
                {
                    // StartCoroutine(SlowSpawnNormalEnemy());
                    StartCoroutine(SlowSpawnShootToMoveEnemy());
                    hasStartedSpawning = true;
                }
                if (Time.time - superWaveStartTime >= 45) // 如果SuperEnemyWave状态已经持续了45秒
                {
                    hasStartedSpawning = false;
                    AIVoiceHasPlayed = false;
                    currentState = GameState.Win; // 进入Win状态
                }
                if(playerController.Health <= 0)
                {
                    currentState = GameState.Lose;
                }

                break;
            case GameState.Win:
                if(!AIVoiceHasPlayed)
                {
                    // AIVoice.PlayOneShot(AIVoice_Final);
                    AIVoiceHasPlayed = true;
                }
                if(AIVoice.isPlaying == false)
                {
                    GameManager._instance.gameState = GameManager.GameState.Win;
                    WinText.SetActive(true);
                }
                break;
            case GameState.Lose:
                LoseText.SetActive(true);
                break;
        }
    }
    public PlayerController playerController;
    public EnemySpawner enemySpawner;
    public GameObject NormalEnemy;
    private IEnumerator SpawnNormalEnemy()
    {

        while (true)
        {
            enemySpawner.SpawnEnemy(NormalEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(6,12 )); // Wait for a random time between 2 and 5 seconds
        }
    }
    private IEnumerator SlowSpawnNormalEnemy()
    {

        while (true)
        {
            enemySpawner.SpawnEnemy(NormalEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(10,20 )); // Wait for a random time between 2 and 5 seconds
        }
    }
    public GameObject ShootToMoveEnemy;
    private IEnumerator SpawnShootToMoveEnemy()
    {

        while (true)
        {
            enemySpawner.SpawnEnemy(ShootToMoveEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(7, 20)); // Wait for a random time between 2 and 5 seconds
        }
    }
    private IEnumerator SlowSpawnShootToMoveEnemy()
    {

        while (true)
        {
            enemySpawner.SpawnEnemy(ShootToMoveEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(7, 20)); // Wait for a random time between 2 and 5 seconds
        }
    }
}