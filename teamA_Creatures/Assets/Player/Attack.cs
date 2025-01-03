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
        Attack,FrontAttack_WeaponChange, UPAttack,DownAttack,ULT
    }
    AttackType attackType;

    public void Start()
    {
        animator = GetComponent<Animator>();
        left = false;
        isGun = false;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) // É{É^ÉìÇâüÇµÇΩèuä‘
        {
            isButtonPressed = true;
            buttonHoldTime = 0; // âüâ∫éûä‘ÇÉäÉZÉbÉg
            StartCoroutine(MeasureButtonHoldTime());
        }
        else if (context.canceled) // É{É^ÉìÇó£ÇµÇΩèuä‘
        {
            isButtonPressed = false;
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
        else if (Move.Instance.move.y < 0)
        {
            attackType = AttackType.DownAttack;
        }

        AttackPending();
    }

    public void AttackPending()
    {
        if (!attackNow)
        {
        animator.SetBool("run", false);
        Move.Instance.moveX = 0;
        switch (attackType)
        {
            case AttackType.ULT:
                Debug.Log("ULT");
                //animator.SetBool("ULTAttack", true);
               // attackNow = true;
                break;

            case AttackType.Attack:
                    if (!isGun)
                    {
                         Debug.Log("çUåÇ");
                        animator.SetBool("Attack", true);
                    }
                    else
                    {
                        Debug.Log("çUåÇ");
                        animator.SetBool("GunAttack", true);
                    }
                    attackNow = true;
                    break;
            case AttackType.FrontAttack_WeaponChange:
                if (left && Move.Instance.move.x < 0)
                {
                    Debug.Log("ëOçUåÇ");
                        if (!isGun)
                        {
                    animator.SetBool("FrontAttack", true);

                        }
                        else
                        {
                            animator.SetBool("GunFrontAttack", true);
                        }
                        attackNow = true;
                    }
                else if (left && Move.Instance.move.x > 0)
                {
                    Debug.Log("ïêäÌïœçX");
                    animator.SetBool("BuckAttack", true);
                    attackNow = true;
                    isGun = !isGun;
                }
                else if (!left && Move.Instance.move.x > 0)
                {
                    Debug.Log("ëOçUåÇ");
                        if (!isGun)
                        {
                            animator.SetBool("FrontAttack", true);

                        }
                        else
                        {
                            animator.SetBool("GunFrontAttack", true);
                        }
                        attackNow = true;
                    }
                else if (!left && Move.Instance.move.x < 0)
                {
                    Debug.Log("ïêäÌïœçX");
                    animator.SetBool("BuckAttack", true);
                    attackNow = true;
                    isGun = !isGun;
                }
                break;
            case AttackType.UPAttack:
                if (Jump.Instance.JumpFlag&&isGun)
                {
                    Debug.Log("è„çUåÇ");
                        //animator.SetBool("UPAttack", true);
                    //attackNow = true;
                }
                    break;
                case AttackType.DownAttack:
                    if (Jump.Instance.JumpFlag&&!isGun)
                    {
                        Debug.Log("â∫çUåÇ");
                        //animator.SetBool("DownAttack", true);
                        //attackNow = true;
                    }
                break;
        }
        }

    }
    public void AttackEnd()
    {
        //animator.SetBool("ULTAttack",false);
        animator.SetBool("Attack", false);
        animator.SetBool("GunAttack",false);
        animator.SetBool("FrontAttack", false);
        animator.SetBool("GunFrontAttack", false);
        //animator.SetBool("UPAttack", false);
        //animator.SetBool("DownAttack", false);
        animator.SetBool("BuckAttack", false);
        attackNow = false;

    }

}
