using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleManager : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Button endButton;
    public Setting settingScript;

    private Color defaultColor = Color.black; // 非選択時の色
    private Color selectedColor = Color.white; // 選択時の色

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSetting);
        endButton.onClick.AddListener(QuitGame);

        // 各ボタンにイベントを設定
        AddButtonSelectionEffect(startButton);
        AddButtonSelectionEffect(settingButton);
        AddButtonSelectionEffect(endButton);

        startButton.Select();
    }

    void AddButtonSelectionEffect(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        // 選択時の処理
        EventTrigger.Entry onSelectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Select
        };
        onSelectEntry.callback.AddListener((data) => ChangeTextColor(button, selectedColor));
        trigger.triggers.Add(onSelectEntry);

        // 非選択時の処理
        EventTrigger.Entry onDeselectEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.Deselect
        };
        onDeselectEntry.callback.AddListener((data) => ChangeTextColor(button, defaultColor));
        trigger.triggers.Add(onDeselectEntry);
    }

    void ChangeTextColor(Button button, Color color)
    {
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.color = color;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Loading");
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
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
