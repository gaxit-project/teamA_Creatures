using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackbox : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return; // プレイヤー以外なら処理を終了

        Debug.Log("プレイヤーに攻撃が当たった");
    }
}
