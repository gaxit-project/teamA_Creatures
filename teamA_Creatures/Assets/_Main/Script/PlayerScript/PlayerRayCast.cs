using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRayCast : MonoBehaviour
{
    public float playerBackRayDistance = 4f; // ÉåÉCÇÃí∑Ç≥
    public static bool isPlayerBackWall = false;
    float BackWallTime = 0f;

    private void Start()
    {
        isPlayerBackWall = false;
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
        Debug.DrawRay(transform.position, -transform.forward * playerBackRayDistance, Color.blue); // å„ï˚Åiê¬Åj
    }
}
