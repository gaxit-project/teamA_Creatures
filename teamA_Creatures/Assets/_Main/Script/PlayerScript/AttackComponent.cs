using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public static AttackComponent Instance;
    public bool Countered;

    Animator animator;
    AudioSource audioSource;
    public AudioClip attackSound;
    public float soundCooldownTime = 0.5f;

    private bool canPlaySound = true;

    public bool attackNow;
    public bool CounterRange;
    private Hit cubeController;
    public TackleCamera counterCamScript;
    public bool isRush;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        cubeController = GetComponent<Hit>();
    }
    public void Start()
    {
        Countered = false;
        CounterRange = false;
        ComboStart = false;
    }
    enum AttackType
    {
        attack,
        counterNow
    }
    AttackType attackType;

    public float attackCooldown;
    private float lastAttackTime = -Mathf.Infinity;
    public bool isCooldown;

    public float ComboCooldown;
    private float lastComboTime = -Mathf.Infinity;
    public bool isComboNow;

    public float counterCooldown;
    private float lastCounterTime = -Mathf.Infinity;
    public bool isCounterCooldown;
    public bool ComboStart;

    public int maxCombo;
    private int currentCombo = 0;



    public void Attack()
    {

        if (attackNow) return;

        if (MoveComponent.Instance.ATFieldNow)
        {
            attackType = AttackType.counterNow;
        }
        else
        {
            attackType = AttackType.attack;
        }

        Debug.Log($"AttackType: {attackType}");

        switch (attackType)
        {

            case AttackType.attack:

                if (DriveGauge.Instance.isDriveGaugeMax)
                {
                    Debug.Log("連打");
                    //ここで無敵の判定を入れておく
                    DriveGauge.Instance.DriveGaugeMaxDown();

                    animator.SetBool("Rush", true);
                    isRush = true;
                    attackNow = true;
                    NewEnemyMove.Instance.EnemyPushFanction();
                }
                else if (!DriveGauge.Instance.isDriveGaugeMax)
                {
                    if (isCooldown && currentCombo == 0)
                    {
                        return;
                    }
                    if (!isComboNow && ComboStart)
                    {
                        currentCombo = 0;
                        lastComboTime = Time.time;
                        ComboStart = false;
                    }

                    if (JumpComponent.Instance.jumpFlag && !attackNow)
                    {
                        if (currentCombo == 0)
                        {
                            Debug.Log("攻撃0");
                            animator.SetTrigger("Attack");
                            attackNow = true;
                            animator.SetBool("run", false);
                            animator.SetBool("Back", false);
                            StartCoroutine(AttackTimeout(0.5f));
                            if (audioSource != null && attackSound != null && canPlaySound)
                            {
                                audioSource.PlayOneShot(attackSound);
                                //canPlaySound = false;
                                StartCoroutine(ResetSoundCooldown());
                            }

                            if (cubeController != null)
                            {
                                cubeController.ShowPunchCube();
                            }
                            ComboStart = true;
                            lastComboTime = Time.time;
                            lastAttackTime = Time.time;

                        }

                        if (currentCombo == 1)
                        {
                            Debug.Log("攻撃1");
                            animator.SetTrigger("Attack");
                            attackNow = true;
                            animator.SetBool("run", false);
                            animator.SetBool("Back", false);
                            StartCoroutine(AttackTimeout(0.5f));
                            if (audioSource != null && attackSound != null && canPlaySound)
                            {
                                audioSource.PlayOneShot(attackSound);
                                //canPlaySound = false;
                                StartCoroutine(ResetSoundCooldown());
                            }

                            if (cubeController != null)
                            {
                                cubeController.ShowPunchCube();
                            }
                            lastComboTime = Time.time;
                            lastAttackTime = Time.time;

                        }
                        // ストレート
                        if (currentCombo == 2)
                        {
                            Debug.Log("攻撃2");
                            animator.SetTrigger("Smash");
                            attackNow = true;
                            animator.SetBool("run", false);
                            animator.SetBool("Back", false);
                            StartCoroutine(AttackTimeout(0.5f));
                            cubeController.ShowStreatCube();
                            if (audioSource != null && attackSound != null && canPlaySound)
                            {
                                audioSource.PlayOneShot(attackSound);
                                //canPlaySound = false;
                                StartCoroutine(ResetSoundCooldown());
                            }
                            lastComboTime = Time.time;
                            lastAttackTime = Time.time;

                        }
                        currentCombo++;
                        if (currentCombo >= maxCombo)
                        {
                            lastAttackTime = Time.time;
                            currentCombo = 0;
                        }
                    }
                }



                break;

            case AttackType.counterNow:
                if (isCounterCooldown)
                {
                    return;
                }

                Debug.Log("カウンター");
                CounterRange = true;
                animator.SetTrigger("Counter");
                attackNow = true;
                StartCoroutine(AttackTimeout(32f/60f));
                lastCounterTime = Time.time;
                break;
        }
    }
    public bool start = false;
    public GameObject Cube;
    public GameObject CountorCube;
    public Vector3 CCube;

    public void RastPanch()
    {
        animator.SetBool("Rush", false);
        animator.SetBool("RastPanch", true);
    }

    public IEnumerator Tame()
    {
        animator.speed = 0f;
        DriveGauge.Instance.isDriveGaugeMax = false;
        yield return new WaitForSeconds(2);
        animator.speed = 1f;
    }

    public void nan()
    {
        // 攻撃終了後のフット橋
        isRush = false;
        animator.SetBool("RastPanch", false);
        attackNow = false;
    }
    public void EndAttack()
    {
        attackNow = false;
        animator.SetTrigger("EndAttack");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Counter");
        animator.ResetTrigger("Smash");
        animator.SetBool("Shield", false);
        if (cubeController != null)
        {
            cubeController.HidePunchCube();
        }
    }
    public void ShieldOut()
    {
        Shield.Instance.OffShield();
        CounterRange = false;
        CCube = MoveComponent.Instance.Player.transform.position + MoveComponent.Instance.Player.transform.forward * 1.3f + MoveComponent.Instance.Player.transform.up * 2.3f;
        CountorCube = Instantiate(Cube, CCube, Quaternion.identity);

    }
    public void CounterOut()
    {
        Destroy(CountorCube);
    }

    private IEnumerator ResetSoundCooldown()
    {
        yield return new WaitForSeconds(soundCooldownTime);
        canPlaySound = true;
    }


    private IEnumerator AttackTimeout(float duration)
    {
        yield return new WaitForSeconds(duration);
            Debug.Log("EndAttack");
            EndAttack();

    }

    public void Update()
    {
        isCooldown = Time.time < lastAttackTime + attackCooldown;
        isCounterCooldown = Time.time < lastCounterTime + counterCooldown;
        isComboNow = Time.time < lastComboTime + ComboCooldown;
    }

}
