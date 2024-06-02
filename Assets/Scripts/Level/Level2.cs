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
    public AudioClip AIVoiceShoot2;
    public AudioClip AIVoiceShoot3;
    public AudioClip AIFinalShootVoice;

    private GameState        currentState;
    private EnemySpawner     enemySpawner;
    public  PlayerController playerController;


    private enum GameState
    {
        Start,
        ShootWithLimitedBullet,
        NewEnemy,
        SuperEnemyWave,
        Win,
        Lose,
    }
    void Start()
    {
        // 游戏开始时，状态设置为LookUp
        currentState = GameState.Start;
        enemySpawner = GetComponent<EnemySpawner>();
        // playerController               = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.currentBullet = 100;

    }

    bool AiVoiceHasPlayed = false;
    bool hasStartedSpawning = false;
    public GameObject StaticEnemy;
    public GameObject NormalEnemy;
    public GameObject HealthText;
    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                if(!AiVoiceHasPlayed)
                {
                    // AIVoice.PlayOneShot(AIVoice1);
                    AiVoiceHasPlayed = true;
                }
                if(!AIVoice.isPlaying)
                {
                    AiVoiceHasPlayed = false;
                    currentState = GameState.ShootWithLimitedBullet;
                }
                break;
            case GameState.ShootWithLimitedBullet:
                playerController.UpdateCurrentBulletText();
                if (!hasStartedSpawning)
                {
                    StartCoroutine(SpawnEnemyAndDelay(StaticEnemy, Random.onUnitSphere * 50));
                    hasStartedSpawning = true;
                }
                if (playerController.currentBullet <= 0)
                {
                    enemySpawner.isSpawnEnemy = false;
                }

                if (playerController.PlayerKillEnemyNum >= 5)
                {
                    hasStartedSpawning = false;
                    currentState = GameState.NewEnemy;
                }
                break;
            case GameState.NewEnemy:
                HealthText.SetActive(true);
                if (!hasStartedSpawning)
                {
                    StartCoroutine(enemySpawner.SpawnNormalEnemy());
                }
                if(playerController.PlayerKillEnemyNum>=20)
                {
                    currentState = GameState.SuperEnemyWave;
                }
                break;
            case GameState.SuperEnemyWave:
                enemySpawner.StaticEnemyCount *= 2;
                enemySpawner.enemyType1CountMax *= 2;
                if(playerController.PlayerKillEnemyNum>=40)
                {
                    currentState = GameState.Win;
                    GameManager._instance.UpdateGameState(GameManager.GameState.Win);
                }
                break;
        }
    }
    private IEnumerator SpawnEnemyAndDelay(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        for(int i = 0 ; i < 5 ; i++)
        {
            spawnPosition = Random.onUnitSphere * 50;
            enemySpawner.SpawnEnemy(enemyPrefab, spawnPosition);
            // hasSpawnEnemy = true;
            yield return new WaitForSeconds(Random.Range(3, 6)); // Wait for a random time between 2 and 5 seconds
        }
        // enemy                      = GameObject.FindWithTag("Enemy");
        // IEnumeratorHasSpawnedEnemy = true;
    }
}