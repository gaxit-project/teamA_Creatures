using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRayCast : MonoBehaviour
{
    public float rayDistance = 3f; // レイの長さ
    public float rayFowardDistance = 9f;
    public float backRayDistance = 5f; // レイの長さ
    public float pushRayDistance = 3f; // レイの長さ
    float downRayDistance = 1f;
    public static bool isTackleWall = false;
    public static bool isBackWallSmash = false;
    public static bool isBackWall = false;
    public static bool isFowardWall = false;
    public static bool isFowardPlayer = false;
    float BackWallTime = 0f;


    public static bool isGround = false;

    private void Start()
    {
        isTackleWall = false;
        isBackWall = false;
        isBackWallSmash = false;
        isFowardWall = false;
        isFowardPlayer = false;
        BackWallTime = 0f;
    }
    void Update()
    {

        Vector3 rayOrigin = transform.position + new Vector3(0, 1.5f, 0);


        // 前方向のレイキャスト（すべてのオブジェクトを取得）
        RaycastHit[] hitsForward = Physics.RaycastAll(transform.position, transform.forward, rayDistance);
        if (hitsForward.Length > 0)
        {
            for (int i = 0; i < hitsForward.Length; i++)
            {
                if (hitsForward[i].collider.CompareTag("Wall"))
                {
                    if (NewEnemyMove._isTackle)
                    {
                        isTackleWall = true;
                    }
                }
            }
        }



        RaycastHit[] hitForward = Physics.RaycastAll(rayOrigin, transform.forward, rayFowardDistance);
        if (hitForward.Length > 0)
        {
            for (int i = 0; i < hitForward.Length; i++)
            {
                if (hitForward[i].collider.CompareTag("Wall"))
                {
                    Debug.Log("壁際処理です");
                    isFowardWall = true;
                }
                else
                {
                    Debug.Log("壁際じゃないです");
                    isFowardWall = false;
                }

                if (hitForward[i].collider.CompareTag("Player"))
                {
                    isFowardPlayer = true;
                    Debug.Log("目の前にプレイヤーがいます");
                }
                else
                {
                    isFowardPlayer = false;
                }
            }
        }

        // 後ろ方向のレイキャスト（すべてのオブジェクトを取得）
        RaycastHit[] hitsBackward = Physics.RaycastAll(transform.position, -transform.forward, backRayDistance);
        bool foundBackWall = false;
        if (hitsBackward.Length > 0)
        {
            for (int i = 0; i < hitsBackward.Length; i++)
            {
                if (hitsBackward[i].collider.CompareTag("Wall"))
                {
                    foundBackWall = true;
                    if (!isBackWallSmash)
                    {
                        BackWallTime += Time.deltaTime;
                    }
                }
            }
        }
        isBackWall = foundBackWall;
        if (!foundBackWall)
        {
            BackWallTime = 0f;
        }
        if (BackWallTime >= 10f)
        {
            isBackWallSmash = true;
            BackWallTime = 0f;
        }

        // 飛ばされ時の後ろ判定
        RaycastHit[] pushBackwardHits = Physics.RaycastAll(transform.position, -transform.forward, pushRayDistance);
        if (pushBackwardHits.Length > 0)
        {
            for (int i = 0; i < pushBackwardHits.Length; i++)
            {
                if (pushBackwardHits[i].collider.CompareTag("Wall"))
                {
                    NewEnemyMove.Instance.isPushWall = true;
                }
            }
        }

        // 床判定（すべてのオブジェクトを取得）
        RaycastHit[] groundHits = Physics.RaycastAll(transform.position, Vector3.down, downRayDistance);
        bool foundGround = false;
        if (groundHits.Length > 0)
        {
            for (int i = 0; i < groundHits.Length; i++)
            {
                if (groundHits[i].collider.CompareTag("Ground"))
                {
                    foundGround = true;
                }
            }
        }
        isGround = foundGround;

        // デバッグ用のレイの可視化
        Debug.DrawRay(transform.position, transform.forward * rayDistance, Color.red);
        Debug.DrawRay(transform.position, transform.forward * rayFowardDistance, Color.green);
        Debug.DrawRay(transform.position, -transform.forward * backRayDistance, Color.blue);
    }

}
