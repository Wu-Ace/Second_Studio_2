using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2 : MonoBehaviour
{
    [Header("Sound")]
    public AudioSource AIVoice;
    public AudioClip AIVoice1;
    public AudioClip AIVoice2;
    public AudioClip AIVoice3;
    public AudioClip AIVoiceShoot1;
    public AudioClip AIVoiceLimitedBullet;
    public AudioClip AIVoiceSuccesiveHit;
    public AudioClip AIVoiceShoot2;
    public AudioClip AIVoiceShoot3;
    public AudioClip AIFinalShootVoice;

    private GameState        currentState;
    private EnemySpawner     enemySpawner;
    public  PlayerController playerController;
    public  GameObject       Player;
    public  GameObject       SuccessiveHitNum;
    public  AudioSource      BackgroundMusicSource;
    public  AudioClip        BackgroundMusicClip;


    private enum GameState
    {
        Start,
        LimitedBullet,
        SuccessiveShoot,
        Survive1Minute,
        NewEnemy,
        SuperEnemyWave,
        Win,
        Lose,
    }
    void Start()
    {
        if (BackgroundMusic.Instance != null)
        {
            Destroy(BackgroundMusic.Instance.gameObject);
        }
        BackgroundMusicSource.loop   = true;
        BackgroundMusicSource.clip   = BackgroundMusicClip;
        BackgroundMusicSource.volume = 0.02f;
        // 游戏开始时，状态设置为LookUp
        currentState = GameState.Start;
        // currentState = GameState.SuperEnemyWave;
        enemySpawner = GetComponent<EnemySpawner>();
        // playerController               = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.currentBullet = 7;
        playerController.bulletMag = 7;
        playerController.Health = 10;
        playerController.needSuccessiveHit = false;
        LoseText.SetActive(false);
        SuccessiveHitNum.SetActive(false);
        playerController.isLevel1_2 = false;
        if (BackgroundMusic.Instance != null)
        {
            Destroy(BackgroundMusic.Instance.gameObject);
        }

    }

    bool              AiVoiceHasPlayed   = false;
    bool              hasStartedSpawning = false;
    public GameObject StaticEnemy;
    public GameObject NormalEnemy;
    public GameObject HealthText;
    public GameObject BulletText;
    public GameObject LoseText;

    private float      old_y = 0;
    private float      new_y;
    private float      currentDistance = 0;
    public  int        PlayerKillEnemyAfterNewEnemy;
    public  bool       hasSpawnEnemy = false;
    public  GameObject enemy;
    public int KilledEnemy=0;
    private float surviveStartTime;
    public GameObject ShootToMoveEnemy;
    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                BulletText.SetActive(false);
                if(!AiVoiceHasPlayed)
                {
                    AiVoiceHasPlayed = true;
                }
                if(!AIVoice.isPlaying)
                {
                    AiVoiceHasPlayed = false;
                    currentState = GameState.LimitedBullet;
                    // currentState = GameState.NewEnemy;
                }
                break;
            case GameState.LimitedBullet:
                Debug.Log("Limited Bullet");

                playerController.UpdateCurrentBulletText();
                BulletText.SetActive(true);

                if(!AiVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoiceLimitedBullet);
                    AiVoiceHasPlayed = true;
                }
                if (AIVoice.isPlaying == false && !hasSpawnEnemy)
                {
                    Debug.Log("1");
                    StartCoroutine( SpawnEnemyAndDelay(NormalEnemy, Player.transform.position - Player.transform.forward * 50+Player.transform.right*50));
                    enemy         = GameObject.FindWithTag("Enemy");
                    hasSpawnEnemy = true;
                }
                if (hasSpawnEnemy==true&& enemy == null&&KilledEnemy<10 )
                {
                    KilledEnemy++;
                    if (KilledEnemy < 10)
                    {
                        StartCoroutine(SpawnEnemyAndDelay(NormalEnemy, Random.onUnitSphere * 50));
                        enemy = GameObject.FindWithTag("Enemy");
                    }
                }
                if(KilledEnemy>=10)
                {
                    hasSpawnEnemy                  =  false;
                    KilledEnemy                    =  0;
                    AiVoiceHasPlayed               =  false;
                    playerController.currentBullet += 7;
                    playerController.bulletMag     += 7;
                    currentState                   =  GameState.SuccessiveShoot;
                }
                if (playerController.currentBullet <= 0&&playerController.bulletMag<=0)
                {
                    enemySpawner.isSpawnEnemy = false;
                    currentState              = GameState.Lose;
                }

                break;
            case GameState.SuccessiveShoot:
                Debug.Log("Successive Shoot");
                if(!AiVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoiceSuccesiveHit);
                    playerController.successiveHitNum = 0;
                    AiVoiceHasPlayed                  = true;
                }
                if (AIVoice.isPlaying==false &&hasSpawnEnemy==false)
                {
                    StartCoroutine( SpawnEnemyAndDelay(NormalEnemy, Random.onUnitSphere * 50));
                    enemy = GameObject.FindWithTag("Enemy");
                    SuccessiveHitNum.SetActive(true);
                    playerController.needSuccessiveHit = true;
                    hasSpawnEnemy = true;
                }
                if (hasSpawnEnemy==true&& enemy == null&&playerController.successiveHitNum<3 )
                {
                    StartCoroutine(SpawnEnemyAndDelay(NormalEnemy, Random.onUnitSphere * 50));
                    enemy = GameObject.FindWithTag("Enemy");
                }
                if (playerController.successiveHitNum >= 3)
                {
                    AiVoiceHasPlayed = false;
                    hasSpawnEnemy = false;
                    currentState = GameState.Survive1Minute;
                }
                if (playerController.currentBullet <= 0&&playerController.bulletMag<=0)
                {
                    enemySpawner.isSpawnEnemy      =  false;
                    playerController.currentBullet += 7;
                    playerController.bulletMag     += 7;
                    currentState                   =  GameState.Lose;
                }
                break;
            case GameState.Survive1Minute:
                Debug.Log("Survive 1 Minute");
                playerController.UpdateCurrentBulletText();
                if(!AiVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoiceShoot2);
                    playerController.successiveHitNum = 0;
                    AiVoiceHasPlayed                  = true;
                }
                if (AIVoice.isPlaying==false&&!hasStartedSpawning&& enemySpawner.StaticEnemyCount < 2)
                {
                    surviveStartTime = Time.time;
                    StartCoroutine(SpawnEnemyAndDelay(NormalEnemy, Random.onUnitSphere * 50));
                    StartCoroutine(SpawnShootToMoveEnemy());

                    hasStartedSpawning = true;
                }
                if (playerController.currentBullet <= 0&&playerController.bulletMag<=0)
                {
                    enemySpawner.isSpawnEnemy = false;
                    currentState = GameState.Lose;
                }

                if (Time.time - surviveStartTime >= 60) // 如果Survive1Minute状态已经持续了60秒
                {
                    hasStartedSpawning = false;
                    currentState       = GameState.SuperEnemyWave; // 进入NewEnemy状态
                }
                if(playerController.Health<=0)
                {
                    currentState = GameState.Lose;
                }
                break;
            case GameState.SuperEnemyWave:
                Debug.Log("Super Enemy Wave");
                // playerController.Health = 0;
                playerController.UpdateHealthText();
                if(!AiVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIFinalShootVoice);
                    AiVoiceHasPlayed = true;
                }
                enemySpawner.StaticEnemyCount *= 2;
                enemySpawner.enemyType1CountMax *= 2;
                if (!hasStartedSpawning&&AIVoice.isPlaying==false)
                {
                    Debug.Log("Start Spawning");
                    StartCoroutine(SpawnNormalEnemy());
                    hasStartedSpawning = true;
                }
                if(PlayerKillEnemyAfterNewEnemy- playerController.PlayerKillEnemyNum >= 6)
                {
                    hasStartedSpawning = false;
                    AiVoiceHasPlayed = false;
                    currentState = GameState.Win;
                    GameManager._instance.UpdateGameState(GameManager.GameState.Win);
                }
                if (playerController.Health <= 0)
                {
                    LoseText.SetActive(true);
                }
                if (playerController.currentBullet <= 0&&playerController.bulletMag<=0)
                {
                    enemySpawner.isSpawnEnemy = false;
                    currentState              = GameState.Lose;
                }
                break;
            case GameState.Lose:
                LoseText.SetActive(true);
                break;
        }
    }
    private IEnumerator SpawnEnemyAndDelay(GameObject enemyPrefab, Vector3 spawnPosition)
    {

            spawnPosition = Random.onUnitSphere * 50;
            enemySpawner.SpawnEnemy(enemyPrefab, spawnPosition);
            enemy = GameObject.FindWithTag("Enemy");

            // hasSpawnEnemy = true;
            yield return new WaitForSeconds(Random.Range(3, 6)); // Wait for a random time between 2 and 5 seconds

        // enemy                      = GameObject.FindWithTag("Enemy");
        // IEnumeratorHasSpawnedEnemy = true;
    }
    private IEnumerator SpawnStaticEnemy()
    {

        while (playerController.successiveHitNum < 3)
        {
            enemySpawner.SpawnEnemy(StaticEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(5, 7)); // Wait for a random time between 2 and 5 seconds
        }
    }
    private IEnumerator SpawnShootToMoveEnemy()
    {

        while (true)
        {
            enemySpawner.SpawnEnemy(ShootToMoveEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(7, 20)); // Wait for a random time between 2 and 5 seconds
        }
    }

    private IEnumerator SpawnNormalEnemy()
    {
        while (true)
        {
            enemySpawner.SpawnEnemy(enemySpawner.NormalEnemy, Random.onUnitSphere * 50);
                // enemySpawner.enemyType1Count++;
            yield return new WaitForSeconds(Random.Range(7,10));

        }
    }
}