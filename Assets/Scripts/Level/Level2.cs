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

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                AIVoice.clip = AIVoice1;
                AIVoice.Play();
                currentState = GameState.ShootWithLimitedBullet;
                break;
            case GameState.ShootWithLimitedBullet:
                enemySpawner.isSpawnEnemy = true;
                if (playerController.currentBullet <= 0)
                {
                    enemySpawner.isSpawnEnemy = false;
                    currentState = GameState.ShootWithLimitedBullet;
                }

                if (playerController.PlayerKillEnemyNum >= 10)
                {
                    currentState = GameState.NewEnemy;
                }
                break;
            case GameState.NewEnemy:
                enemySpawner.enemyPrefab[1] = enemySpawner.enemyPrefabs[1];
                StartCoroutine(enemySpawner.SpawnEnemies1());
                if(playerController.PlayerKillEnemyNum>=20)
                {
                    currentState = GameState.SuperEnemyWave;
                }
                break;
            case GameState.SuperEnemyWave:
                enemySpawner.enemyType0CountMax *= 2;
                enemySpawner.enemyType1CountMax *= 2;
                if(playerController.PlayerKillEnemyNum>=40)
                {
                    currentState = GameState.Win;
                    GameManager._instance.UpdateGameState(GameManager.GameState.Win);
                }
                break;
        }
    }
}