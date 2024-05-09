using System;
using TMPro;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    // Start is called before the first frame update
    public  float countdownTime;
    private float currentTime;

    public TextMeshProUGUI timerText; // 新增的Text组件引用

    public event Action OnTimerEnd;

    void Start()
    {
        currentTime = countdownTime;
    }

    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0)
        {
            OnTimerEnd?.Invoke();
            currentTime = 0;
        }
        UpdateTime();
    }

    public void AddTime(float timeToAdd)
    {
        currentTime += timeToAdd;
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }
    public void UpdateTime()
    {
        timerText.text = "Time: " + currentTime.ToString("0.00");
    }
}