using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    public static PlayerHP Instance;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(Instance);
        }
    }
    private float playerMaxHP;
    private float playerHP;

    Animator animator;
    public Slider slider;

    public bool HitNow;
    void Start()
    {
        playerMaxHP = 100;
        playerHP = playerMaxHP;
        animator = GetComponent<Animator>();
        HitNow = false;
    }

    private void Update()
    {
        slider.value = playerHP / playerMaxHP;
        if (Input.GetKeyDown(KeyCode.K))
        {
            animator.SetTrigger("EnemyTackleHit");
            HitNow = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("falter");
            HitNow = true;
        }
    }
    public void Button()
    {
        playerHP = playerHP - 7;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerHP = playerHP - 5;

        }

        if(collision.gameObject.tag == "EnemyPunchAttack")
        {
            playerHP = playerHP - 10;
            animator.SetTrigger("falter");
            HitNow = true;
            //ãØÇﬁ
        }

        if (collision.gameObject.tag == "EnemyChainAttack")
        {
            playerHP = playerHP - 10;
            animator.SetTrigger("falter");
            HitNow = true;
            //ãØÇﬁ
        }

        if (collision.gameObject.tag == "EnemyTackleAttack")
        {
            playerHP = playerHP - 20;
            animator.SetTrigger("EnemyTackleHit");
            HitNow = true;
            //êÅÇ¡îÚÇ‘
        }

        if (collision.gameObject.tag == "EnemySmashAttack")
        {
            playerHP = playerHP - 30;
            animator.SetTrigger("EnemyTackleHit");
            HitNow = true;
            //êÅÇ¡îÚÇ‘
        }
    }

    public void GetUpTime()
    {
        animator.SetTrigger("GetUp");
        
    }
    public void Stand()
    {
        animator.SetTrigger("Stand");
        HitNow = false;
    }
}
