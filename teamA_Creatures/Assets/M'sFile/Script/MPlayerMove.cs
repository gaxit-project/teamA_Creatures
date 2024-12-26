using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlayerMove : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] float _jumpForce = 5f; // ジャンプ力
    float _speed = 5f; // 移動速度
    private Rigidbody _rb;
    private bool _isGrounded; // 接地しているかどうかの判定用
    [SerializeField] LayerMask groundLayer; // 地面レイヤー


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-4, -0.6f, 0);
        // Rigidbody2D を取得
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        // 水平方向の入力を取得
        float moveInput = Input.GetAxis("Horizontal");

        Vector3 newPosition = _rb.position + new Vector3(moveInput * _speed * Time.deltaTime, 0, 0);
        _rb.MovePosition(newPosition);

        // プレイヤーの向きを変更
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1.8f, 1); // 右向き
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1.8f, 1); // 左向き
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

    }

    /// <summary>
    /// FixedUpdateで接地判定を行う
    /// </summary>
    private void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.1f);
    }
}
