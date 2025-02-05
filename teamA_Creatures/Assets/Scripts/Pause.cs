using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Pause : MonoBehaviour
{
    public GameObject pauseObject;
    public GameObject settingObject;
    public GameObject volumeObject; // 音量調整用のオブジェクト
    public Image speakerIcon; // スピーカーマークの画像
    public Slider volumeSlider; // 音量調整スライダー

    public Button continueButton;
    public Button settingButton;
    public Button titleButton;
    public Button volumeButton;

    void Start()
    {
        continueButton.onClick.AddListener(Continue);
        settingButton.onClick.AddListener(Setting);
        titleButton.onClick.AddListener(Title);
        volumeButton.onClick.AddListener(Volume);

        pauseObject.SetActive(false);
        settingObject.SetActive(false);
        volumeObject.SetActive(false);

        // スライダーの初期値をシステム音量に設定
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(ChangeVolume);

        Time.timeScale = 1f;
    }

    public void OnEnable()
    {
        continueButton.Select();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            if (!pauseObject.activeSelf && !settingObject.activeSelf && !volumeObject.activeSelf)
            {
                PauseGame();
            }
            else if (pauseObject.activeSelf && !settingObject.activeSelf && !volumeObject.activeSelf)
            {
                Continue();
            }
            else if (settingObject.activeSelf)
            {
                settingObject.SetActive(false);
                volumeObject.SetActive(false);
                volumeButton.gameObject.SetActive(true);
                continueButton.gameObject.SetActive(true);
                settingButton.gameObject.SetActive(true);
                titleButton.gameObject.SetActive(true);
                continueButton.Select();
            }
            else if (volumeObject.activeSelf) // Escapeで音量設定を閉じる
            {
                volumeObject.SetActive(false);
                volumeButton.gameObject.SetActive(true);
                continueButton.Select();
            }
        }

        // スライダーが選択されているとき、上下入力で操作を変更
        if (volumeObject.activeSelf)
        {
            float horizontalInput = Input.GetAxis("Horizontal"); // 上下の入力

            if (horizontalInput > 0.1f) // 上入力
            {
                volumeSlider.value += 0.01f; // 音量を増加
            }
            else if (horizontalInput < -0.1f) // 下入力
            {
                volumeSlider.value -= 0.01f; // 音量を減少
            }
        }
    }

    public void PauseGame()
    {
        pauseObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        pauseObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Setting()
    {
        settingObject.SetActive(true);

        continueButton.gameObject.SetActive(false);
        settingButton.gameObject.SetActive(false);
        titleButton.gameObject.SetActive(false);

        volumeButton.Select();
    }

    public void Title()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }

    public void Volume()
    {
        volumeObject.SetActive(true);
        volumeButton.gameObject.SetActive(false);
    }

    public void ChangeVolume(float value)
    {
        AudioListener.volume = value;
    }
}
