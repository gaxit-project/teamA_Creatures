using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton; // STARTボタン
    public Button endButton;   // ENDボタン

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
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

    public void QuitGame()
    {
        // ゲームを終了
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ内での終了処理
#endif
    }
}
