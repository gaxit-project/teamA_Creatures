using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseObject;
    public Button continueButton;
    public Button settingButton;
    public Button titleButton;

    // Start is called before the first frame update
    void Start()
    {
        continueButton.onClick.AddListener(Continue);
        settingButton.onClick.AddListener(Setting);
        titleButton.onClick.AddListener(Title);
        pauseObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Continue()
    {
        pauseObject.SetActive(false);
    }

    public void Setting()
    {

    }

    public void Title()
    {
        SceneManager.LoadScene("Title");
    }
}
