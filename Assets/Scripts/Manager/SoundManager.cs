using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager instance;

    [SerializeField] private AudioSource _musicSource, _sfxSource;
    private void Awake()
    {
        if (instance !=  null)
        {
            Destroy(this);
        }
        instance                           =  this;
        EventManager.instance.onPlaySound  += PlaySound;
        EventManager.instance.onPlayerDie  += PlaySound;
        EventManager.instance.onPlayerHurt += PlaySound;
        EventManager.instance.onPlayerWin += PlaySound;
    }
    public void PlaySound(AudioClip clip, float volume)
    {
        // _sfxSource.pitch = pitch;
        _sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayerDie(AudioClip clip, float volume)
    {
        _sfxSource.PlayOneShot(clip, volume);
    }


}