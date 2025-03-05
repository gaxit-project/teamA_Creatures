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
    [SerializeField] Image playerHPGauge;
    [SerializeField] Image playerSmoothHPGauge;

    public static float BeastModeHP = 0f;
    public static bool isPlayerDown;

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
        BeastModeHP = 1f;
        playerMaxHP = 300;
        playerHP = playerMaxHP;
        animator = GetComponent<Animator>();
        HitNow = false;
        muteki = false;
    }

    private void Update()
    {
        if (playerHP <= 0)
        {
            StartCoroutine(PlayerDown());
            //JM.ChangeOverScene();
        }
        //slider.value = playerHP / playerMaxHP;
        playerHPGauge.fillAmount = playerHP / playerMaxHP;
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
                AttackComponent.Instance.EndAttack();
                //AttackComponent.Instance.attackNow = false;
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
                    EnemyDriveGauge.Instance.DriveGaugeUP(0.5f);
                    StartCoroutine(SmoothHPBar());
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
                    EnemyDriveGauge.Instance.DriveGaugeUP(1f);
                    StartCoroutine(SmoothHPBar());
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
                    //tackleCamScript.StartCoroutine("tackleCameraCor");
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.EnemyBack();
                    EnemyDriveGauge.Instance.DriveGaugeUP(1f);
                    StartCoroutine(SmoothHPBar());
                    //吹っ飛ぶ
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                

            }
        }
        if (other.gameObject.tag == "EnemySmashAttack" && !muteki)
        {
            playerHP = playerHP - (30 * BeastModeHP);
            isPlayerDown = true;
            EE.SmashEffect();
            EE.BurstEffect();
            PE.DamageEffect();
            animator.SetTrigger("EnemyTackleHit");
            // ヒットストップ
            animator.CrossFade("EnemyTackleHit", 0f);
            HitStopScript.Instance.StartHitStop(0.5f, "Player");
            HitNow = true;
            muteki = true;
            AudioManager.GetInstance().PlaySE("enemyAttack", 3);
            NewEnemyMove.EnemyBack();
            EnemyDriveGauge.Instance.DriveGaugeUP(1f);
            StartCoroutine(SmoothHPBar());
            //吹っ飛ぶ
            Destroy(AttackComponent.Instance.CountorCube);
        }
    }
    public void GetUpTime()
    {
        animator.SetTrigger("GetUp");
        isPlayerDown = false;
    }
    public void Stand()
    {
        animator.SetTrigger("Stand");
        animator.CrossFade("Stand", 0f);
        
        StartCoroutine(Mmuteki());
    }
    public IEnumerator Mmuteki()
    {
        //HitNow = false;
        yield return new WaitForSeconds(1f);
        HitNow = false;
        muteki = false;
    }

    /// <summary>
    /// ゲームオーバー関連
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerDown()
    {
        float downTime = 0f;
        // 向きを変えるやつを入れる
        // ダウンアニメーション再生
        animator.SetTrigger("EnemyTackleHit"); // これ仮置き
        JM.LoseEffect();
        // アニメーター.CrossFade("Down", 0.1f, 0, 0.35f);
        while (true)
        {
            downTime += Time.deltaTime;
            if (/*アニメーション終了フラグ &&*/ downTime >= 1.8f)
            {
                break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        JM.ChangeOverScene();
    }

    IEnumerator SmoothHPBar()
    {
        float startFill = playerSmoothHPGauge.fillAmount;
        float elapsedTime = 0f;
        float endFill = playerHP / playerMaxHP;
        float smoothSpeed = 0.2f;

        while (elapsedTime < smoothSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / smoothSpeed;

            // fillAmountを滑らかに変化
            playerSmoothHPGauge.fillAmount = Mathf.Lerp(startFill, endFill, progress);

            yield return null;
        }
    }

}