using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton;
    public Button settingButton;
    public Button endButton;
    public Setting settingScript;

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        settingButton.onClick.AddListener(OpenSetting);
        endButton.onClick.AddListener(QuitGame);
        startButton.Select();
    }

    public void StartGame()
    {
        // まずロードシーンへ遷移
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
