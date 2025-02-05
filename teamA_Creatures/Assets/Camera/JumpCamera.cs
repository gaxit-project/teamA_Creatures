using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCamera : MonoBehaviour
{
    public GameObject player;
    public float cameraJump = 0.3f;
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
        posi.y = player.transform.position.y * cameraJump + startPosi.y;
        this.transform.position = posi;
    }
}
