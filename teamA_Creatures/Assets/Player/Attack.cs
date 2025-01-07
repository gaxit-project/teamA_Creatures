using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Move;

public class Attack : MonoBehaviour
{
    public static Attack Instance;
    public Animator animator;
    public bool attackNow = false;
    public bool left;
    public float absoluteValueX;
    public float absoluteValueY;

    private float buttonHoldTime;
    public float ULTPressed = 0.5f;
    private bool isButtonPressed = false;

    public bool isGun;
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

    public enum AttackType
    {
        Attack, FrontAttack_WeaponChange, UPAttack, DownAttack, ULT
    }
    AttackType attackType;

    public void Start()
    {
        animator = GetComponent<Animator>();
        left = false;
        isGun = false;
    }
    private Coroutine buttonHoldCoroutine;
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) // ボタンを押した瞬間
        {
            isButtonPressed = true;
            buttonHoldTime = 0;

            if (buttonHoldCoroutine != null)
            {
                StopCoroutine(buttonHoldCoroutine); // 既存のコルーチンを停止
            }
            buttonHoldCoroutine = StartCoroutine(MeasureButtonHoldTime());
        }
        else if (context.canceled) // ボタンを離した瞬間
        {
            isButtonPressed = false;

            if (buttonHoldCoroutine != null)
            {
                StopCoroutine(buttonHoldCoroutine);
                buttonHoldCoroutine = null;
            }

            AttackMotion();
        }
    }

    private IEnumerator MeasureButtonHoldTime()
    {
        while (isButtonPressed)
        {
            buttonHoldTime += Time.deltaTime;
            yield return null;
        }
    }
    public void AttackMotion()
    {
        if (!Jump.Instance.JumpFlag)
        {
            Debug.Log("ジャンプ中のため攻撃無効");
            return;
        }

        if (Move.Instance.move.x < 0)
        {
            absoluteValueX = Move.Instance.move.x * -1;
        }
        else
        {
            absoluteValueX = Move.Instance.move.x;
        }

        if (Move.Instance.move.y < 0)
        {
            absoluteValueY = Move.Instance.move.y * -1;
        }
        else
        {
            absoluteValueY = Move.Instance.move.y;
        }

        if (absoluteValueX < 0.05 && absoluteValueY < 0.05 && buttonHoldTime >= ULTPressed)
        {
            attackType = AttackType.ULT;
        }
        else if (absoluteValueX < 0.05 && absoluteValueY < 0.05)
        {
            attackType = AttackType.Attack;
        }
        else if (absoluteValueX > absoluteValueY)
        {
            attackType = AttackType.FrontAttack_WeaponChange;
        }
        else if (Move.Instance.move.y > 0)
        {
            attackType = AttackType.UPAttack;
        }

        AttackPending();
    }

    public void AttackPending()
    {
        if (!attackNow) // 攻撃中か確認
        {

            animator.SetBool("run", false);
            Move.Instance.moveX = 0;

            switch (attackType)
            {
                case AttackType.ULT:
                    if (buttonHoldTime >= ULTPressed)
                    {
                        Debug.Log("ULT");
                        animator.SetTrigger("ULTAttack"); // トリガーを設定
                        attackNow = true; // 攻撃中フラグを設定
                    }
                    break;

                case AttackType.Attack:
                    Debug.Log("攻撃");
                    animator.SetTrigger(isGun ? "GunAttack" : "Attack"); // トリガーを設定
                    attackNow = true; // 攻撃中フラグを設定
                    break;

                case AttackType.FrontAttack_WeaponChange:
                    Debug.Log("前攻撃または武器変更");
                    if (left && Move.Instance.move.x < 0 || !left && Move.Instance.move.x > 0)
                    {
                        animator.SetTrigger(isGun ? "GunFrontAttack" : "FrontAttack"); // トリガーを設定
                        attackNow = true; // 攻撃中フラグを設定
                    }
                    else
                    {
                        animator.SetTrigger("Change"); // トリガーを設定
                        isGun = !isGun; // 武器を切り替え
                        attackNow = true; // 攻撃中フラグを設定
                    }
                    break;

                case AttackType.UPAttack:
                    if (Jump.Instance.JumpFlag)
                    {
                        Debug.Log("上攻撃");
                        animator.SetTrigger(isGun ? "GunUPAttack" : "UPAttack"); // トリガーを設定
                        attackNow = true; // 攻撃中フラグを設定
                    }
                    break;
            }
        }
    }

    public void AttackEnd()
    {
        Debug.Log("EndAttack");
        animator.SetTrigger("EndAttack");
        attackNow = false;

    }

}
