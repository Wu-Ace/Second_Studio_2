using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;


public class ShootToMoveEnemyController : MonoBehaviour
{
    public Transform player; // Reference to the player's position
    PlayerController playerController;
    EnemySpawner     enemySpawner;


    void Start()
    {
        player            = GameObject.FindWithTag("Player").transform;
        playerController = player.GetComponent<PlayerController>();

        enemySpawner = GameObject.FindWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        EventManager.instance.onEnemyHit += EnemyBeingHurt;
        EventManager.instance.onPlayerShoot += Move;

    }

    // Update is called once per frame
    private float distanceToPlayer;
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);

    }
    [SerializeField]private AudioClip defeatedClip;
    [SerializeField] int health = 1;
    private void EnemyBeingHurt(GameObject enemy)
    {
        if (enemy == this.gameObject)
        {
            health--;
            if (health == 0)
            {
                playerController.PlayerKillEnemyNum++;
                SoundManager.instance.PlayEnemySound(defeatedClip, 0.5f);
                Debug.Log("EnemyController:EnemyBeingHurt");
                Debug.Log("Enemy is being hurt");
                enemySpawner.EnemyNumber--;
                Destroy(this.gameObject);
            }
        }
    }

    [SerializeField] float moveDistance;
    [SerializeField] AudioClip moveClip;
    private void Move()
    {
        float distanceToPlayer;
        float volume;
        distanceToPlayer   = Vector3.Distance(transform.position, player.position);
        volume             = 1 / Mathf.Pow(20, distanceToPlayer/ enemySpawner.spawnRadius);
        transform.position = Vector3.MoveTowards(transform.position, player.position, moveDistance);
        EventManager.instance.PlaySound(moveClip, volume);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.Health--;
            enemySpawner.EnemyNumber--;
            Debug.Log("Player Health: " + playerController.Health);
            // EventManager.instance.PlayerHurt(playerHurtClip,1);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.onEnemyHit -= EnemyBeingHurt;
        EventManager.instance.onPlayerShoot -= Move;
        playerController.IncrementKillCount("ShootToMoveEnemy");
    }
}