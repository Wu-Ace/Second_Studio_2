using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Level1 : MonoBehaviour
{
    // 定义一个枚举类型来表示游戏的状态
    private enum GameState
    {
        Start,
        HoldThePhoneEvenly,
        TurnBack,
        Shoot1,
        Shoot2,
        Shoot3,
        FinalShoot,
    }
    public GameObject enemyPrefab;

    // 当前的游戏状态
    private GameState    currentState;
    private EnemySpawner enemySpawner;
    public PlayerController playerController;
    public  float        spawnRadius = 10f;

    public                   Transform  mainCamera;

    [Header("Sound")]
    public AudioSource AIVoice;
    public                                 AudioClip AIVoice1;
    public                                 AudioClip AIVoice2;
    public                                 AudioClip AIVoice3;
    public                                 AudioClip AIVoiceShoot1;
    public                                 AudioClip AIVoiceShoot2;
    public AudioClip AIVoiceShoot3;
    public AudioClip AIFinalShootVoice;


    // Start is called before the first frame update
    void Start()
    {
        // 游戏开始时，状态设置为LookUp
        currentState                   = GameState.Start;
        enemySpawner                   = GetComponent<EnemySpawner>();
        // playerController               = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.currentBullet = 100;
        // AIVoice.PlayOneShot(AIVoice1);
    }

    private float      holdEvenlyTimer           = 0;
    private bool       hasRunOnce                = false;
    private bool       hasSpawnEnemy             = false;
    private float      accumulatedRotationChange = 0;
    private float      previousYRotation         = 0;
    private GameObject enemy;
    private bool IEnumeratorHasSpawnedEnemy = false;

    void Update()
    {
        // Debug.Log("enemy"+(enemy!=null));

        // 根据当前的状态执行相应的操作
        switch (currentState)
        {
            case GameState.Start:
                if (!AIVoice.isPlaying)
                {
                    currentState = GameState.HoldThePhoneEvenly;
                    // currentState = GameState.FinalShoot;
                }
                break;

            case GameState.HoldThePhoneEvenly:
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoice2);
                    hasRunOnce = true;
                }
                if (mainCamera.localRotation.eulerAngles.x >= 70 && mainCamera.localRotation.eulerAngles.x <= 100)
                {
                    // 当手机端平时，开始计时
                    holdEvenlyTimer += Time.deltaTime;
                    Debug.Log("holdEvenlyTimer: " + holdEvenlyTimer);

                    // 当计时器的值大于等于3秒时，切换到下一个状态
                    if (holdEvenlyTimer >= 3)
                    {
                        Debug.Log("HoldThePhoneEvenly");
                        previousYRotation = mainCamera.localRotation.eulerAngles.y;
                        currentState = GameState.TurnBack;
                        hasRunOnce = false;
                        holdEvenlyTimer = 0; // 重置计时器
                    }
                }
                else
                {
                    holdEvenlyTimer = 0; // 如果手机不再端平，重置计时器
                }
                break;

            case GameState.TurnBack:
                // if (!hasRunOnce)
                // {
                //     AIVoice.PlayOneShot(AIVoice3);
                //     hasRunOnce = true;
                //     accumulatedRotationChange = 0; // 重置累计旋转角度
                // }

                // 获取当前的y轴旋转角度
                float currentYRotation = mainCamera.localRotation.eulerAngles.y;

                // 计算当前的y轴旋转角度和上一帧的y轴旋转角度的差值
                float rotationChangeThisFrame = currentYRotation - previousYRotation;

                // 处理角度从360度到0度的情况
                if (rotationChangeThisFrame < -180)
                {
                    rotationChangeThisFrame += 360;
                }
                else if (rotationChangeThisFrame > 180)
                {
                    rotationChangeThisFrame -= 360;
                }

                // 累积每一帧的旋转变化
                accumulatedRotationChange += rotationChangeThisFrame;

                // 更新previousYRotation为当前的y轴旋转角度，以便下一帧计算旋转变化
                previousYRotation = currentYRotation;
                Debug.Log("Accumulated Rotation: " + accumulatedRotationChange);
                currentState = GameState.Shoot1;
                // 如果累积的旋转变化大于等于360度，表示玩家旋转了一圈
                if (Mathf.Abs(accumulatedRotationChange) >= 360)
                {
                    Debug.Log("Player has spun around");
                    hasRunOnce = false;

                    currentState = GameState.Shoot1;
                    // 这里可以添加你希望在玩家旋转一圈后执行的代码
                    // ...
                    accumulatedRotationChange = 0; // 重置累计旋转角度
                }
                break;
            case GameState.Shoot1:
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoiceShoot1);
                    hasRunOnce                = true;
                }
                if(!hasSpawnEnemy)
                {
                    Debug.Log("Shoot1");
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, mainCamera.gameObject.transform.position - mainCamera.gameObject.transform.right * 10));
                }
                Debug.Log("enemy"+(enemy!=null));

                if (enemy == null && IEnumeratorHasSpawnedEnemy)
                {
                    Debug.Log("enemy");
                    hasRunOnce      = false;
                    hasSpawnEnemy   = false;
                    IEnumeratorHasSpawnedEnemy = false;
                    currentState    = GameState.Shoot2;
                }
                break;
            case GameState.Shoot2:
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoiceShoot2);
                    hasRunOnce = true;
                }
                if (!hasSpawnEnemy)
                {
                    Debug.Log("Shoot2");
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, mainCamera.gameObject.transform.position + mainCamera.gameObject.transform.right * 10));
                }
                if(enemy == null&&IEnumeratorHasSpawnedEnemy)
                {
                    hasRunOnce = false;
                    hasSpawnEnemy = false;
                    IEnumeratorHasSpawnedEnemy = false;
                    currentState = GameState.Shoot3;
                }
                break;
            case GameState.Shoot3:
                Debug.Log("Shoot3");
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoiceShoot3);
                    hasRunOnce = true;
                }
                if (!hasSpawnEnemy)
                {
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, mainCamera.gameObject.transform.position - mainCamera.gameObject.transform.up * 10));

                }

                if (enemy == null && IEnumeratorHasSpawnedEnemy)
                {
                    hasRunOnce                 = false;
                    hasSpawnEnemy              = false;
                    IEnumeratorHasSpawnedEnemy = false;
                    enemySpawner.isSpawnEnemy           = true;
                    // playerController.PlayerKillEnemyNum = 0;
                    if (!hasRunOnce)
                    {
                        AIVoice.PlayOneShot(AIFinalShootVoice);
                        hasRunOnce = true;
                    }
                    currentState = GameState.FinalShoot;

                }


                break;
            case GameState.FinalShoot:
                playerController.currentBullet = 100;
                if (!hasSpawnEnemy)
                {
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, mainCamera.gameObject.transform.position - mainCamera.gameObject.transform.up * 10));
                    hasSpawnEnemy = true;
                }
                if (enemy == null && IEnumeratorHasSpawnedEnemy)
                {
                    hasRunOnce                 = false;
                    hasSpawnEnemy              = false;
                    IEnumeratorHasSpawnedEnemy = false;
                    enemySpawner.isSpawnEnemy  = true;
                    // playerController.PlayerKillEnemyNum = 0;
                }
                if (playerController.PlayerKillEnemyNum == 7)
                {
                    GameManager._instance.UpdateGameState(GameManager.GameState.Win);
                }
                break;
        }

    }
    public void SpawnEnemiesAtRandomPositions(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector3 spawnPosition = transform.position + Random.onUnitSphere * spawnRadius;
            spawnPosition.y = 0;
            enemySpawner.SpawnEnemy(enemyPrefab, spawnPosition);
        }
    }
    private IEnumerator SpawnEnemyAndDelay(GameObject enemyPrefab, Vector3 spawnPosition)
    {
        enemySpawner.SpawnEnemy(enemyPrefab, spawnPosition);
        hasSpawnEnemy = true;
        yield return new WaitForSeconds(Random.Range(2,5)); // Wait for 0.1 seconds
        enemy         = GameObject.FindWithTag("Enemy");
        IEnumeratorHasSpawnedEnemy = true;
        // Debug.Log("enemy"+(enemy!=null));
    }
}