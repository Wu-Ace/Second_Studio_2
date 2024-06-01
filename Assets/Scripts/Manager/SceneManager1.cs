using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager1 : MonoBehaviour
{
    public static SceneManager1 Instance;
    // private void Awake()
    // {
    //     if (Instance !=  null)
    //     {
    //         Destroy(this);
    //     }
    //     Instance = this;
    // }
    public void UI_StartGame()
    {
        SceneManager.LoadScene("LevelSelectionScene");
        Debug.Log("UI_StartGame");
    }
    public void UI_QuitGame()
    {
        Application.Quit();
        Debug.Log("UI_QuitGame");
    }
    public void UI_BackToLevelSelectionScene()
    {
        SceneManager.LoadScene("LevelSelectionScene");
        Debug.Log("UI_BackToLevelSelectionScene");
    }
    public void SelectLevel1()
    {
        SceneManager.LoadScene("Level1");
        Debug.Log("SelectLevel1");
    }
}