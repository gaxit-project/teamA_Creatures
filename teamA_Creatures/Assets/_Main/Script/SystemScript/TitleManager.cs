using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton; // STARTボタン
    public Button settingButton; // 設定ボタン
    public Button endButton;   // ENDボタン
    public Setting settingScript; // Settingスクリプトの参照

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSetting);
        endButton.onClick.AddListener(QuitGame);
        startButton.Select();
    }

    void Update()
    {
    }

    public void StartGame()
    {
        // メインシーンに遷移
        SceneManager.LoadScene("Main");
    }

    public void OpenSetting()
    {
        if (settingScript != null)
        {
            settingScript.OpenSettingPanel(true);
        }
    }

    public void QuitGame()
    {
        // ゲームを終了
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ内での終了処理
#endif
    }
}