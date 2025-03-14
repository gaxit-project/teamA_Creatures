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

    public bool back;

    public bool GardBreak;
    void Start()
    {
        BeastModeHP = 1f;
        playerMaxHP = 30000;
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
        if (Input.GetKeyDown(KeyCode.K)&&!muteki)
        {
            animator.SetTrigger("EnemyTackleHit");
            StartCoroutine(GetUpTime());
            HitNow = true;
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("falter");
            HitNow = true;
        }
        if (Input.GetKeyDown(KeyCode.I)&&!muteki)
        {
            StartCoroutine(GuardBreakStop());
        }
        if (back)
        {
            //今のままじゃ壁に埋もれてしまう
            Knockback(-transform.forward, 20f * Time.deltaTime);
            //transform.position -= transform.forward * 10f * Time.deltaTime;
        }
    }
    public void Hit()
    {
        muteki = true;
        HitNow = true;
    }
    void Knockback(Vector3 forceDirection, float forceAmount)
    {
        float maxDistance = 1.0f; // 壁の近さを判定する距離
        if (!Physics.Raycast(transform.position, forceDirection, maxDistance))
        {
            // 壁が近くにないなら吹き飛ばし
            transform.position += forceDirection * forceAmount;
        }
    }
    public void Back()
    {
        back = true;
        muteki = true;
        HitNow = true;
    }

    public void Backfalse()
    {
        back = false;
    }

    public void Button()
    {
        playerHP = playerHP - 7;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            back = false;
        }
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

                // 敵右パンチ
                if (other.gameObject.tag == "EnemyRightPunchAttack")
                {
                    GardBreak = false;
                    playerHP = playerHP - (10 * BeastModeHP);
                    AudioManager.GetInstance().PlaySE("enemyVoice", 21);
                    EE.PunchEffect();

                    animator.SetTrigger("falter");
                    // ヒットストップ
                    //animator.CrossFade("falter", 0f);
                    HitStopScript.Instance.StartHitStop(0.2f, "Player");
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.Instance.backWallSpeed = 1f;
                    NewEnemyMove.EnemyBack();
                    StartCoroutine(SmoothHPBar());
                    HitNow = true;
                    //muteki = true;
                    //怯む
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                // 敵左パンチ
                if (other.gameObject.tag == "EnemyLeftPunchAttack")
                {
                    GardBreak = false;
                    playerHP = playerHP - (10 * BeastModeHP);
                    AudioManager.GetInstance().PlaySE("enemyVoice", 21);
                    EE.PunchEffect();

                    animator.SetTrigger("falter");
                    // ヒットストップ
                    //animator.CrossFade("falter", 0f);
                    HitStopScript.Instance.StartHitStop(0.2f, "Player");
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.Instance.backWallSpeed = 1f;
                    NewEnemyMove.EnemyBack();
                    StartCoroutine(KnockBack());
                    StartCoroutine(SmoothHPBar());
                    HitNow = true;
                    //muteki = true;
                    //怯む
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                // 敵チェーンアタック
                if (other.gameObject.tag == "EnemyChainAttack")
                {
                    GardBreak = false;
                    playerHP = playerHP - (10 * BeastModeHP);
                    animator.SetTrigger("falter");
                    HitNow = true;
                    muteki = true;
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.EnemyBack();
                    StartCoroutine(SmoothHPBar());
                    //怯む
                    Destroy(AttackComponent.Instance.CountorCube);

                }

                // 敵タックル
                if (other.gameObject.tag == "EnemyTackleAttack")
                {
                    GardBreak = false;
                    playerHP = playerHP - (20 * BeastModeHP);
                    AudioManager.GetInstance().PlaySE("enemyVoice", 21);

                    animator.SetTrigger("falter");
                    HitNow = true;
                    muteki = true;
                    //tackleCamScript.StartCoroutine("tackleCameraCor");
                    AudioManager.GetInstance().PlaySE("enemyAttack", 3);
                    NewEnemyMove.Instance.backWallSpeed = 3f;
                    NewEnemyMove.EnemyBack();
                    StartCoroutine(SmoothHPBar());
                    //吹っ飛ぶ
                    Destroy(AttackComponent.Instance.CountorCube);

                }



            }
        }
        // 敵スマッシュ攻撃
        if (other.gameObject.tag == "EnemySmashAttack" && !muteki)
        {
            if (muteki) return;
            HitNow = true;
            muteki = true;
            GardBreak = false;
            playerHP = playerHP - (30 * BeastModeHP);
            AudioManager.GetInstance().PlaySE("enemyVoice", 21);
            isPlayerDown = true;
            Shield.Instance.OffShield();
            EE.SmashEffect();
            EE.BurstEffect();

            animator.SetTrigger("EnemyTackleHit");
            StartCoroutine(GetUpTime());
            EnemyDriveGauge.Instance.EnemyGaugeUp("Smash");
            // ヒットストップ
            //animator.CrossFade("EnemyTackleHit", 0f);
            HitStopScript.Instance.StartHitStop(0.5f, "Player");
            HitNow = true;
            muteki = true;
            AudioManager.GetInstance().PlaySE("enemyAttack", 3);
            NewEnemyMove.Instance.backWallSpeed = 3f;
            NewEnemyMove.EnemyBack();
            StartCoroutine(SmoothHPBar());
            //吹っ飛ぶ
            Destroy(AttackComponent.Instance.CountorCube);

        }

        // 敵ガードブレイク攻撃
        else if (other.gameObject.tag == "EnemyGuardBreakAttack" && !muteki && !HitNow)
        {
            isPlayerDown = true;
            Shield.Instance.OffShield();
            AudioManager.GetInstance().PlaySE("enemyVoice", 21);
            //EE.SmashEffect();
            //EE.BurstEffect();

            animator.SetTrigger("falter");
            EnemyDriveGauge.Instance.EnemyGaugeUp("GuardBreak");
            // ヒットストップ
            //animator.CrossFade("falter", 0f);
            HitStopScript.Instance.StartHitStop(0.5f, "Player");

            AudioManager.GetInstance().PlaySE("enemyAttack", 3);
            NewEnemyMove.Instance.backWallSpeed = 3f;
            NewEnemyMove.EnemyBack();
            StartCoroutine(SmoothHPBar());
            //吹っ飛ぶ
            Destroy(AttackComponent.Instance.CountorCube);

            if (Shield.Instance.onGuard)
            {
                StartCoroutine(GuardBreakStop());
            }
            else
            {
                GardBreak = false;
                HitNow = true;
                muteki = true;
            }
            Debug.Log("ブレイクされた。うわああああああああああああ！！！！");
        }

        else if (Shield.Instance.onGuard)
        {
            Debug.Log("ガード成功！！！");
            AudioManager.Instance.PlaySE("playerAttack", 12);
            KnockBack();
        }
    }

    public IEnumerator GuardBreakStop()
    {

        if (GardBreak)
        {
            yield break;
        }
        GardBreak = true;
        animator.SetBool("falter", true);
        Debug.Log("ガードブレイクされた");
        float guardBreakTime = 0f;
        while (true)
        {
            muteki = false;
            if (!GardBreak)
            {
                break;
            }
            guardBreakTime += Time.deltaTime;
            if (guardBreakTime >= 5f)
            {
                //animator.SetBool("falter", false);
                GardBreak = false;
                break;
            }
            yield return null;
        }
    }

    public IEnumerator GetUpTime()
    {
        animator.ResetTrigger("Stand");
        yield return new WaitForSeconds(47 / 60f);
        animator.ResetTrigger("EnemyTackleHit");
        animator.SetTrigger("GetUp");
        isPlayerDown = false;
    }
    public void Stand()
    {
        StartCoroutine(Mmuteki());
        animator.ResetTrigger("GetUp");
        animator.SetBool("falter", false);
        animator.SetTrigger("Stand");
        //animator.CrossFade("Stand", 0f);

        

    }
    public void FalterStand()
    {
        StartCoroutine(Mmuteki());
        if (!GardBreak)
        {
            animator.ResetTrigger("GetUp");
            animator.SetBool("falter", false);
            animator.SetTrigger("Stand");
            //animator.CrossFade("Stand", 0f);

            
        }
    }
    public IEnumerator Mmuteki()
    {
        Debug.Log("al;fdjksa");
        HitNow = false;
        yield return new WaitForSeconds(1f);
        //HitNow = false;
        Debug.Log("la;ksdjfa");
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


    IEnumerator KnockBack()
    {
        float knockBackTime = 0f;
        float knockBackSpeed = 3f;
        while (true)
        {
            knockBackTime += Time.deltaTime;
            transform.position -= transform.forward * knockBackSpeed * Time.deltaTime;
            if (knockBackTime >= 1f || PlayerRayCast.isPlayerWall)
            {
                break;
            }
            yield return null;
        }
    }

}
