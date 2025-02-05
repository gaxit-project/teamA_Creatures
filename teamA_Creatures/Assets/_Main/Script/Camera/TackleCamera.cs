using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleCamera : MonoBehaviour
{
    public static TackleCamera Instance;

    private static GameObject tCamera;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
    }

    private void Start()
    {
        tCamera = this.gameObject;
        this.gameObject.SetActive(false);
    }


    public static IEnumerator tackleCameraCor()
    {
        tCamera.gameObject.SetActive(true);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        tCamera.gameObject.SetActive(false);
    }
}
