using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class resulutButton : MonoBehaviour
{
    public Button titleButton;
    public Button retryButton;
    public JudgeManager JM;
    // Start is called before the first frame update
    void Start()
    {
        titleButton.onClick.AddListener(GoToTitle);
        retryButton.onClick.AddListener(GoToRetry);
        retryButton.Select();
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene("Title");
    }
    public void GoToRetry()
    {
        SceneManager.LoadScene("Loading");
    }

    private void Update()
    {
        GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

        if (selectedObj == null)
        {
            retryButton.Select();
        }
    }
}
