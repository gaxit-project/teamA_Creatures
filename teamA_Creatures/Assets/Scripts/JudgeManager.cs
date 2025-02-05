using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JudgeManager : MonoBehaviour
{

    void ChangeOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    void ChangeClearScene()
    {
        SceneManager.LoadScene("GameClear");
    }

}
