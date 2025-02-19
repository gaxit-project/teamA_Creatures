using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayCast : MonoBehaviour
{
    public float rayDistance = 3f; // レイの長さ
    public float backRayDistance = 5f; // レイの長さ
    public static bool isTackleWall = false;
    public static bool isBackWallSmash = false;
    public static bool isBackWall = false;
    float BackWallTime = 0f;

    private void Start()
    {
        isTackleWall = false;
        isBackWall = false;
        isBackWallSmash = false;
        BackWallTime = 0f;
    }
    void Update()
    {
        RaycastHit hitForward;
        if (Physics.Raycast(transform.position, transform.forward, out hitForward, rayDistance)) // layerMaskなし
        {
            if (hitForward.collider.CompareTag("Wall"))
            {
                Debug.Log("へーすごいですね！");
                if (NewEnemyMove._isTackle)
                {
                    isTackleWall = true;
                }
            }
        }


        RaycastHit hitBackward;
        if (Physics.Raycast(transform.position, -transform.forward, out hitBackward, backRayDistance))
        {
            if (hitBackward.collider.CompareTag("Wall"))
            {
                Debug.Log("ここはかべだよーーーー");
                isBackWall = true;
                if(!isBackWallSmash)
                {
                    BackWallTime += Time.deltaTime;
                }
                if(BackWallTime >= 10f)
                {
                    isBackWallSmash = true;
                    BackWallTime = 0f;
                }
            }
            else
            {
                isBackWall = false;
                BackWallTime = 0f;
            }
        }

        // デバッグ用のレイの可視化
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);  // 前方（赤）
        Debug.DrawRay(transform.position, -transform.forward * backRayDistance, Color.blue); // 後方（青）
    }
}
