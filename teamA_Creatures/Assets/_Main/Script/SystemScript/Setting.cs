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
    public List<AudioSource> seSources;

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
            Time.timeScale = 0f; // ★設定画面を開いたらゲームを停止
            StartCoroutine(SetFocusAfterFrame());
        }
        else
        {
            Time.timeScale = 1f; // ★設定画面を閉じたらゲームを再開
            EventSystem.current.SetSelectedGameObject(titleButtons[0].gameObject);
        }
    }

    public void Volume()
    {
        isVolumeOpen = !isVolumeOpen;
        volumePanel.SetActive(isVolumeOpen);

        if (isVolumeOpen)
        {
            Time.timeScale = 0f; // ★音量調節画面を開いたらゲームを停止
            currentIndex = 0;
            UpdateTextColors();
            EventSystem.current.SetSelectedGameObject(volumeOptions[currentIndex].gameObject);
        }
        else
        {
            Time.timeScale = 1f; // ★音量調節画面を閉じたらゲームを再開
            ResetTextColors();
            EventSystem.current.SetSelectedGameObject(volumeButton.gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            // ★ポーズ画面が開いている場合は、設定の処理をスキップ
            if (Pause.isPaused) return;

            if (isVolumeOpen)
            {
                isVolumeOpen = false;
                volumePanel.SetActive(false);
                ResetTextColors();
                EventSystem.current.SetSelectedGameObject(volumeButton.gameObject);
            }
            else if (isSettingOpen)
            {
                OpenSettingPanel(false);
                volumePanel.SetActive(false);
                EventSystem.current.SetSelectedGameObject(titleButtons[1].gameObject);
            }
            return;
        }

        // ★スライダーの選択状態に応じてテキスト色を変更
        GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

        if (selectedObj == bgmSlider.gameObject)
        {
            bgmText.color = Color.red;
            seText.color = Color.white;
        }
        else if (selectedObj == seSlider.gameObject)
        {
            bgmText.color = Color.white;
            seText.color = Color.red;
        }
        else
        {
            bgmText.color = Color.white;
            seText.color = Color.white;
        }
    }



    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

    public void SetSeVolume(float volume)
    {
        seVolume = volume;

        // リスト内のすべてのSEの音量を調整
        foreach (AudioSource se in seSources)
        {
            se.volume = seVolume;
        }
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
    private IEnumerator SetFocusAfterFrame()
    {
        yield return null; // 1フレーム待機
        EventSystem.current.SetSelectedGameObject(volumeButton.gameObject);
    }
}
