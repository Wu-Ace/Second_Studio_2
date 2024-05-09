using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class UiManager : MonoBehaviour
{
    static UiManager  _instance;
    public GameObject winPanel;
    public GameObject losePanel;

    private void Awake()
    {
        if(_instance != null)
        {
            Destroy(this);
        }
        _instance = this;

        EventManager.instance.onGameStateChange += GameManagerOnGameStateChange; ;
    }
    private void OnDestroy()
    {
        EventManager.instance.onGameStateChange -= GameManagerOnGameStateChange;
    }
    private void GameManagerOnGameStateChange(GameManager.GameState newGameState)
    {
        switch (newGameState)
        {
            case GameManager.GameState.Start:
                break;
            case GameManager.GameState.Playing:
                break;
            case GameManager.GameState.Win:
                winPanel.SetActive(true);
                break;
            case GameManager.GameState.Lose:
                losePanel.SetActive(true);
                Debug.Log("UiManager:GameManagerOnGameStateChange:losePanel.SetActive(true)");
                break;
        }
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}