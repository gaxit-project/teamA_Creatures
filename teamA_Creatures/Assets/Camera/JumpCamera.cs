using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCamera : MonoBehaviour
{
    public GameObject player;
    private Vector3 posi;
    private Vector3 startPosi;

    private void Start()
    {
        posi = this.transform.position;
        startPosi = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        posi.y = (float)(player.transform.position.y * 0.3) + startPosi.y;
        this.transform.position = posi;
    }
}
