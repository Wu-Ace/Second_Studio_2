using UnityEngine;
using TMPro;
using System.Collections;

public class ComboEffect : MonoBehaviour
{
    public static ComboEffect instance; // 这是新添加的单例

    public TMP_Text comboText;      // Assign this in the inspector
    public float    fadeSpeed = 5f; // Speed at which the combo text fades away

    private void Start()
    {
        // Ensure the combo text is fully transparent at the start
        Color color = comboText.color;
        color.a         = 0f;
        comboText.color = color;
    }

    public void TriggerComboEffect(int comboCount)
    {
        if (comboCount > 3)
        {
            comboText.text = "连击数: " + comboCount;
            // Stop any existing fade coroutines to start a new one
            StopAllCoroutines();
            StartCoroutine(FadeComboEffect());
        }
    }

    private IEnumerator FadeComboEffect()
    {
        Color color = comboText.color;
        color.a         = 1f;
        comboText.color = color;

        // Gradually fade the color to transparent
        while (comboText.color.a > 0f)
        {
            color           =  comboText.color;
            color.a         -= fadeSpeed * Time.deltaTime;
            comboText.color =  color;

            yield return null;
        }
    }
}