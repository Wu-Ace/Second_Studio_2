using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Manager;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;


public class PlayerController : MonoBehaviour
{
    public LayerMask shootableObjectLayerMask;
    public int       bulletMag = 8;
    public int       currentBullet;
    public int       Health = 3;

    public int PlayerKillEnemyNum = 0;
    public int PlayerKillHeartNum = 0;


    private float           old_y = 0;
    private float           new_y;
    private float           currentDistance = 0;
    public  TextMeshProUGUI HealthText;
    public  bool            isLose;


    [SerializeField] private AudioClip         _shootClip;
    [SerializeField] private AudioClip         _reloadClip;
    [SerializeField] private AudioClip         _emptyClip;
    [SerializeField] private AudioClip         _dieClip;
    private                  AndroidJavaObject vibrator;

    public  ConeCollider ConeCollider;
    private GameObject   heartGameObject;
    public  HeartSpawner _heartSpawner;
    public  GameManager  gameManager;


    public void Start()
    {
        EventManager.instance.onPlayerShoot += PlayerShoot;
        EventManager.instance.onPlayerPress += PlayerShootHeart;

        currentBullet      = 7;
        PlayerKillEnemyNum = 0;
        PlayerKillHeartNum = 0;
        isLose             = false;
        successiveHitNum   = 0;

        UpdateHealthText();
        if (EventManager.instance == null)
        {
            Debug.LogError("EventManager.instance is null");
        }
    }

    private bool  isShootingHeart;
    public  float shootHeartTime;
    private float shootHeartTimer;
    public bool needSuccessiveHit=true;

    public void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 10, Color.yellow);
        CheckIfReload();
        // CheckHealth();
        if (isShootingHeart)
        {
            shootHeartTimer += Time.deltaTime;
        }

        CheckPlayerState();
    }

    public           int             successiveHitNum;
    [SerializeField] TextMeshProUGUI successiveHitText;
    [SerializeField] TextMeshProUGUI currentBulletText;

    public void PlayerShoot()
    {
        float      radius        = 100f;
        Collider[] hits          = Physics.OverlapSphere(transform.position, radius, shootableObjectLayerMask);
        float      angleToObject = 180;
        float      attackAngle   = ConeCollider.FastAngle;

        // Ray        ray  = new Ray(transform.position,  transform.forward);
        // RaycastHit hit;
        // Debug.DrawLine(transform.position, transform.position+transform.forward, Color.yellow);
        bool isSuccesiveHit = false;

        if (currentBullet > 0)
        {
            SoundManager.instance.PlayPlayerSound(_shootClip, 1);
            currentBullet--;
            if(currentBullet==0)
            {
                SoundManager.instance.PlayPlayerSound(_emptyClip, 1);
            }
            UpdateCurrentBulletText();
        }


        foreach (Collider hit in hits)
        {
            Vector3 direction = hit.transform.position - transform.position;
            angleToObject = Vector3.Angle(transform.forward, direction);
            if (currentBullet > 0)
            {
                if (angleToObject < attackAngle && currentBullet > 0 && hit.CompareTag("Enemy"))
                {
                    Debug.Log("Hit");
                    Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
                    // StartCoroutine(PauseAfterHit(1f));
                    EventManager.instance.EnemyHit(hit.transform.gameObject);
                    // gameManager.CurrentSurvivalTime += 10;
                    isSuccesiveHit = true;
                    successiveHitNum++;
                    ComboEffect.instance.TriggerComboEffect(successiveHitNum);
                    if (needSuccessiveHit &&successiveHitNum > 3 )
                    {
                        bulletMag += successiveHitNum - 2;
                        UpdateCurrentBulletText();
                    }

                    UpdateSuccesiveHitText();
                    if (hit.tag == "Enemy")
                    {
                        PlayerKillEnemyNum++;
                    }
                }


                if (GameManager.GameState.Start != GameManager._instance.gameState)
                {
                    // gameManager.CurrentSurvivalTime -= 5;
                    // Debug.Log("-5 second");
                }
            }
            else
            {
                SoundManager.instance.PlayPlayerSound(_emptyClip, 1);
            }

        }

        if (!isSuccesiveHit)
        {
            successiveHitNum = 0;
            UpdateSuccesiveHitText();
        }
    }

    public void UpdateSuccesiveHitText()
    {
        successiveHitText.text = "连击: " + successiveHitNum;
    }

    public void UpdateCurrentBulletText()
    {
        currentBulletText.text = "子弹: " + currentBullet + "/" + bulletMag;
    }

    [SerializeField] AudioClip absorbClip;

    public void PlayerShootHeart()
    {
        float      radius        = 100f;
        Collider[] hits          = Physics.OverlapSphere(transform.position, radius, shootableObjectLayerMask);
        float      angleToObject = 180;
        float      attackAngle   = ConeCollider.FastAngle;
        foreach (Collider hit in hits)
        {
            Vector3 direction = hit.transform.position - transform.position;
            angleToObject = Vector3.Angle(transform.forward, direction);

            if (angleToObject < attackAngle && hit.transform.gameObject.tag == "Heart")
            {

                Debug.DrawLine(transform.position, hit.transform.position, Color.yellow);
                isShootingHeart = true;
                SoundManager.instance.PlayPlayerSound(absorbClip, 1);
                if (shootHeartTimer >= shootHeartTime)
                {
                    Health++;
                    Debug.Log("shootHeart");
                    EventManager.instance.HeartHit(hit.transform.gameObject);
                    shootHeartTimer = 0f;
                    isShootingHeart = false;
                    // gameManager.CurrentSurvivalTime += 20;
                    PlayerKillHeartNum++;
                    UpdateHealthText();
                    _heartSpawner.HeartNumber--;
                    if (PlayerKillHeartNum < 3)
                    {
                        Debug.Log("SpawnHeart");
                        _heartSpawner.SpawnHeart();
                    }

                }
            }

            if (angleToObject > attackAngle && isShootingHeart && hit.transform.gameObject.tag == "Heart")
            {
                isShootingHeart = false;
                shootHeartTimer = 0f;
            }
        }
    }

    IEnumerator PauseAfterHit(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }


    public bool isLevel1_2=false;
    public void CheckIfReload()
    {

        new_y           = Input.acceleration.y;
        currentDistance = new_y - old_y;
        old_y           = new_y;

        if (!isLevel1_2&&currentDistance > 0.5f && Input.deviceOrientation == DeviceOrientation.Portrait)
        {
            Debug.Log("Reload");
            if (currentBullet + bulletMag > 7)
            {
                bulletMag     = bulletMag - (7 - currentBullet);
                currentBullet = 7;
            }
            else if (currentBullet + bulletMag <= 7)
            {
                currentBullet = currentBullet + bulletMag;
                bulletMag     = 0;
            }


            SoundManager.instance.PlayPlayerSound(_reloadClip, 1);
            UpdateCurrentBulletText();
        }
    }

    // public void CheckHealth()
    // {
    //     if (Health <= 0&&!isLose)
    //     {
    //         EventManager.instance.PlayerDie(_dieClip,0.7f);
    //         isLose = true;
    //         EventManager.instance.GameStateChange(GameManager.GameState.Lose);
    //     }
    //     UpdateHealthText();
    // }

    public void UpdateHealthText()
    {
        HealthText.text = "体力: " + Health; // 更新生命值文本
    }

    private void OnDestroy()
    {
        EventManager.instance.onPlayerShoot -= PlayerShoot;
    }

    private void CheckPlayerState()
    {
        if (Health <= 0 && GameManager.GameState.Playing == GameManager._instance.gameState)
        {
            EventManager.instance.PlayerDie(_dieClip, 0.7f);
            EventManager.instance.GameStateChange(GameManager.GameState.Lose);
        }

        if (PlayerKillHeartNum == 1)
        {
            EventManager.instance.GameStateChange(GameManager.GameState.ShootingTutorial);
        }

        if (PlayerKillHeartNum == 3)
        {
            EventManager.instance.GameStateChange(GameManager.GameState.EnemySpawning);
        }
    }

    public Dictionary<string, int> killCount = new Dictionary<string, int>();

    public void IncrementKillCount(string enemyName)
    {
        if (killCount.ContainsKey(enemyName))
        {
            killCount[enemyName]++;
        }
        else
        {
            killCount.Add(enemyName, 1);
        }
    }
}