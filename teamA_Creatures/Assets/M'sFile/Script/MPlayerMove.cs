using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPlayerMove : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] float _jumpForce = 5f; // �W�����v��
    float _speed = 5f; // �ړ����x
    private Rigidbody _rb;
    private bool _isGrounded; // �ڒn���Ă��邩�ǂ����̔���p
    [SerializeField] LayerMask groundLayer; // �n�ʃ��C���[


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-4, -0.6f, 0);
        // Rigidbody2D ���擾
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionZ |
                         RigidbodyConstraints.FreezeRotationX |
                         RigidbodyConstraints.FreezeRotationY;
    }

    // Update is called once per frame
    void Update()
    {
        // ���������̓��͂��擾
        float moveInput = Input.GetAxis("Horizontal");

        Vector3 newPosition = _rb.position + new Vector3(moveInput * _speed * Time.deltaTime, 0, 0);
        _rb.MovePosition(newPosition);

        // �v���C���[�̌�����ύX
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1.8f, 1); // �E����
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1.8f, 1); // ������
        }
        if (Input.GetKey(KeyCode.Space))
        {
            _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        }

    }

    /// <summary>
    /// FixedUpdate�Őڒn������s��
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
