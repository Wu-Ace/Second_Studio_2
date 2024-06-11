using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager1 : MonoBehaviour
{
    public static SceneManager1 Instance;

    public AudioSource ButtonSource;
    public AudioClip   ShortButtonClip;
    public AudioClip  LongButtonClip;
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
        ButtonSource.PlayOneShot(ShortButtonClip);

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
        ButtonSource.PlayOneShot(LongButtonClip);
        SceneManager.LoadScene("Level1");
        Debug.Log("SelectLevel1");
    }
    public void SelectLevel2()
    {
        ButtonSource.PlayOneShot(LongButtonClip);

        SceneManager.LoadScene("Level2");
        Debug.Log("SelectLevel2");
    }
    public void SelectLevel3()
    {
        ButtonSource.PlayOneShot(LongButtonClip);

        SceneManager.LoadScene("Level3");
        Debug.Log("SelectLevel3");
    }
    public void SelectLevel4()
    {
        ButtonSource.PlayOneShot(LongButtonClip);

        SceneManager.LoadScene("Level4");
        Debug.Log("SelectLevel4");
    }
    public void SelectLevel5()
    {
        ButtonSource.PlayOneShot(LongButtonClip);

        SceneManager.LoadScene("Level5");
        Debug.Log("SelectLevel5");
    }
    public void SelectLevel6()
    {
        ButtonSource.PlayOneShot(LongButtonClip);

        SceneManager.LoadScene("Level6");
        Debug.Log("SelectLevel6");
    }
}