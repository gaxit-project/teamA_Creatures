using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseObject;

    public Button continueButton;
    public Button settingButton;
    public Button titleButton;

    // Settingクラスのインスタンスを参照
    public Setting settingScript;

    void Start()
    {
        continueButton.onClick.AddListener(Continue);
        settingButton.onClick.AddListener(Setting);
        titleButton.onClick.AddListener(Title);

        pauseObject.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OnEnable()
    {
        continueButton.Select();
    }

    void Update()
    {
        // ポーズ画面の開閉処理
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (Time.timeScale == 1f)
            {
                PauseGame(); // ゲームをポーズ状態にする
            }
            else
            {
                Continue(); // ゲームを再開する
            }
        }
    }

    public void PauseGame()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0f; // ゲームの時間を停止
    }

    public void Continue()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1f; // ゲームを再開
    }

    public void Setting()
    {
        // ポーズ画面を閉じて設定パネルを開く
        pauseObject.SetActive(false);
        Time.timeScale = 1f;

        // SettingクラスのOpenSettingPanelを呼び出して設定画面を開く
        settingScript.OpenSettingPanel(true);
    }

    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
}
