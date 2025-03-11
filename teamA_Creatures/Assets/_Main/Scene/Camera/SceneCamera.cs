using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    private float twoDistance;
    private Camera mainCamera;
    public float cameraJump = 0.3f;
    public float cameraFuttobi = 3f;
    public float smallestZoom = 30f;
    private Vector3 posi;
    private Vector3 startPosi;

    private void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        posi = this.transform.position;
        startPosi = this.transform.position;
    }
    // Update is called once per frame
    private void Update()
    {
        twoDistance = player.transform.position.x - enemy.transform.position.x;
        if (twoDistance < 0 )
        {
            twoDistance *= -1;
        }
        mainCamera.fieldOfView = twoDistance+smallestZoom+(enemy.transform.position.y*cameraFuttobi);
        posi.y = player.transform.position.y * cameraJump + startPosi.y;
        posi.x = (player.transform.position.x + enemy.transform.position.x) / 2;
        this.transform.position = posi;
    }
}
