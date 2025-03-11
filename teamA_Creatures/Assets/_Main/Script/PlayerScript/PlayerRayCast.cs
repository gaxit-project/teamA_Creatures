using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    public float playerBackRayDistance = 4f; // レイの長さ
    public float playerRayDistance = 1f; // レイの長さ
    public static bool isPlayerBackWall = false;
    public static bool isPlayerWall = false;
    float BackWallTime = 0f;

    private void Start()
    {
        isPlayerBackWall = false;
        isPlayerWall = false;
        BackWallTime = 0f;
    }
    void Update()
    {
        RaycastHit hitBackward;
        if (Physics.Raycast(transform.position, -transform.forward, out hitBackward, playerBackRayDistance))
        {
            if (hitBackward.collider.CompareTag("Wall"))
            {
                isPlayerBackWall = true;
            }
            else
            {
                isPlayerBackWall = false;
            }
        }
        RaycastHit hitsBackward;
        if (Physics.Raycast(transform.position, -transform.forward, out hitsBackward, playerRayDistance))
        {
            if (hitsBackward.collider.CompareTag("Wall"))
            {
                isPlayerWall = true;
            }
            else
            {
                isPlayerWall = false;
            }
        }
        Debug.DrawRay(transform.position, -transform.forward * playerBackRayDistance, Color.blue); // 後方（青）
    }
}
