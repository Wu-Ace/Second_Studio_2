using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Manager;
public class MusicEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] EnemySpawner     enemySpawner;
    [SerializeField] PlayerController playerController;
    [SerializeField] AudioClip        defeatedClip;
    [SerializeField] AudioClip musicToPlay;
    void Start()
    {
        EventManager.instance.onEnemyHit += EnemyBeingHurt;
        SoundManager.instance.PlayMusic(musicToPlay,1);
    }
    public Transform player;     // Reference to the player's position
    public float     speed = 1f; // Speed at which the enemy moves towards the player
    public AudioClip PlayerHurtClip;
    float            distanceToPlayer;
    void Update()
    {
        distanceToPlayer = Vector3.Distance(transform.position, player.position);
    }

    private void EnemyBeingHurt(GameObject enemy)
    {
        Debug.Log("EnemyController:EnemyBeingHurt");
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerController.Health--;
            enemySpawner.EnemyNumber--;
            Debug.Log("Player Health: " + playerController.Health);
            EventManager.instance.PlayerHurt(PlayerHurtClip,1);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        EventManager.instance.onEnemyHit -= EnemyBeingHurt;
        SoundManager.instance.StopMusic();
    }
}