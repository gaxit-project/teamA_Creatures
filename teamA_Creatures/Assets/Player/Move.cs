using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Move : MonoBehaviour
{
    public static bool attackNow = false;
    /// <summary>
    /// 何の攻撃をしているか分かりやすくしている
    /// </summary>
    public enum AttackType
    {
        UPAttack,Attack
    }
    AttackType attackType;

    public  Vector2 move;
    public float moveX;
    public float absoluteValueX;
    public float absoluteValueY;

    public float Speed;

    public float DeadZone = 0.1f;

    public float time = 0f;
    public float isWait;
    public bool left;
    public Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        left = false;
    }
    /// <summary>
    /// inputactionから取ってきている
    /// </summary>
    public void OnMove(InputAction.CallbackContext context)
    {
        
        move = context.ReadValue<Vector2>();
        if (!attackNow)
        {
            StartCoroutine(Pending());
        }
    }
    /// <summary>
    /// 攻撃判断
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pending()
    {
        time = 0;
        while (isWait > time)
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                if (move.x < 0)
                {
                    absoluteValueX = move.x*-1;
                }else
                {
                    absoluteValueX = move.x;
                }
                if (absoluteValueX > move.y) 
                {
                    attackType = AttackType.Attack;
                }
                else
                {
                    attackType = AttackType.UPAttack;
                }

                AttackPending();
                yield break;
            }
            time += Time.deltaTime;
            yield return null;
        }

        if (!attackNow)
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
    public void AttackPending()
    {
        animator.SetBool("run", false);
        moveX = 0;
        switch (attackType)
        {
            case AttackType.UPAttack:
                Debug.Log("上攻撃");
                //attackNow = true;
                break;
            case AttackType.Attack:
                if (left && move.x < 0)
                {
                    Debug.Log("前攻撃");
                    animator.SetBool("FrontAttack", true);
                    attackNow = true;
                }
                else if (left && move.x > 0) 
                {
                    Debug.Log("後ろ攻撃");
                    animator.SetBool("BuckAttack",true);
                    attackNow = false;
                }
                else if (!left && move.x > 0)
                {
                    Debug.Log("前攻撃");
                    animator.SetBool("FrontAttack", true);
                    attackNow = true;
                }
                else if(!left && move.x < 0)
                {
                    Debug.Log("後ろ攻撃");
                    animator.SetBool("BuckAttack", true);
                    attackNow = true;
                }
                break;
        }
    }
    /// <summary>
    /// 攻撃処理を終わらせる
    /// </summary>
    public void AttackEnd()
    {
        animator.SetBool("FrontAttack",false);
        animator.SetBool("BuckAttack",false );
        attackNow = false;
    }
    private void Update()
    {
        if(moveX!=0)left = moveX > 0 ? false : true;

        if (left)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        transform.Translate(transform.TransformDirection(new Vector2(-moveX, 0)*Speed*Time.deltaTime));
    }
}
