using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public Button startButton; // STARTボタン
    public Button endButton;   // ENDボタン
    private int selectedIndex = 0;
    private Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        // ボタンを配列にまとめる
        buttons = new Button[] { startButton, endButton };

        // 最初のボタンを選択状態にする
        SelectButton(selectedIndex);
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // 上方向の入力 (Wキーまたはジョイスティックの上)
        if (Input.GetKeyDown(KeyCode.W) || Input.GetAxis("Vertical") > 0.5f)
        {
            selectedIndex = (selectedIndex - 1 + buttons.Length) % buttons.Length;
            SelectButton(selectedIndex);
        }

        // 下方向の入力 (Sキーまたはジョイスティックの下)
        if (Input.GetKeyDown(KeyCode.S) || Input.GetAxis("Vertical") < -0.5f)
        {
            selectedIndex = (selectedIndex + 1) % buttons.Length;
            SelectButton(selectedIndex);
        }

        // 決定 (EnterキーまたはAボタン)
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            ExecuteButtonAction();
        }
    }

    private void SelectButton(int index)
    {
        // 現在選択されているボタンを EventSystem に設定
        EventSystem.current.SetSelectedGameObject(buttons[index].gameObject);
    }

    private void ExecuteButtonAction()
    {
        if (selectedIndex == 0)
        {
            // STARTボタンのアクション
            SceneManager.LoadScene("Main");
        }
        else if (selectedIndex == 1)
        {
            // ENDボタンのアクション
            Application.Quit();
            Debug.Log("Game Exited"); // デバッグ用（エディタではゲームが終了しないため）
        }
    }
}
