using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayCast : MonoBehaviour
{
    public float rayDistance = 3f; // レイの長さ
    public float backRayDistance = 5f; // レイの長さ
    public float pushRayDistance = 3f; // レイの長さ
    float downRayDistance = 1f;
    public static bool isTackleWall = false;
    public static bool isBackWallSmash = false;
    public static bool isBackWall = false;
    float BackWallTime = 0f;


    public static bool isGround = false;

    private void Start()
    {
        isTackleWall = false;
        isBackWall = false;
        isBackWallSmash = false;
        BackWallTime = 0f;
    }
    void Update()
    {
        // 前判定
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

        // 後ろ判定
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

        // 飛ばされ時の後ろ判定
        RaycastHit pushBackward;
        if (Physics.Raycast(transform.position, -transform.forward, out pushBackward, pushRayDistance))
        {
            if (pushBackward.collider.CompareTag("Wall"))
            {
                NewEnemyMove.Instance.isPushWall = true;
            }
        }

        // 床判定
        RaycastHit hitGround;
        if (Physics.Raycast(transform.position, Vector3.down, out hitGround, downRayDistance))
        {
            if (hitGround.collider.CompareTag("Ground"))
            {
                isGround = true;
            }
            else
            {
                isGround = false;
            }
        }
        else
        {
            isGround = false;
        }

        // デバッグ用のレイの可視化
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);  // 前方（赤）
        Debug.DrawRay(transform.position, -transform.forward * backRayDistance, Color.blue); // 後方（青）
    }
}
