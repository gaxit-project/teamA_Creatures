using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackbox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 敵タグを持つオブジェクトに当たった場合
        {
            Debug.Log("プレイヤーに攻撃が当たった");

            // 敵にダメージを与える処理
            // 例: other.GetComponent<Enemy>().TakeDamage(10);
        }
        else
        {
            Debug.Log("敵の攻撃が外れた");
        }
    }
}
