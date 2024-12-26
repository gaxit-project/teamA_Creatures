using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Move : MonoBehaviour
{
    public static Move Instance;
    /// <summary>
    /// ���̍U�������Ă��邩������₷�����Ă���
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public  Vector2 move;
    public float moveX;


    public float Speed;

    public float DeadZone = 0.1f;

    public float time = 0f;
    public float isWait;
    public bool attackMotion;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        attackMotion = false;
    }
    /// <summary>
    /// inputaction�������Ă��Ă���
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        
        move = context.ReadValue<Vector2>();
        if (!Attack.Instance.attackNow)
        {
            StartCoroutine(Pending());
        }
    }
    /// <summary>
    /// �ړ����邩�U�����邩
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pending()
    {
        time = 0;
        while (isWait > time)
        {
            if (Attack.Instance.attackNow)
            {
                yield break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        if (!Attack.Instance.attackNow)
        {
            animator.SetBool("run", true);
            moveX = move.x;
        }

        if (Mathf.Abs(moveX) < DeadZone)
        {
            animator.SetBool("run", false);
            moveX = 0; // �����Ȓl�𖳎�
        }
    }

    /// <summary>
    /// �ǂ̍U��������̂�
    /// </summary>

    /// <summary>
    /// �U���������I��点��
    /// </summary>

    private void Update()
    {
        if(moveX!=0) Attack.Instance.left = moveX > 0 ? false : true;

        if (Attack.Instance.left)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        if (!Attack.Instance.attackNow)
        {
        transform.Translate(transform.TransformDirection(new Vector2(-moveX, 0)*Speed*Time.deltaTime));
        }
        if (moveX == 0)
        {
            attackMotion = false;
        }
    }
}
