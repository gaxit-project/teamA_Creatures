using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public GameObject attackCubePrefab; // キューブのPrefab
    private bool _isAttacking = false;
    private List<GameObject> attackCubes = new List<GameObject>();
    private GameObject attackCube; // 削除したいキューブを保持する変数

    public void DestroyCube()
    {
        if (attackCube != null)
        {
            attackCube.SetActive(false);
        }
    }



    public void Punch()
    {
        if (attackCubePrefab != null)
        {
            if (attackCube == null)
            {
                // y軸の位置を現在の位置に設定して高さを保持
                Vector3 spawnPosition = transform.position + transform.forward * 1.0f;
                spawnPosition.y = 2.0f; // 高さ2の位置で生成
                Vector3 size = new Vector3(1, 1, 1); // 小さなキューブ
                attackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity); // キューブを生成
                attackCube.transform.localScale = size; // サイズを設定

            }
            else
            {
                attackCube.SetActive(true);
                Vector3 offset = transform.forward * 1.0f; // エネミーの前方1ユニット
                // y軸の位置を現在の位置に設定して高さを保持
                Vector3 updatedPosition = transform.position + offset;
                updatedPosition.y = 2.0f; // 高さ2の位置で生成
                Vector3 size = new Vector3(1, 1, 1); // 小さなキューブ
                attackCube.transform.localScale = size;
                attackCube.transform.position = updatedPosition;
            }
        }

    }

    public void HardPunch()
    {
        Vector3 offset = transform.forward * 1.0f; // エネミーの前方1ユニット
        // y軸の位置を現在の位置に設定して高さを保持
        Vector3 spawnPosition = transform.position + offset;
        spawnPosition.y = 2.0f; // 高さ2の位置で生成
        Vector3 size = new Vector3(-2, 1, 1);
    }

    public void Tackle()
    {
        Vector3 offset = transform.forward * 1.0f; // エネミーの前方1ユニット
        // y軸の位置を現在の位置に設定して高さを保持
        Vector3 spawnPosition = transform.position + offset;
        spawnPosition.y = 2.0f; // 高さ2の位置で生成
        Vector3 size = new Vector3(-1, 3, 1);
    }
}
