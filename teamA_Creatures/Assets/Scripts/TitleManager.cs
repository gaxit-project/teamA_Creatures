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

    private float joystickInputCooldown = 0.2f; // ジョイスティック入力のクールダウンタイム
    private float joystickInputTimer = 0f; // クールダウン用タイマー

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
        // タイマーを更新
        joystickInputTimer += Time.deltaTime;

        // ジョイスティックの入力を取得
        float joystickY = Input.GetAxis("Vertical");

        // ジョイスティック上方向
        if (joystickY > 0.5f && joystickInputTimer >= joystickInputCooldown)
        {
            if (currentButton == endButton)
            {
                currentButton = startButton;
                HighlightButton(currentButton);
            }
            joystickInputTimer = 0f;
        }
        // ジョイスティック下方向
        else if (joystickY < -0.5f && joystickInputTimer >= joystickInputCooldown)
        {
            if (currentButton == startButton)
            {
                currentButton = endButton;
                HighlightButton(currentButton);
            }
            joystickInputTimer = 0f;
        }

        // Bボタンで選択中のボタンを実行
        if (Input.GetButtonDown("Cancel"))
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
