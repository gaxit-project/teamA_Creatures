using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading1 : MonoBehaviour
{
    public Slider progressBar;

    void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    IEnumerator LoadMainSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false; // 自動遷移を防ぐ

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress; // 進捗をスライダーに反映

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f); // 1秒待機（演出用）
                operation.allowSceneActivation = true; // メインシーンへ遷移
            }

            yield return null;
        }
    }
}
