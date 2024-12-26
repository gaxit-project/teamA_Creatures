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
        Attack,FrontAttack_WeaponChange, UPAttack,ULT
    }
    AttackType attackType;

    public void Start()
    {
        animator = GetComponent<Animator>();
        left = false;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started) // �{�^�����������u��
        {
            isButtonPressed = true;
            buttonHoldTime = 0; // �������Ԃ����Z�b�g
            StartCoroutine(MeasureButtonHoldTime());
        }
        else if (context.canceled) // �{�^���𗣂����u��
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
        else if (absoluteValueX > Move.Instance.move.y) 
        {
            attackType = AttackType.FrontAttack_WeaponChange;
        }
        else
        {
            attackType = AttackType.UPAttack;
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
                Debug.Log("�U��");
                //animator.SetBool("Attack", true);
                //attackNow = true;
                break;
            case AttackType.FrontAttack_WeaponChange:
                if (left && Move.Instance.move.x < 0)
                {
                    Debug.Log("�O�U��");
                    animator.SetBool("FrontAttack", true);
                    attackNow = true;
                }
                else if (left && Move.Instance.move.x > 0)
                {
                    Debug.Log("����ύX");
                    animator.SetBool("BuckAttack", true);
                    attackNow = true;
                }
                else if (!left && Move.Instance.move.x > 0)
                {
                    Debug.Log("�O�U��");
                    animator.SetBool("FrontAttack", true);
                    attackNow = true;
                }
                else if (!left && Move.Instance.move.x < 0)
                {
                    Debug.Log("����ύX");
                    animator.SetBool("BuckAttack", true);
                    attackNow = true;
                }
                break;
            case AttackType.UPAttack:
                if (Jump.Instance.JumpFlag)
                {
                    Debug.Log("��U��");
                    //attackNow = true;
                }
                break;
        }
        }

    }
    public void AttackEnd()
    {
        //animator.SetBool("ULTAttack",false);
        //animator.SetBool("Attack", false);
        animator.SetBool("FrontAttack", false);
        animator.SetBool("BuckAttack", false);
        attackNow = false;

    }

}
