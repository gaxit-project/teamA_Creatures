using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public GameObject attackCubePrefab; // キューブのPrefab
    public float hitActiveTime = 0.2f;//攻撃判定のある時間

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    IEnumerator ShortAttack(Vector3 offset, Vector3 size)
    {
        
        GameObject attackCube = Instantiate(attackCubePrefab, transform.position + offset, Quaternion.identity);//キューブを生成

        // サイズを設定
        attackCube.transform.localScale = size;

        yield return new WaitForSeconds(hitActiveTime);

        // 一定時間後にキューブを破壊
        Destroy(attackCube);
    }

    public void Punch()
    {
        Vector3 offset = new Vector3(-1, 0, 0); // エネミーの前方1ユニット
        Vector3 size = new Vector3(1, 1, 1); // 小さなキューブ
        StartCoroutine(ShortAttack(offset, size));
    }

    public void HardPunch()
    {
        Vector3 offset = new Vector3(-1, 0, 0);
        Vector3 size = new Vector3(-2, 1, 1);
        StartCoroutine(ShortAttack(offset, size));
    }

    public void Tackle()
    {
        Vector3 offset = new Vector3(-1, 0, 0);
        Vector3 size = new Vector3(-1, 3, 1);
        StartCoroutine(ShortAttack(offset, size));
    }
}
