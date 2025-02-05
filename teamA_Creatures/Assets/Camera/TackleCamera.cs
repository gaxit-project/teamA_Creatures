using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine("tackleCamera");
        }
    }

    public IEnumerator tackleCamera()
    {
        this .gameObject.SetActive(true);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
        this .gameObject.SetActive(false);
    }
}
