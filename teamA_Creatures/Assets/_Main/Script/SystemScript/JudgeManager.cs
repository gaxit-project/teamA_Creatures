using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JudgeManager : MonoBehaviour
{
    public KnockOutEffect knockOutEffect;

    void Start()
    {
        if (knockOutEffect == null)
        {
            knockOutEffect = FindObjectOfType<KnockOutEffect>();
        }
    }

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

    public void LoseEffect()
    {
        knockOutEffect.ShowLoseEffect();
    }

    public void WinEffect()
    {
        knockOutEffect.ShowWinEffect();
    }


}
