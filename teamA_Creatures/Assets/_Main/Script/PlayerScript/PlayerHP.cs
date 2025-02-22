using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHP : MonoBehaviour
{
    public static PlayerHP Instance;
    public JudgeManager JM;
    public NewEnemyMove NewEnemyMove;
    public EnemyEffect EE;
    public PlayerEffect PE;

    public static float BeastModeHP = 0f;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
    }
    private float playerMaxHP;
    private float playerHP;

    Animator animator;
    public Slider slider;
    public TackleCamera tackleCamScript;

    public bool HitNow;
    public bool muteki;
    void Start()
    {
        BeastModeHP = 0f;
        playerMaxHP = 100;
        playerHP = playerMaxHP;
        animator = GetComponent<Animator>();
        HitNow = false;
        muteki = false;
    }

    private void Update()
    {
        if (playerHP <= 0)
        {
            JM.ChangeOverScene();
        }
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
    public void OnTriggerEnter(Collider other)
    {
        if (!MoveComponent.Instance.ATFieldNow && !muteki)
        {
            Destroy(AttackComponent.Instance.CountorCube);
            if (other.gameObject.tag == "Enemy")
            {
                playerHP = playerHP - 5;
                Destroy(AttackComponent.Instance.CountorCube);

            }
            else
            {
                AttackComponent.Instance.attackNow = false;
                animator.SetBool("Shield", false);

                if (other.gameObject.tag == "EnemyPunchAttack")
                {
                    playerHP = playerHP - (10 * BeastModeHP);
                    EE.PunchEffect();
                    PE.DamageEffect();
                    animator.SetTrigger("falter");
                    // ヒットストップ
                    animator.CrossFade("falter", 0f);
                    HitStopScript.Instance.StartHitStop(0.2f, "Player");
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.EnemyBack();
                    HitNow = true;
                    muteki = true;
                    //怯む
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                if (other.gameObject.tag == "EnemyChainAttack")
                {
                    playerHP = playerHP - (10 * BeastModeHP);
                    animator.SetTrigger("falter");
                    HitNow = true;
                    muteki = true;
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.EnemyBack();
                    //怯む
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                if (other.gameObject.tag == "EnemyTackleAttack")
                {
                    playerHP = playerHP - (20 * BeastModeHP);
                    PE.TackleDamageEffect();
                    animator.SetTrigger("falter");
                    HitNow = true;
                    muteki = true;
                    tackleCamScript.StartCoroutine("tackleCameraCor");
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.EnemyBack();
                    //吹っ飛ぶ
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                if (other.gameObject.tag == "EnemySmashAttack")
                {
                    playerHP = playerHP - (30 * BeastModeHP);
                    EE.SmashEffect();
                    PE.DamageEffect();
                    animator.SetTrigger("EnemyTackleHit");
                    // ヒットストップ
                    animator.CrossFade("EnemyTackleHit", 0f);
                    HitStopScript.Instance.StartHitStop(0.5f, "Player");
                    HitNow = true;
                    muteki = true;
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.EnemyBack();
                    //吹っ飛ぶ
                    Destroy(AttackComponent.Instance.CountorCube);
                }

            }
        }
    }
    public void GetUpTime()
    {
        animator.SetTrigger("GetUp");
    }
    public void Stand()
    {
        animator.SetTrigger("Stand");
        animator.CrossFade("Stand", 0f);
        HitNow = false;
        StartCoroutine(Mmuteki());
    }
    public IEnumerator Mmuteki()
    {
        yield return new WaitForSeconds(2f);
        muteki = false;
    }
}