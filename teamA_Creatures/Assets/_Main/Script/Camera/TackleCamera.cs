using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleCamera : MonoBehaviour
{
    private Camera tackleCamera;
    private void Start()
    {
        tackleCamera = this.GetComponent<Camera>();
        tackleCamera.depth = -2;
    }


    public IEnumerator tackleCameraCor()
    {
        tackleCamera.depth = 1;
        Time.timeScale = 0.2f;
        tackleCamera.fieldOfView = 103f;
        yield return new WaitForSeconds(0.005f);
        tackleCamera.fieldOfView = 100f;
        yield return new WaitForSeconds(0.005f);
        tackleCamera.fieldOfView = 102f;
        yield return new WaitForSeconds(0.005f);
        tackleCamera.fieldOfView = 100f;
        yield return new WaitForSeconds(0.005f);
        tackleCamera.fieldOfView = 101f;
        yield return new WaitForSeconds(0.005f);
        tackleCamera.fieldOfView = 100f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1f;
        tackleCamera.depth = -2;
    }
}