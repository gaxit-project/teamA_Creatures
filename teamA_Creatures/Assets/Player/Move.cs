using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

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
        }

        if (Mathf.Abs(moveX) < DeadZone)
        {
            moveX = 0; // 微小な値を無視
        }
    }

    /// <summary>
    /// どの攻撃をするのか
    /// </summary>

    /// <summary>
    /// 攻撃処理を終わらせる
    /// </summary>

    public float turnDelay; // 後ろを向くまでの猶予時間
    private float turnTimer = 0f;  // 猶予時間を計測するタイマー
    private bool ChangeDirection = false; // 向きを変える必要があるか
    private bool isTurn = false;

    private void Update()
    {
        // スティック入力がある場合に向きを判定
        if (Mathf.Abs(moveX) > 0.1f) // DeadZone より大きい場合に処理
        {
            bool newDirection = moveX < 0; // moveX が負なら左向き

            if (newDirection != Attack.Instance.left)
            {
                if (!ChangeDirection)
                {
                    ChangeDirection = true; // 向き変更の準備開始
                    turnTimer = 0f; // タイマーをリセット
                    isTurn = true;
                }
            }
            else
            {
                ChangeDirection = false; // 向き変更の必要がなくなる
                isTurn = false;
            }
        }
        else
        {
            ChangeDirection = false; // スティック入力がなければ向き変更をリセット
            isTurn = false;
        }

        // タイマーを進める
        if (ChangeDirection)
        {
            turnTimer += Time.deltaTime;
            if (turnTimer >= turnDelay)
            {
                Attack.Instance.left = moveX < 0; // 向きを変更
                ChangeDirection = false;  // 処理完了
                isTurn = false;
            }
        }

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
        if (!Attack.Instance.attackNow && !isTurn)
        {
            transform.Translate(transform.TransformDirection(new Vector2(-moveX, 0) * Speed * Time.deltaTime));
        }

        if (!Attack.Instance.attackNow && Mathf.Abs(moveX) > DeadZone && !isTurn && Jump.Instance.JumpFlag)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
        }

    }
}
