using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using System;
using Lofelt.NiceVibrations;
using Manager;
using Random = System.Random;

public class ConeCollider : MonoBehaviour {
    public                   float     radius;
    public                   float     SlowAngle;
    public                   float     FastAngle;
    public                   float     warmRadius;
    public                   LayerMask enemyLayerMask; // 用于过滤敌人的LayerMask
    [SerializeField] private AudioClip _radarClip;
    [SerializeField] private AudioClip _keepClip;
    public                   Sonar     sonar;

    private bool isWaiting = false; // 新增一个标志来表示是否在等待中

    private void Start() {
        EventManager.instance.onEnemyHit += EnemyBeingHurt;
        bool hapticsSupported = DeviceCapabilities.isVersionSupported;
        StartCoroutine(DetectSound());
        StartCoroutine(HandleEnemy());
    }
    [SerializeField] private AudioClip normalEnemyClip1;
    [SerializeField] private AudioClip shootToMoveEnemyClip1;
    [SerializeField] private AudioClip statueEnemyClip1;
    [SerializeField] private AudioClip musicEnemyClip1;
    public class Enemy
    {
        public Collider collider;
        public bool     isSoundPlayed;

        public Enemy(Collider collider)
        {
            this.collider      = collider;
            this.isSoundPlayed = false;
        }
    }

    public GameObject NormalEnemyUI;
    public Image      NormalEnemyUIImage; // Assign this in the Unity editor

    public GameObject StaticEnemyUI;
    public Image ShootToMoveEnemyUIImage;
    public GameObject ShootToMoveEnemyUI;
    public bool isLevel4 = false;
    public float uiDisplayProbability = 0.5f;

    private IEnumerator HandleEnemy()
    {
        float       angleToObject = 180;
        List<Enemy> enemies       = new List<Enemy>();
        while (true)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);
            NormalEnemyUI.SetActive(false);
            StaticEnemyUI.SetActive(false);
            ShootToMoveEnemyUI.SetActive(false);
            foreach (Collider hit in hits)
            {
                Enemy enemy = enemies.Find(e => e.collider == hit);
                if (enemy == null)
                {
                    enemy = new Enemy(hit);
                    enemies.Add(enemy);
                }

                Vector3 direction        = hit.transform.position - transform.position;
                float   distanceToObject = direction.magnitude;
                direction     = direction.normalized;
                angleToObject = Vector3.Angle(transform.forward, direction);
                if (hit.CompareTag("Heart"))
                {
                    if (angleToObject <= FastAngle)
                    {
                        // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                        float volume = 1 - 0.01f;
                        // Debug.Log(volume);
                        SoundManager.instance.PlayEnemySound(_keepClip, volume);
                    }
                    else if (angleToObject >= FastAngle && angleToObject <= SlowAngle)
                    {
                        // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                        float volume = Mathf.InverseLerp(radius, 0, distanceToObject);
                        // Debug.Log(volume);
                        SoundManager.instance.PlayEnemySound(_radarClip, volume);
                    }
                }

                if (hit.CompareTag("Enemy"))
                {
                    if (angleToObject <= (FastAngle + 25) && !isLevel4)
                    {
                        float transparency = 0.9f - angleToObject / (FastAngle + 15);
                        Color imageColor   = NormalEnemyUIImage.color;
                        imageColor.a = transparency;
                            if (hit.name == "NormalEnemy(Clone)") // Add this line
                            {
                                NormalEnemyUI.SetActive(true);

                                NormalEnemyUIImage.color = imageColor;
                            }
                            else if (hit.name == "StaticEnemy(Clone)")
                            {
                                StaticEnemyUI.SetActive(true);
                                // float transparency = 0.8f - angleToObject / (FastAngle + 15);
                                // Color imageColor   = NormalEnemyUIImage.color;
                                // imageColor.a             = transparency;
                                ShootToMoveEnemyUIImage.color = imageColor;
                            }
                            else if (hit.name == "ShootToMoveEnemy(Clone)")
                            {
                                ShootToMoveEnemyUI.SetActive(true);
                                ShootToMoveEnemyUIImage.color = imageColor;


                            }

                    }
                    else
                    {
                        enemy.isSoundPlayed = false;

                    }
                }
            }
            yield return new WaitForSeconds(0.2f);

        }

    }

    IEnumerator DetectSound() {
        float       volume        = 0;
        float       angleToObject = 180;
        float       waitTime      = 0;
        List<Enemy> enemies       = new List<Enemy>();
        while(true){
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);
            NormalEnemyUI.SetActive(false);
            StaticEnemyUI.SetActive(false);
            ShootToMoveEnemyUI.SetActive(false);
            foreach (Collider hit in hits)
            {
                Enemy enemy = enemies.Find(e => e.collider == hit);
                if (enemy == null)
                {
                   enemy = new Enemy(hit);
                   enemies.Add(enemy);
                }
                Vector3 direction        = hit.transform.position - transform.position;
                float   distanceToObject = direction.magnitude;
                direction     = direction.normalized;
                angleToObject = Vector3.Angle(transform.forward, direction);
                if (hit.CompareTag("Heart"))
                {
                    if (angleToObject <= FastAngle)
                    {
                        // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                        volume   = 1 - 0.01f;
                        waitTime = 0f;
                        // Debug.Log(volume);
                        SoundManager.instance.PlayEnemySound(_keepClip, volume);
                    }
                    else if (angleToObject >= FastAngle && angleToObject <= SlowAngle)
                    {
                        // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                        volume   = Mathf.InverseLerp(radius, 0, distanceToObject);
                        waitTime = angleToObject / SlowAngle;
                        // Debug.Log(volume);
                        SoundManager.instance.PlayEnemySound(_radarClip, volume);

                    }
                }

                if (hit.CompareTag("Enemy"))
                {
                    // if (angleToObject<=(FastAngle+15)&&!isLevel4)
                    // {
                    //     float randomValue = UnityEngine.Random.value; // 生成一个0到1之间的随机数
                    //     if (randomValue <= uiDisplayProbability)
                    //     {
                    //         if (hit.name == "NormalEnemy(Clone)") // Add this line
                    //         {
                    //             NormalEnemyUI.SetActive(true);
                    //             float transparency = 1 - angleToObject / (FastAngle+15);
                    //             Color imageColor   = NormalEnemyUIImage.color;
                    //             imageColor.a             = transparency;
                    //             NormalEnemyUIImage.color = imageColor;
                    //         }
                    //         else if (hit.name == "StaticEnemy(Clone)")
                    //         {
                    //             StaticEnemyUI.SetActive(true);
                    //         }
                    //         else if (hit.name == "ShootToMoveEnemy(Clone)")
                    //         {
                    //             ShootToMoveEnemyUI.SetActive(true);
                    //         }
                    //     }
                    // }
                    // else
                    // {
                    //     enemy.isSoundPlayed = false;
                    //
                    // }
                    if (angleToObject <= SlowAngle)
                    {
                        // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                        volume   = 1 - 0.01f;
                        waitTime = angleToObject / SlowAngle;
                        Debug.Log(volume);
                        // SoundManager.instance.PlaySound(_keepClip, volume);
                        sonar.StartSonar();
                        HapticPatterns.PlayEmphasis(volume, 0.0f); // SoundManager.instance.PlaySound(_radarClip, volume);


                    }
                }
            }
            yield return new WaitForSeconds(waitTime+0.12f);
        }
    }

    // 新的方法用于处理角度检测逻辑
    private void HandleAngleDetection(float angle, float distance) {
        float volume = 0;
        float waitTime = 0;

        if(angle <= FastAngle) {
            volume = 1 - 0.01f;
            waitTime = angle/SlowAngle; // 根据角度计算等待时间
            if (!isWaiting) {
                StartCoroutine(PlayKeepVibration(volume, waitTime));
            }
        }
        else if(angle >= FastAngle && angle <= SlowAngle) {
            volume   = angle/SlowAngle;
            waitTime = angle/SlowAngle; // 根据角度计算等待时间
            if (!isWaiting) {
                StartCoroutine(PlayNextSound(_radarClip, volume, waitTime));
            }
        }
    }

    // 新的协程用于播放音频并等待一定时间后执行下一次的音频播放
    private IEnumerator PlayNextSound(AudioClip clip, float volume, float waitTime) {
        isWaiting = true; // 设置等待标志为true
        HapticPatterns.PlayEmphasis(volume, 0.0f);
        SoundManager.instance.PlayEnemySound(clip, volume);
        yield return new WaitForSeconds(waitTime);
        isWaiting = false; // 等待结束后，重置等待标志为false
    }
    private IEnumerator PlayKeepVibration(float volume, float waitTime) {
        isWaiting = true; // 设置等待标志为true
        HapticPatterns.PlayConstant(volume, 0.0f,10.0f);
        yield return new WaitForSeconds(waitTime);
        isWaiting = false; // 等待结束后，重置等待标志为false
    }
    private void EnemyBeingHurt(GameObject enemy) {
        StopCoroutine("PlayKeepVibration");
    }
}