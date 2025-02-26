using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReadyFight : MonoBehaviour
{
    public Text readyText;
    public Text fightText;

    void Start()
    {
        // 最初は非表示
        readyText.enabled = false;
        fightText.enabled = false;

        StartCoroutine(ShowReadyFight());
    }

    IEnumerator ShowReadyFight()
    {
        // Ready? を表示
        readyText.enabled = true;
        yield return new WaitForSeconds(1f);

        // フェードアウト
        StartCoroutine(FadeOutText(readyText));

        yield return new WaitForSeconds(0.5f);

        // Fight! を表示
        fightText.enabled = true;
        yield return new WaitForSeconds(1f);

        // フェードアウト
        StartCoroutine(FadeOutText(fightText));
    }

    IEnumerator FadeOutText(Text text)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Color originalColor = text.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - (elapsed / duration));
            yield return null;
        }

        text.enabled = false;
    }
}
