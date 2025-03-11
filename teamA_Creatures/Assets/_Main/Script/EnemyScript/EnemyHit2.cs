using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHit2 : MonoBehaviour
{
    public GameObject attackCubePrefab; // キューブのPrefab
    private List<GameObject> attackCubes = new List<GameObject>();
    private GameObject attackCube; // 削除したいキューブを保持する変数

    private Vector3 offset = new Vector3(0, 2.0f, 1.5f); // キューブの初期オフセット位置

    public void DestroyCube()
    {
        if (attackCube != null)
        {
            Destroy(attackCube);
            //attackCube.SetActive(false);
        }
    }


    private void Update()
    {
        if (attackCube != null && attackCube.activeSelf)
        {
            // キャラクターの位置に基づいてキューブの位置を更新
            Vector3 updatedPosition = transform.position + transform.forward * offset.z;
            updatedPosition.y = transform.position.y + offset.y;
            attackCube.transform.position = updatedPosition;
        }
    }

    public void RightPunch()
    {
        if (attackCubePrefab != null)
        {

            if (attackCube == null)
            {

                RightPunchCreateCube();

            }
            //else
            //{
            //    attackCube.SetActive(true);
            //}
            // キューブが生成済みかつオブジェクトが動いている場合、キューブを再生成
            // 前回位置を更新

        }
    }

    private void RightPunchCreateCube()
    {
        Vector3 spawnPosition = transform.position + transform.forward * offset.z;
        spawnPosition.y = transform.position.y + offset.y;
        attackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity);
        attackCube.transform.localScale = new Vector3(1, 1, 1); // サイズ設定
        attackCube.tag = "EnemyRightPunchAttack";
    }

    public void LeftPunch()
    {
        if (attackCubePrefab != null)
        {

            if (attackCube == null)
            {

                LeftPunchCreateCube();

            }
            //else
            //{
            //    attackCube.SetActive(true);
            //}
            // キューブが生成済みかつオブジェクトが動いている場合、キューブを再生成
            // 前回位置を更新

        }
    }

    private void LeftPunchCreateCube()
    {
        Vector3 spawnPosition = transform.position + transform.forward * offset.z;
        spawnPosition.y = transform.position.y + offset.y;
        attackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity);
        attackCube.transform.localScale = new Vector3(1, 1, 1); // サイズ設定
        attackCube.tag = "EnemyLeftPunchAttack";
    }

    public void GuardBreak()
    {
        if (attackCubePrefab != null)
        {

            if (attackCube == null)
            {

                GuardBreakCreateCube();

            }
            //else
            //{
            //    attackCube.SetActive(true);
            //}
            // キューブが生成済みかつオブジェクトが動いている場合、キューブを再生成
            // 前回位置を更新

        }
    }

    private void GuardBreakCreateCube()
    {
        Vector3 spawnPosition = transform.position + transform.forward * offset.z;
        spawnPosition.y = transform.position.y + offset.y;
        attackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity);
        attackCube.transform.localScale = new Vector3(1, 1, 1); // サイズ設定
        attackCube.tag = "EnemyGuardBreakAttack";
    }



    public void HardPunch()
    {
        if (attackCubePrefab != null)
        {
            if (attackCube == null)
            {

                HardPunchCreateCube();

            }
            //else
            //{
            //    attackCube.SetActive(true);
            //}

        }
    }

    private void HardPunchCreateCube()
    {
        Vector3 spawnPosition = transform.position + transform.forward * offset.z;
        spawnPosition.y = transform.position.y + offset.y;
        attackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity);
        attackCube.transform.localScale = new Vector3(3, 1, 1); // サイズ設定
        attackCube.tag = "EnemySmashAttack";
    }

    public void TackleCube()
    {
        if (attackCubePrefab != null)
        {
            if (attackCube == null)
            {

                TackleCreateCube();

            }
            //else
            //{
            //    attackCube.SetActive(true);
            //}

        }
    }

    private void TackleCreateCube()
    {
        Vector3 spawnPosition = transform.position + transform.forward * offset.z;
        spawnPosition.y = transform.position.y + offset.y;
        attackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity);
        attackCube.transform.localScale = new Vector3(3f, 4, 1); // サイズ設定
        attackCube.tag = "EnemyTackleAttack";
    }
}


