using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public                   static GameManager     _instance;

    public float           CurrentSurvivalTime; // 新增变量记录生存时长
    public float           WinSurvivalTime     = 0; //
    public float           countdownTime;
    public TextMeshProUGUI timerText;
    public GameObject winText;


    public                          bool            isWin               = false;
    [SerializeField] private        AudioClip       playerWin_clip; // 新增变量记录玩家是否获胜

    public  GameObject   heartSpawnerGameObject;
    public  GameObject   enemySpawnerGameObject;
    private HeartSpawner _heartSpawner;
    public  GameObject   playerInformation;


    private void Awake()
    {
        if (_instance !=  null)
        {
            Destroy(this);
        }
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    public enum GameState
    {
        Start,
        ShootingTutorial,
        EnemySpawning,
        Playing,
        Win,
        Lose
    }
    public GameState gameState;

public bool winHasPlayed = false;
    public void UpdateGameState(GameState newGameState)
    {
        gameState = newGameState;
        switch (newGameState)
        {
            case GameState.Start:
                // heartSpawnerGameObject.SetActive(false);
                // enemySpawnerGameObject.SetActive(false);
                // playerInformation.SetActive(false);
                // _heartSpawner.SpawnHeart();
                break;
            case GameState.ShootingTutorial:
                heartSpawnerGameObject.SetActive(true);
                playerInformation.SetActive(true);
                Debug.Log("GameManager:UpdateGameState:ShootingTutorial");
                break;
            case GameState.EnemySpawning:
                heartSpawnerGameObject.SetActive(true);
                enemySpawnerGameObject.SetActive(true);
                enemySpawnerGameObject.GetComponent<EnemySpawner>().isSpawnEnemy=true;
                winText.gameObject.SetActive(false);
                break;
            case GameState.Win:
                isWin = true;
                winText.gameObject.SetActive(true);
                if (!winHasPlayed)
                {
                    SoundManager.instance.PlayPlayerSound(playerWin_clip, 1);
                }
                break;
            case GameState.Lose:
                break;
        }
        // EventManager.instance.GameStateChange(newGameState);
    }
    void Start()
    {
        EventManager.instance.onGameStateChange += UpdateGameState;
        _heartSpawner = heartSpawnerGameObject.GetComponent<HeartSpawner>();
        UpdateGameState(GameState.Start);
        CurrentSurvivalTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameState.Start!=gameState&&GameState.Win!=gameState&&GameState.Lose!=gameState&&GameState.ShootingTutorial!=gameState)
        {
            CurrentSurvivalTime += Time.deltaTime;
        }
        // CurrentSurvivalTime += Time.deltaTime; // 更新生存时长
        CheckIfWin();                          // 检查是否获胜
        UpdateTime();                     // 更新计时器文本

    }
    void CheckIfWin()
    {
        if (CurrentSurvivalTime >= WinSurvivalTime) // 如果生存时长达到或超过1分钟
        {
            isWin = true;              // 玩家获胜
            UpdateGameState(GameState.Win);
            Debug.Log("Player Wins!");// 输出获胜信息
            EventManager.instance.PlayerWin(playerWin_clip,1); // 触发玩家获胜事件
        }
    }

    public float GetCurrentTime()
    {
        return CurrentSurvivalTime;
    }
    public void UpdateTime()
    {
        timerText.text = "Time: " + CurrentSurvivalTime.ToString("0.00");
    }

}