using System.Collections;
using UnityEngine;
using System;
using Lofelt.NiceVibrations;
using Manager;
public class ConeCast : MonoBehaviour {
    public float radius;
    public float SlowAngle;
    public float FastAngle;
    public float warmRadius;
    public LayerMask enemyLayerMask; // 用于过滤敌人的LayerMask
    [SerializeField] private AudioClip _radarClip;
    [SerializeField] private AudioClip _keepClip;

    private bool isWaiting = false; // 新增一个标志来表示是否在等待中

    private void Start() {
        EventManager.instance.onEnemyHit += EnemyBeingHurt;
        bool hapticsSupported = DeviceCapabilities.isVersionSupported;
        StartCoroutine(DetectSound());
    }

    IEnumerator DetectSound() {
        float volume        = 0;
        float angleToObject = 180;
        float waitTime      = 0;
        while(true){
            Collider[] hits = Physics.OverlapSphere(transform.position, radius, enemyLayerMask);


            foreach (Collider hit in hits)
            {

                    Vector3 direction        = hit.transform.position - transform.position;
                    float   distanceToObject = direction.magnitude;
                    direction     = direction.normalized;
                    angleToObject = Vector3.Angle(transform.forward, direction);
                    if (hit.tag == "Heart")
                    {
                        if (angleToObject <= FastAngle)
                        {
                            // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                            volume   = 1 - 0.01f;
                            waitTime = 0f;
                            // Debug.Log(volume);
                            SoundManager.instance.PlaySound(_keepClip, volume);
                        }
                        else if (angleToObject >= FastAngle && angleToObject <= SlowAngle)
                        {
                            // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                            volume   = Mathf.InverseLerp(radius, 0, distanceToObject);
                            waitTime = angleToObject / SlowAngle;
                            // Debug.Log(volume);
                            SoundManager.instance.PlaySound(_radarClip, volume);

                        }
                    }

                    if (hit.tag == "Enemy")
                    {
                        if (angleToObject <= SlowAngle)
                        {
                            // Debug.Log("Object detected! Distance: " + distanceToObject + " Angle: " + angleToObject);
                            volume   = 1 - 0.01f;
                            waitTime = angleToObject / SlowAngle;
                            Debug.Log(volume);
                            // SoundManager.instance.PlaySound(_keepClip, volume);
                            HapticPatterns.PlayEmphasis(volume, 0.0f);                            // SoundManager.instance.PlaySound(_radarClip, volume);
                        }
                    }

            }

            yield return new WaitForSeconds(waitTime);
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
        SoundManager.instance.PlaySound(clip, volume);
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