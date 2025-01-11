using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public GameObject hitJudgmentPrefab; // CubeのPrefabをアタッチ
    private GameObject activeCube;

    // Cubeを表示
    public void ShowPunchCube()
    {
        if (hitJudgmentPrefab != null)
        {
            if (activeCube == null)
            {
                // プレイヤーの前方にCubeを生成
                Vector3 spawnPosition = transform.position + transform.forward * 1.5f; // 前方に1.5f
                spawnPosition.y += 2.2f; // Y座標を2上げる
                activeCube = Instantiate(hitJudgmentPrefab, spawnPosition, Quaternion.identity);

                SetupHitDetection(); // 当たり判定をセットアップ
            }
            else
            {
                activeCube.SetActive(true);
                Vector3 updatedPosition = transform.position + transform.forward * 1.5f; // 前方に1.5f
                updatedPosition.y += 2.2f; // Y座標を2上げる
                activeCube.transform.position = updatedPosition;
            }
        }
    }


    // Cubeを非表示
    public void HidePunchCube()
    {
        if (activeCube != null)
        {
            activeCube.SetActive(false);
        }
    }

    // 当たり判定のセットアップ
    private void SetupHitDetection()
    {
        if (activeCube != null)
        {
            Collider collider = activeCube.GetComponent<Collider>();
            if (collider == null)
            {
                collider = activeCube.AddComponent<BoxCollider>(); // BoxColliderを追加
            }
            collider.isTrigger = true; // トリガーとして設定

            // トリガースクリプトを追加
            if (activeCube.GetComponent<HitDetection>() == null)
            {
                activeCube.AddComponent<HitDetection>();
            }
        }
    }
}
