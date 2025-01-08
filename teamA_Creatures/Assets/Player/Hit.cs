using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hit : MonoBehaviour
{
    public void PunchHitCheck()
    {
        // 攻撃範囲をCubeのColliderで定義していると仮定
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, transform.rotation);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy")) // 敵に衝突した場合
            {
                Debug.Log("パンチが敵にヒットしました！: " + hitCollider.name);
            }
        }
    }

    private void OnDrawGizmos()
    {
        // オーバーラップボックスの範囲を視覚化（デバッグ用）
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }
}
