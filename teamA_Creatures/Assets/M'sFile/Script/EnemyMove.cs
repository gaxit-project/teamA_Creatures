using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 4; // 敵の動くスピード
    [SerializeField] float backSpeed = 3; // 敵の動くスピード

    public bool isFollow = false; // 追従するかどうかのフラグ

    Transform _playerTr; // プレイヤーのTransform

    Rigidbody _rb; // このオブジェクトの Rigidbody

    float _distanceAway = 5f; // プレイヤーから離れる距離
    float _distanceApp = 1.5f; // プレイヤーに近づく距離

   
    float _distancePtoE;  // エネミーとプレイヤーの距離を入れる変数


    void Start()
    {
        transform.position = new Vector3(4, 0, 0);
        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        // リジットボディの設定
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        // エネミーとプレイヤーの距離計測
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
    }


    void Update()
    {
        // 互いの距離計測
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("isFollow" + isFollow);
            isFollow = true;
        }
        else if(Input.GetKey(KeyCode.X))
        {
            Debug.Log("isFollow" + isFollow);
            isFollow = false;
        }
        if(isFollow)
        {
            PlayerBackFollow();
        }
        else
        {
            PlayerFollow();
        }

    }



    /// <summary>
    /// プレイヤーを追従する関数
    /// </summary>
    public void PlayerFollow()
    {
        // プレイヤーとの距離が指定の距離になったら実行しない
        if (_distancePtoE < _distanceApp)
        {
            return;
        }
        // プレイヤーに向けて進む
        transform.position = Vector2.MoveTowards(
                            transform.position,
                            new Vector2(_playerTr.position.x, transform.position.y), // X軸だけプレイヤーに追従
                            speed * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤーから逃げる関数
    /// </summary>
    public void PlayerBackFollow()
    {
        // 指定の距離離れるまで実行
        if (_distancePtoE < _distanceAway)
        {
            // プレイヤーから逃げる方向を計算
            Vector3 directionAwayFromPlayer = (transform.position - _playerTr.position).normalized;
            // 逆方向に移動
            Vector3 newPosition = transform.position + (directionAwayFromPlayer * backSpeed * Time.deltaTime);
            _rb.MovePosition(newPosition);
        }
    }
}
