using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 4; // �G�̓����X�s�[�h
    [SerializeField] float backSpeed = 3; // �G�̓����X�s�[�h

    public bool isFollow = false; // �Ǐ]���邩�ǂ����̃t���O

    Transform _playerTr; // �v���C���[��Transform

    Rigidbody _rb; // ���̃I�u�W�F�N�g�� Rigidbody

    float _distanceAway = 5f; // �v���C���[���痣��鋗��
    float _distanceApp = 1.5f; // �v���C���[�ɋ߂Â�����

   
    float _distancePtoE;  // �G�l�~�[�ƃv���C���[�̋���������ϐ�


    void Start()
    {
        transform.position = new Vector3(4, 0, 0);
        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        // ���W�b�g�{�f�B�̐ݒ�
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        // �G�l�~�[�ƃv���C���[�̋����v��
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
    }


    void Update()
    {
        // �݂��̋����v��
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
    /// �v���C���[��Ǐ]����֐�
    /// </summary>
    public void PlayerFollow()
    {
        // �v���C���[�Ƃ̋������w��̋����ɂȂ�������s���Ȃ�
        if (_distancePtoE < _distanceApp)
        {
            return;
        }
        // �v���C���[�Ɍ����Đi��
        transform.position = Vector2.MoveTowards(
                            transform.position,
                            new Vector2(_playerTr.position.x, transform.position.y), // X�������v���C���[�ɒǏ]
                            speed * Time.deltaTime);
    }

    /// <summary>
    /// �v���C���[���瓦����֐�
    /// </summary>
    public void PlayerBackFollow()
    {
        // �w��̋��������܂Ŏ��s
        if (_distancePtoE < _distanceAway)
        {
            // �v���C���[���瓦����������v�Z
            Vector3 directionAwayFromPlayer = (transform.position - _playerTr.position).normalized;
            // �t�����Ɉړ�
            Vector3 newPosition = transform.position + (directionAwayFromPlayer * backSpeed * Time.deltaTime);
            _rb.MovePosition(newPosition);
        }
    }
}
