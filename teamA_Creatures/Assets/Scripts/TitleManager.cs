using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public Button startButton; // STARTボタン
    public Button endButton;   // ENDボタン
    private Button currentButton; // 現在選択中のボタン

    void Start()
    {
        // 初期設定でSTARTボタンを選択
        currentButton = startButton;
        HighlightButton(currentButton);
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // ボタンの切り替え
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentButton == endButton)
            {
                currentButton = startButton;
                HighlightButton(currentButton);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentButton == startButton)
            {
                currentButton = endButton;
                HighlightButton(currentButton);
            }
        }

        // Enterキーで選択中のボタンを実行
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (currentButton == startButton)
            {
                StartGame();
            }
            else if (currentButton == endButton)
            {
                QuitGame();
            }
        }
    }

    void HighlightButton(Button button)
    {
        // すべてのボタンの文字色をリセット
        ResetButtonColors();

        // 現在選択中のボタンの文字色を変更
        Text buttonText = button.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.color = Color.red; // 選択中の文字色
        }

        // ボタンを選択状態にする
        button.Select();
    }

    void ResetButtonColors()
    {
        // STARTボタンの文字色をデフォルトに戻す
        Text startText = startButton.GetComponentInChildren<Text>();
        if (startText != null)
        {
            startText.color = Color.black; // デフォルトの文字色
        }

        // ENDボタンの文字色をデフォルトに戻す
        Text endText = endButton.GetComponentInChildren<Text>();
        if (endText != null)
        {
            endText.color = Color.black; // デフォルトの文字色
        }
    }


    void StartGame()
    {
        // メインシーンに遷移
        SceneManager.LoadScene("Main");
    }

    void QuitGame()
    {
        // ゲームを終了
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // エディタ内での終了処理
#endif
    }
}
