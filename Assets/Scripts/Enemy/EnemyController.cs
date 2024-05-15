using System;
using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    EnemySpawner EnemySpawner;
    PlayerController PlayerController;

    private void Awake()
    {
        EnemySpawner     = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        PlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
    private Coroutine _detectPlayerCoroutine;
    private void Start()
    {
        EventManager.instance.onEnemyHit += EnemyBeingHurt;
        player                           =  GameObject.FindWithTag("Player").transform;
        // _detectPlayerCoroutine           =  StartCoroutine(DetectPlayer());


    }

    public Transform player;     // Reference to the player's position
    public float     speed = 1f; // Speed at which the enemy moves towards the player
    public AudioClip DefeatedClip;
    public AudioClip PlayerHurtClip;
    float            distanceToPlayer;
    private void Update()
    {
        // Move our position a step closer to the target.
        distanceToPlayer   = Vector3.Distance(transform.position, player.position);
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed*Time.deltaTime);
    }

    [SerializeField] private AudioClip   _enemyNearClip;
    [SerializeField] private AudioSource _sfxSource;
    // IEnumerator DetectPlayer() {
    //     while (true) {
    //         if (distanceToPlayer < 10f) {
    //             _sfxSource.PlayOneShot(_enemyNearClip, 0.2f);
    //         }
    //         yield return new WaitForSeconds(0.1f);
    //     }
    // }

    private void EnemyBeingHurt(GameObject enemy)
    {
        if (enemy == this.gameObject)
        {
            PlayerController.PlayerKillEnemyNum++;
            SoundManager.instance.PlayEnemySound(DefeatedClip, 0.5f);
            Debug.Log("EnemyController:EnemyBeingHurt");
            Debug.Log("Enemy is being hurt");
            EnemySpawner.EnemyNumber--;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerController.Health--;
            EnemySpawner.EnemyNumber--;
            Debug.Log("Player Health: " + PlayerController.Health);
            EventManager.instance.PlayerHurt(PlayerHurtClip,1);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.onEnemyHit -= EnemyBeingHurt;
        if (_detectPlayerCoroutine != null)
        {
            StopCoroutine(_detectPlayerCoroutine);
        }
    }
}