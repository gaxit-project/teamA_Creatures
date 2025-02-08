using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Setting : MonoBehaviour
{
    public Button settingButton;
    public GameObject settingPanel;
    public GameObject volumePanel;

    public Button volumeButton;
    public Slider bgmSlider;
    public Slider seSlider;

    public List<Button> titleButtons; // タイトル画面のボタンをリストで取得

    private bool isSettingOpen = false;
    private bool isVolumeOpen = false;
    void Start()
    {
        settingButton.onClick.AddListener(OpenSettingPanel);
        volumeButton.onClick.AddListener(Volume);

        settingPanel.SetActive(false);
        volumePanel.SetActive(false);
    }

    public void OpenSettingPanel()
    {
        isSettingOpen = !isSettingOpen;
        settingPanel.SetActive(isSettingOpen);

        // タイトルのボタンを無効化/有効化
        foreach (Button btn in titleButtons)
        {
            btn.interactable = !isSettingOpen;
        }

        if (isSettingOpen)
        {
            // 設定画面を開いたら VolumeButton を選択状態にする
            EventSystem.current.SetSelectedGameObject(volumeButton.gameObject);
        }
    }

    public void Volume()
    {
        
    }
}
