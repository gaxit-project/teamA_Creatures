using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class resulutButton : MonoBehaviour
{
    public Button titleButton;
    public Button retryButton;
    public JudgeManager JM;
    // Start is called before the first frame update
    void Start()
    {
        retryButton.Select();
    }

    public void GoToTitle()
    {
        JM.ChangeTitleScene();
    }
    public void GoToRetry()
    {
        JM.ChangeMainScene();
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
