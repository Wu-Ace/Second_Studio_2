using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static SoundManager instance;

    [SerializeField] private AudioSource _musicSource, _playerSfxSource, enemySfxSource;
    private void Awake()
    {
        if (instance !=  null)
        {
            Destroy(this);
        }
        instance                           =  this;
        EventManager.instance.onPlaySound  += PlayPlayerSound;
        EventManager.instance.onPlayerDie  += PlayPlayerSound;
        EventManager.instance.onPlayerHurt += PlayPlayerSound;
        EventManager.instance.onPlayerWin  += PlayPlayerSound;
    }
    public void PlayPlayerSound(AudioClip clip, float volume)
    {
        // _sfxSource.pitch = pitch;
        _playerSfxSource.PlayOneShot(clip, volume);
    }
    public void StopPlayerSound()
    {
        _playerSfxSource.Stop();
    }
    public void PlayEnemySound(AudioClip clip, float volume)
    {
        // _sfxSource.pitch = pitch;
        enemySfxSource.PlayOneShot(clip, volume);
    }

    public void PlayerDie(AudioClip clip, float volume)
    {
        _playerSfxSource.PlayOneShot(clip, volume);
    }
    public void PlayMusic(AudioClip clip, float volume)
    {
        _musicSource.PlayOneShot(clip, volume);
    }
    public void StopMusic()
    {
        _musicSource.Stop();
    }

}