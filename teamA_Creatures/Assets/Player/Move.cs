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
            animator.SetBool("run", true);
            moveX = move.x;
        }

        if (Mathf.Abs(moveX) < DeadZone)
        {
            animator.SetBool("run", false);
            moveX = 0; // 微小な値を無視
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
