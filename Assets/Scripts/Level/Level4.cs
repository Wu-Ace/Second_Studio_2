using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level4 : MonoBehaviour
{
    // Start is called before the first frame update
    private enum GameState
    {
        Start,
        NormalWave,
        Win,
        Lose,
    }
    private GameState    currentState;
    public ConeCollider coneCollider;
    void Start()
    {
        currentState                   = GameState.Start;
        enemySpawner                   = GetComponent<EnemySpawner>();
        playerController               = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        coneCollider                  = GameObject.FindWithTag("Player").GetComponent<ConeCollider>();
        coneCollider.isLevel4 = true;
        playerController.currentBullet = 7;
        playerController.bulletMag = 7;
        playerController.Health = 5;
        // StartCoroutine(SpawnNormalEnemy());
    }
    public GameObject LoseText;
    public AudioSource AIVoice;
    public AudioClip AIVoice_Start;
    public AudioClip AIVoice_NewEnemyWave;
    public AudioClip AIVoice_SuperEnemyWave;
    public AudioClip AIVoice_Final;
    public bool AIVoiceHasPlayed = false;
    public bool hasStartedSpawning = false;

    private float superWaveStartTime;

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GameState.Start:
                if(!AIVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoice_Start);
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
                    StartCoroutine(SpawnNormalEnemy());
                    hasStartedSpawning = true;
                }
                Debug.Log(playerController.killCount["NormalEnemy"]);
                if (playerController.killCount.ContainsKey("NormalEnemy")&&playerController.killCount["NormalEnemy"]>=10)
                {
                    hasStartedSpawning = false;
                    AIVoiceHasPlayed = false;
                    // currentState = GameState.NewEnemyWave;
                }
                if(playerController.Health <= 0)
                {
                    currentState = GameState.Lose;
                }

                break;

            case GameState.Win:
                if(!AIVoiceHasPlayed)
                {
                    AIVoice.PlayOneShot(AIVoice_Final);
                    AIVoiceHasPlayed = true;
                }
                if(AIVoice.isPlaying == false)
                {
                    GameManager._instance.gameState = GameManager.GameState.Win;
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
            yield return new WaitForSeconds(Random.Range(4, 7)); // Wait for a random time between 2 and 5 seconds
        }
    }
    public GameObject ShootToMoveEnemy;
    private IEnumerator SpawnShootToMoveEnemy()
    {

        while (true)
        {
            enemySpawner.SpawnEnemy(ShootToMoveEnemy, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(5, 12)); // Wait for a random time between 2 and 5 seconds
        }
    }
}