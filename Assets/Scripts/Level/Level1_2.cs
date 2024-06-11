using System.Collections;
using System.Collections.Generic;
using Manager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Lofelt.NiceVibrations;


public class Level1_2 : MonoBehaviour
{
    // 定义一个枚举类型来表示游戏的状态
    private enum GameState
    {
        Start,
        Shoot1,
        ShootBackward,
        ShootAFew,
        Reload,
        FinalShoot,
        TurnBack,
    }
    public GameObject enemyPrefab;

    // 当前的游戏状态
    private GameState        currentState;
    public  ConeCollider     coneCollider;
    private EnemySpawner     enemySpawner;
    public  PlayerController playerController;
    public  GameObject       Player;
    public  float            spawnRadius = 10f;

    public                   Transform  mainCamera;

    [Header("Sound")]
    public AudioSource AIVoice;
    public AudioSource BackgroundMusicSource;
    public AudioClip   BackgroundMusicClip;
    public AudioClip AIVoice1_PhoneFront;
    public AudioClip AIVoice2_Shoot;
    public AudioClip AIVoice2_Shoot2;
    public AudioClip AIVoice3_ShootBackward;
    public AudioClip AIVoice_ShootAFew;
    public AudioClip AIVoice4_Reload;
    public AudioClip AIVoice_FinalShoot;



    // Start is called before the first frame update
    void Start()
    {
        // 游戏开始时，状态设置为LookUp
        // currentState = GameState.Shoot1;
        if (BackgroundMusic.Instance != null)
        {
            Destroy(BackgroundMusic.Instance.gameObject);
        }
        BackgroundMusicSource.loop   = true;
        BackgroundMusicSource.clip   = BackgroundMusicClip;
        BackgroundMusicSource.volume = 0.02f;
        AIVoice.volume               = 1f;
        BackgroundMusicSource.Play();
        playerController.Health            = 10;
        playerController.needSuccessiveHit = false;
        enemySpawner                       = GetComponent<EnemySpawner>();
        // playerController               = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        playerController.currentBullet = 100;
        playerController.bulletMag= 100;
        AIVoice.PlayOneShot(AIVoice1_PhoneFront);
        Debug.Log("1");
        hasRunOnce   = false;
        hasRunOnce2  = false;

        coneCollider.isLevel4 = true;
        playerController.isLevel1_2 = true;
        currentState = GameState.Start;
        // currentState = GameState.ShootAFew;
        // EventManager.instance.onPlayerShoot           += HandlePlayerShoot;

    }

    private float      holdEvenlyTimer           = 0;
    private bool       hasRunOnce                = false;
    private bool       hasRunOnce2               = false;
    private bool       hasSpawnEnemy             = false;
    private float      accumulatedRotationChange = 0;
    private float      previousYRotation         = 0;
    private GameObject enemy;
    private bool IEnumeratorHasSpawnedEnemy = false;

    private float old_y = 0;
    private float new_y;
    private float currentDistance = 0;

    public  GameObject BulletText;
    private int        Bullet;
    private float      finalShootStartTime = 0;
    private float      timeInFinalShoot;
    public  GameObject WinText;
    void Update()
    {
        // Debug.Log("enemy"+(enemy!=null));

        // 根据当前的状态执行相应的操作
        switch (currentState)
        {
            case GameState.Start:
                Debug.Log("Start");
                playerController.isLevel1_2 = true;
                if (AIVoice.isPlaying == false && mainCamera.localRotation.eulerAngles.x >= 70 &&
                    mainCamera.localRotation.eulerAngles.x <= 100)
                {
                    // 当手机端平时，开始计时
                    holdEvenlyTimer += Time.deltaTime;
                    if (AIVoice.isPlaying==false && holdEvenlyTimer >= 1)
                    {
                        HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                        Debug.Log("HoldThePhoneEvenly");
                        holdEvenlyTimer = 0; // 重置计时器
                        hasRunOnce      = false;
                        currentState    = GameState.Shoot1;
                    }
                }

                break;
            case GameState.Shoot1:
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoice2_Shoot);
                    hasRunOnce = true;
                    holdEvenlyTimer+=Time.deltaTime;
                    if(holdEvenlyTimer>=2&&hasRunOnce2==false)
                    {
                        AIVoice.clip = AIVoice2_Shoot2;//快快快
                        AIVoice.Play();
                        hasRunOnce2 = true;
                    }
                }

                if (!hasSpawnEnemy)
                {
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, Player.transform.position + Player.gameObject. transform.forward * 100));
                }
                if (AIVoice.isPlaying ==false &&!hasSpawnEnemy)
                {
                    Debug.Log("Shoot1");
                    hasSpawnEnemy = true;
                }
                Debug.Log("enemy"+(enemy!=null));

                if (enemy == null && IEnumeratorHasSpawnedEnemy)
                {
                    HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                    Debug.Log("enemy");
                    hasRunOnce      = false;
                    hasSpawnEnemy   = false;
                    IEnumeratorHasSpawnedEnemy = false;
                    currentState    = GameState.ShootBackward;
                }
                break;
            case GameState.ShootBackward:
                if(!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoice3_ShootBackward);
                    hasRunOnce = true;
                }
                if (AIVoice.isPlaying==false&& !hasSpawnEnemy)
                {
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, Player.transform.position-Player.transform.forward*50));
                    hasSpawnEnemy = true;
                }
                if (enemy == null && IEnumeratorHasSpawnedEnemy)
                {
                    HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                    Debug.Log("enemy");
                    hasRunOnce                     = false;
                    hasSpawnEnemy                  = false;
                    IEnumeratorHasSpawnedEnemy     = false;
                    playerController.currentBullet = 2;
                    Bullet                         = 5;
                    currentState                   = GameState.ShootAFew;
                }
                break;
            case GameState.ShootAFew:
                if(!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoice_ShootAFew);
                    playerController.killCount["StaticEnemy"] = 0;
                    hasRunOnce = true;
                }

                if (AIVoice.isPlaying == false && !hasSpawnEnemy)
                {
                    // Debug.Log("1");
                    StartCoroutine( SpawnEnemyAndDelay(enemyPrefab, Player.transform.position - Player.transform.forward * 50+Player.transform.right*50));
                    enemy = GameObject.FindWithTag("Enemy");
                    hasSpawnEnemy = true;
                }
                Debug.Log(playerController.currentBullet);
                if (enemy == null && playerController.currentBullet>0)
                {
                    StartCoroutine(SpawnEnemyAndDelay(enemyPrefab, Random.onUnitSphere * 50));
                    enemy = GameObject.FindWithTag("Enemy");
                }

                if (playerController.currentBullet<=0)
                {
                    HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                    hasRunOnce    = false;
                    hasSpawnEnemy = false;
                    playerController.isLevel1_2 = false;
                    currentState  = GameState.Reload;
                }




                break;
            case GameState.Reload:
                new_y           = Input.acceleration.y;
                currentDistance = new_y - old_y;
                old_y           = new_y;
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoice4_Reload);
                    hasRunOnce = true;
                }
                if(AIVoice.isPlaying==false)
                {
                    playerController.isLevel1_2 = false;
                }
                if (AIVoice.isPlaying==false&&currentDistance > 0.5f && Input.deviceOrientation == DeviceOrientation.Portrait)
                {
                    // BulletText.SetActive(true);
                    HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                    hasRunOnce   = false;
                    currentState = GameState.FinalShoot;
                }
                break;
            case GameState.FinalShoot:
                // Timer += Time.deltaTime;
                if (!hasRunOnce)
                {
                    AIVoice.PlayOneShot(AIVoice_FinalShoot);
                    finalShootStartTime = Time.time;
                    hasRunOnce          = true;
                }
                if (!hasSpawnEnemy)
                {
                    StartCoroutine(SpawnStaticEnemy());
                    hasSpawnEnemy = true;
                }

                if (AIVoice.isPlaying == false)
                {
                    timeInFinalShoot = Time.time - finalShootStartTime;
                }

                // Debug.Log(timeInFinalShoot>1);
                if (timeInFinalShoot > 60)
                {
                    HapticPatterns.PlayConstant(1.0f, 0.0f, 1.0f);
                    Debug.Log("1");
                    GameManager._instance.gameState = GameManager.GameState.Win;
                    WinText.SetActive(true);
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
            enemy = GameObject.FindWithTag("Enemy");
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
    private IEnumerator SpawnStaticEnemy()
    {

        while (playerController.successiveHitNum < 7)
        {
            enemySpawner.SpawnEnemy(enemyPrefab, Random.onUnitSphere * 50);
            yield return new WaitForSeconds(Random.Range(5, 7)); // Wait for a random time between 2 and 5 seconds
        }
    }

}