using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class HeartController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.instance.onHeartHit += HeartBeingHit;
    }

    // Update is called once per frame
    void Update()
    {

    }
    [SerializeField] private AudioClip _heartClip;

    void HeartBeingHit(GameObject heart)
    {
        if (heart == this.gameObject)
        {
            SoundManager.instance.PlayEnemySound(_heartClip, 0.5f);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.onHeartHit -= HeartBeingHit;
    }
}