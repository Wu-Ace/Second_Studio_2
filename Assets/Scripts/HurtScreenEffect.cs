using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Manager;

public class ScreenEffect : MonoBehaviour
{
    public Image damageImage;    // Assign this in the inspector
    public float fadeSpeed = 5f; // Speed at which the screen effect fades away

    private void Start()
    {
        // Ensure the image is fully transparent at the start
        Color color = damageImage.color;
        color.a           = 0f;
        damageImage.color = color;
        EventManager.instance.onPlayerHurt += TriggerDamageEffect;
    }

    public void TriggerDamageEffect(AudioClip clip, float volume)
    {
        // Play the audio clip
        // AudioSource.PlayClipAtPoint(clip, transform.position, volume);

        // Stop any existing fade coroutines to start a new one
        StopAllCoroutines();
        StartCoroutine(FadeDamageEffect());
    }

    private IEnumerator FadeDamageEffect()
    {
        Color color = damageImage.color;
        color.a           = 1f;
        damageImage.color = color;

        // Gradually fade the color to transparent
        while (damageImage.color.a > 0f)
        {
            color             =  damageImage.color;
            color.a           -= fadeSpeed * Time.deltaTime*0.2f;
            damageImage.color =  color;

            yield return null;
        }
    }
}