using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneCamera : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;
    private float twoDistance;
    private Vector3 center;
    private Camera tackleCamera;
    private void Start()
    {
        tackleCamera = this.GetComponent<Camera>();
        center = this.transform.position;
    }

    private void Update()
    {
        twoDistance = player.transform.position.x - enemy.transform.position.x;
        if (twoDistance > 0 )
        {
            twoDistance *= -1;
        }
        //tackleCamera.fieldOfView = 60 - 60 / (1 + twoDistance);
        center.x = (player.transform.position.x + enemy.transform.position.x) / 2;
        this.transform.position = center;
    }
}
