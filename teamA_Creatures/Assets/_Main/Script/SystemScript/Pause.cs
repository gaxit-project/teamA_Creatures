using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseObject;
    public GameObject settingObject;

    public Button continueButton;
    public Button settingButton;
    public Button titleButton;

    // Settingクラスのインスタンスを参照
    public Setting settingScript;

    public static bool isPaused = false;
    private bool canPause = false;

    void Start()
    {
        continueButton.onClick.AddListener(() => { Continue(); continueButton.Select(); });
        settingButton.onClick.AddListener(() => { Setting(); settingButton.Select(); });
        titleButton.onClick.AddListener(() => { Title(); titleButton.Select(); });

        pauseObject.SetActive(false);
        Time.timeScale = 1f;
        StartCoroutine(BeforeFightTime());
    }

    public void OnEnable()
    {
        continueButton.Select();
    }

    void Update()
    {
        if (canPause)
        {
            // ポーズ画面の開閉処理
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
            {
                if (Time.timeScale == 1f)
                {
                    PauseGame(); // ゲームをポーズ状態にする
                }
                else if (Time.timeScale == 0f && settingObject == null)
                {
                    Continue(); // ゲームを再開する
                }
            }
        }

        // ★ボタンのフォーカスを強制的に維持する
        if (pauseObject.activeSelf)
        {
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

            if (selectedObj == null || (selectedObj != continueButton.gameObject &&
                                         selectedObj != settingButton.gameObject &&
                                         selectedObj != titleButton.gameObject))
            {
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
            }
        }
    }

    public void PauseGame()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0f; // ゲームの時間を停止

        // ★ポーズを開いた時に必ず continueButton を選択する
        EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
    }

    public void Continue()
    {
        pauseObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f; // ゲームを再開
    }

    public void Setting()
    {
        // ポーズ画面を閉じて設定パネルを開く
        pauseObject.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;

        // SettingクラスのOpenSettingPanelを呼び出して設定画面を開く
        settingScript.OpenSettingPanel(true);

        // ★設定パネルを開いた後にvolumeButtonを選択する
        EventSystem.current.SetSelectedGameObject(settingScript.volumeButton.gameObject);
    }

    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    IEnumerator BeforeFightTime()
    {
        yield return new WaitForSeconds(3f);
        canPause = true;
    }
}
