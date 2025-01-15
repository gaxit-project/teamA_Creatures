using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject attackCubePrefab; // キューブのPrefab
    public float hitActiveTime = 0.2f;//攻撃判定のある時間
    private bool _isAttacking = false;
    private List<GameObject> attackCubes = new List<GameObject>(); // 攻撃キューブのリスト

    public void DestroyAttackCube(GameObject attackCube)
    {
        attackCubes.Remove(attackCube);
        Destroy(attackCube);  // キューブを削除
        Debug.Log("攻撃キューブを削除しました");
    }

    public IEnumerator ShortAttack(Vector3 spawnPosition, Vector3 size)
    {
        if (_isAttacking) yield break; // 実行中の場合は処理を終了

        _isAttacking = true; // 実行中フラグを立てる

        Debug.Log("攻撃キューブを生成します");

        GameObject newAttackCube = Instantiate(attackCubePrefab, spawnPosition, Quaternion.identity);//キューブを生成

        attackCubes.Add(newAttackCube);

        // サイズを設定
        newAttackCube.transform.localScale = size;

        yield return new WaitForSeconds(hitActiveTime);

        attackCubes.Remove(newAttackCube);
        DestroyAttackCube(newAttackCube);  // 作成したキューブを削除

        _isAttacking = false; // 実行中フラグを解除
    }

    public void Punch()
    {
        Vector3 offset = transform.forward * 1.0f; // エネミーの前方1ユニット
        // y軸の位置を現在の位置に設定して高さを保持
        Vector3 spawnPosition = transform.position + offset;
        spawnPosition.y = 2.0f; // 高さ2の位置で生成
        Vector3 size = new Vector3(2, 1, 1); // 小さなキューブ
        StartCoroutine(ShortAttack(spawnPosition, size));
    }

    public void HardPunch()
    {
        Vector3 offset = transform.forward * 1.0f; // エネミーの前方1ユニット
        // y軸の位置を現在の位置に設定して高さを保持
        Vector3 spawnPosition = transform.position + offset;
        spawnPosition.y = 2.0f; // 高さ2の位置で生成
        Vector3 size = new Vector3(-2, 1, 1);
        StartCoroutine(ShortAttack(offset, size));
    }

    public void Tackle()
    {
        Vector3 offset = transform.forward * 1.0f; // エネミーの前方1ユニット
        // y軸の位置を現在の位置に設定して高さを保持
        Vector3 spawnPosition = transform.position + offset;
        spawnPosition.y = 2.0f; // 高さ2の位置で生成
        Vector3 size = new Vector3(-1, 3, 1);
        StartCoroutine(ShortAttack(offset, size));
    }
}
