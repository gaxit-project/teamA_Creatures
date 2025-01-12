using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    public static Move Instance;

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
    public float moveY;
    public float Speed;
    public float DeadZone = 0.1f;
    public float time = 0f;
    public float isWait;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
        if (!Attack.Instance.attackNow)
        {
            moveX = move.x;
            moveY = move.y;
            if (Jump.Instance.JumpFlag)
            {
                if (Mathf.Abs(moveX) > Mathf.Abs(moveY))
                {
                    animator.SetBool("run", true);
                }
                else
                {
                    animator.SetBool("Shield", true);
                }
            }
            if (moveX != 0)
            {
                Attack.Instance.left = moveX < 0;
            }
        }

        if (Mathf.Abs(moveX) < DeadZone)
        {
            moveX = 0; // 微小な値を無視
            animator.SetBool("run", false);
        }
        if (Mathf.Abs(moveY) < DeadZone)
        {
            moveY = 0;
            animator.SetBool("Shield", false);
        }
    }



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
