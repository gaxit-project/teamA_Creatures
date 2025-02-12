using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JudgeManager : MonoBehaviour
{

    public void ChangeOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void ChangeClearScene()
    {
        SceneManager.LoadScene("GameClear");
    }

    public void ChangeMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void ChangeTitleScene()
    {
        SceneManager.LoadScene("Title");
    }


}
