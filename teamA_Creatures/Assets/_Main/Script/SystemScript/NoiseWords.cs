using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNoiseEffect : MonoBehaviour
{
    public Text targetText; // Legacy Text UI
    public float noiseFrequency = 0.1f; // ‚Ç‚ê‚­‚ç‚¢‚Ì•p“x‚ÅƒmƒCƒY‚ð“ü‚ê‚é‚©
    private string originalText;

    void Start()
    {
        if (targetText != null)
        {
            originalText = targetText.text;
            StartCoroutine(AddNoiseEffect());
        }
    }

    IEnumerator AddNoiseEffect()
    {
        while (true)
        {
            targetText.text = GetNoisyText(originalText);
            yield return new WaitForSeconds(noiseFrequency);
        }
    }

    string GetNoisyText(string text)
    {
        char[] noisyChars = { '#', '@', '%', '&', '*', '!', '?' };
        char[] textArray = text.ToCharArray();

        for (int i = 0; i < textArray.Length; i++)
        {
            if (Random.value < 0.2f) // 20%‚ÌŠm—¦‚Å•¶Žš‚ðƒmƒCƒY‚É•Ï‚¦‚é
            {
                textArray[i] = noisyChars[Random.Range(0, noisyChars.Length)];
            }
        }

        return new string(textArray);
    }
}

