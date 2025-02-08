using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    public Button settingButton;
    public GameObject settingPanel;
    public GameObject volumePanel;

    public Button volumeButton;
    public Slider bgmSlider;
    public Slider seSlider;

    public Text bgmText;
    public Text seText;

    public AudioSource bgmSource;
    public AudioSource seSource;

    public List<Button> titleButtons; // タイトル画面のボタンをリストで取得

    // 音量設定を保持するための静的変数
    public static float bgmVolume = 1f;
    public static float seVolume = 1f;

    private bool isSettingOpen = false;
    private bool isVolumeOpen = false;

    private Selectable[] volumeOptions;
    private Text[] volumeTexts;
    private int currentIndex = 0;
    private float switchCooldown = 0.5f;
    private float lastSwitchTime = 0f;

    void Start()
    {
        settingButton.onClick.AddListener(() => OpenSettingPanel(true));
        volumeButton.onClick.AddListener(Volume);

        settingPanel.SetActive(false);
        volumePanel.SetActive(false);

        volumeOptions = new Selectable[] { bgmSlider, seSlider };
        volumeTexts = new Text[] { bgmText, seText };

        bgmSlider.onValueChanged.AddListener(SetBgmVolume);
        seSlider.onValueChanged.AddListener(SetSeVolume);

        // 最初に音量を設定
        bgmSlider.value = bgmVolume;
        seSlider.value = seVolume;
    }

    public void OpenSettingPanel(bool open)
    {
        isSettingOpen = open;
        settingPanel.SetActive(isSettingOpen);

        // 設定パネルが開かれている場合、タイトル画面のボタンを無効化
        foreach (Button btn in titleButtons)
        {
            btn.interactable = !isSettingOpen;
        }

        if (isSettingOpen)
        {
            // 設定パネルが開いているときはVolumeボタンを選択
            EventSystem.current.SetSelectedGameObject(volumeButton.gameObject);
        }
        else
        {
            // 設定パネルを閉じるときに、タイトル画面の「Start」ボタンを選択
            EventSystem.current.SetSelectedGameObject(titleButtons[0].gameObject); // Startボタン
        }
    }

    public void Volume()
    {
        isVolumeOpen = !isVolumeOpen;
        volumePanel.SetActive(isVolumeOpen);

        if (isVolumeOpen)
        {
            currentIndex = 0;
            UpdateTextColors();
            EventSystem.current.SetSelectedGameObject(volumeOptions[currentIndex].gameObject);
        }
        else
        {
            ResetTextColors();
            EventSystem.current.SetSelectedGameObject(volumeButton.gameObject);
        }
    }

    void Update()
    {
        // ESCキーまたはJoystickButton7で設定画面を閉じる
        if (isSettingOpen && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)))
        {
            OpenSettingPanel(false);
            volumePanel.SetActive(false);

            // 設定パネルを閉じた後、次のボタンを選択できるように設定
            EventSystem.current.SetSelectedGameObject(titleButtons[1].gameObject); // 次のボタン（例えばExit）を選択
            return;
        }

        // その他の更新処理（音量の設定など）
        if (isVolumeOpen && Time.time - lastSwitchTime > switchCooldown)
        {
            float vertical = Input.GetAxis("Vertical");

            if (vertical > 0.1f)
            {
                currentIndex = (currentIndex - 1 + volumeOptions.Length) % volumeOptions.Length;
                UpdateTextColors();
                EventSystem.current.SetSelectedGameObject(volumeOptions[currentIndex].gameObject);
                lastSwitchTime = Time.time;
            }
            else if (vertical < -0.1f)
            {
                currentIndex = (currentIndex + 1) % volumeOptions.Length;
                UpdateTextColors();
                EventSystem.current.SetSelectedGameObject(volumeOptions[currentIndex].gameObject);
                lastSwitchTime = Time.time;
            }
        }

        if (isVolumeOpen)
        {
            float horizontal = Input.GetAxis("Horizontal");

            if (horizontal > 0.1f)
            {
                if (volumeOptions[currentIndex] is Slider slider)
                {
                    slider.value += 0.01f;
                }
            }
            else if (horizontal < -0.1f)
            {
                if (volumeOptions[currentIndex] is Slider slider)
                {
                    slider.value -= 0.01f;
                }
            }
        }
    }



    // 音量の設定
    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

    public void SetSeVolume(float volume)
    {
        seVolume = volume;
        seSource.volume = seVolume;
    }

    private void UpdateTextColors()
    {
        for (int i = 0; i < volumeTexts.Length; i++)
        {
            volumeTexts[i].color = (i == currentIndex) ? Color.red : Color.white;
        }
    }

    private void ResetTextColors()
    {
        foreach (Text text in volumeTexts)
        {
            text.color = Color.white;
        }
    }
}
