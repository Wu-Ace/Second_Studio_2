using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance = null;
    public static  BackgroundMusic Instance
    {
        get { return instance; }
    }
    public         AudioSource     BackgroundMusicSource;
    public        AudioClip       BackgroundMusicClip;


    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        BackgroundMusicSource.PlayOneShot(BackgroundMusicClip, 0.5f);
    }
}