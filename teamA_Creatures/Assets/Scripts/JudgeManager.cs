using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JudgeManager : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ClearJudge());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeOverScene()
    {
        SceneManager.LoadScene("GameOver");
    }

    void ChangeClearScene()
    {
        SceneManager.LoadScene("GameClear");
    }

}
