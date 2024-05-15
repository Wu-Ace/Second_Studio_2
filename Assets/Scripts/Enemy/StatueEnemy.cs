using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;

public class StatueEnemy : MonoBehaviour
{
    [SerializeField] EnemySpawner     enemySpawner;
    [SerializeField] PlayerController playerController;
    [SerializeField] ConeCollider     coneCollider;

    private float stopMovingAngle;
    private Coroutine detectPlayerCoroutine;
    private void Start()
    {
        EventManager.instance.onEnemyHit += EnemyBeingHurt;
        stopMovingAngle                  =  coneCollider.SlowAngle / 2;
        detectPlayerCoroutine            =  StartCoroutine(DetectPlayer());    }

    [SerializeField] Transform player;
    [SerializeField] float     speed = 1f;
    private void Update()
    {
        distanceToPlayer   = Vector3.Distance(transform.position, player.position);
    }

    [SerializeField] AudioClip defeatedClip;
    [SerializeField] AudioClip playerHurtClip;
    private          float     distanceToPlayer;
    private void EnemyBeingHurt(GameObject enemy)
    {
        if (enemy == this.gameObject)
        {
            playerController.PlayerKillEnemyNum++;
            SoundManager.instance.PlayEnemySound(defeatedClip, 0.5f);
            Debug.Log("EnemyController:EnemyBeingHurt");
            Debug.Log("Enemy is being hurt");
            enemySpawner.EnemyNumber--;
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private LayerMask playerLayerMask;

    IEnumerator DetectPlayer()
    {
        while (true)
        {
           Collider[] hits = Physics.OverlapSphere(transform.position, 1000f, playerLayerMask);
           foreach (Collider hit in hits)
           {
               if (hit.CompareTag("Player"))
               {
                   Vector3 direction     = transform.position - hit.transform.position;
                   direction = direction.normalized;
                   float   angleToObject = Vector3.Angle(hit.transform.forward,direction);
                   if (angleToObject > 90f)
                   {
                       transform.position = Vector3.MoveTowards(transform.position, hit.transform.position, speed * Time.deltaTime);
                       Debug.Log("Player Detected");

                   }
               }
           }
           yield return new WaitForSeconds(0.1f);
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.Health--;
            enemySpawner.EnemyNumber--;
            Debug.Log("Player Health: " + playerController.Health);
            EventManager.instance.PlayerHurt(playerHurtClip,1);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.onEnemyHit -= EnemyBeingHurt;
        if (detectPlayerCoroutine != null)
        {
            StopCoroutine(detectPlayerCoroutine);
        }
    }
}