using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public static Move Instance;
    /// <summary>
    /// 何の攻撃をしているか分かりやすくしている
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

    public Vector2 move;
    public float moveX;


    public float Speed;

    public float DeadZone = 0.1f;

    public float time = 0f;
    public float isWait;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    /// <summary>
    /// inputactionから取ってきている
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        // ゲームオブジェクトがアクティブである場合のみコルーチンを開始
        if (!Attack.Instance.attackNow && gameObject.activeInHierarchy)
        {
            StartCoroutine(Pending());
        }

    }
    /// <summary>
    /// 移動するか攻撃するか
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
            moveX = move.x;
            if (Jump.Instance.JumpFlag)
            {
                animator.SetBool("run", true);
            }
            if (moveX != 0)
            {
                Attack.Instance.left = moveX < 0;
            }

        }

        if (Mathf.Abs(moveX) < DeadZone)
        {
            moveX = 0; // 微小な値を無視
            animator.SetBool("run",false);
        }
    }

    /// <summary>
    /// どの攻撃をするのか
    /// </summary>

    /// <summary>
    /// 攻撃処理を終わらせる
    /// </summary>

    private void Update()
    {


        // 向きの変更を適用
        if (Attack.Instance.left)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }

        // 攻撃中でない場合は移動
        if (!Attack.Instance.attackNow)
        {
            transform.Translate(transform.TransformDirection(new Vector2(-moveX, 0) * Speed * Time.deltaTime));
        }

    }
}
