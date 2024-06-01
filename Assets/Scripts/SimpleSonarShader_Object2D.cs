using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleSonarShader_Object2D : MonoBehaviour
{
    private                 SpriteRenderer[] ObjectRenderers;
    private static readonly Vector4          GarbagePosition  = new Vector4(-5000, -5000, -5000, -5000);
    private static          int              QueueSize        = 20;
    private static          Queue<Vector4>   positionsQueue   = new Queue<Vector4>(QueueSize);
    private static          Queue<float>     intensityQueue   = new Queue<float>(QueueSize);
    private static          bool             NeedToInitQueues = true;
    private delegate        void             Delegate();
    private static          Delegate         RingDelegate;

    private void Start()
    {
        ObjectRenderers = GetComponentsInChildren<SpriteRenderer>();

        if(NeedToInitQueues)
        {
            NeedToInitQueues = false;
            for (int i = 0; i < QueueSize; i++)
            {
                positionsQueue.Enqueue(GarbagePosition);
                intensityQueue.Enqueue(-5000f);
            }
        }

        RingDelegate += SendSonarData;
        RingDelegate += SendSonarData;

        // 开始Coroutine
        StartCoroutine(TriggerSonarRing());
    }
    private IEnumerator TriggerSonarRing()
    {
        // 无限循环
        while (true)
        {
            // 你可以根据需要修改这里的参数
            StartSonarRing(new Vector4(0, 0, 0, 0), 1.0f);


            // 等待一定的时间后再次触发，你可以根据需要修改这里的等待时间
            yield return new WaitForSeconds(1.0f);
        }
    }
    public void StartSonarRing(Vector4 position, float intensity)
    {
        position.w = Time.timeSinceLevelLoad;
        positionsQueue.Dequeue();
        positionsQueue.Enqueue(position);

        intensityQueue.Dequeue();
        intensityQueue.Enqueue(intensity);

        RingDelegate();
        Debug.Log("StartSonarRing called with position: " + position + ", intensity: " + intensity);
    }

    private void SendSonarData()
    {
        foreach (SpriteRenderer r in ObjectRenderers)
        {
            r.material.SetVectorArray("_hitPts", positionsQueue.ToArray());
            r.material.SetFloatArray("_Intensity", intensityQueue.ToArray());
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        StartSonarRing(collision.contacts[0].point, collision.relativeVelocity.magnitude / 10.0f);
    }

    private void OnDestroy()
    {
        RingDelegate -= SendSonarData;
    }
}