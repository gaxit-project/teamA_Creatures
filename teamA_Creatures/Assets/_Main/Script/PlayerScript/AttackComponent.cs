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
    }
    enum AttackType
    {
        attack,
        counterNow
    }
    AttackType attackType;

    public float attackCooldown = 1.0f;
    private float lastAttackTime = -Mathf.Infinity;
    public float counterCooldown = 1.0f;
    private float lastCounterTime = -Mathf.Infinity;
    private int maxCombo = 1;
    private int currentCombo = 0;
    public void Attack()
    {
        bool isCooldown = Time.time < lastAttackTime + attackCooldown;
        bool isCounterCooldown = Time.time < lastAttackTime + counterCooldown;


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
                if (isCooldown && currentCombo == 0)
                {
                    return;
                }

                if (JumpComponent.Instance.jumpFlag && !attackNow)
                {
                    Debug.Log("攻撃");
                    animator.SetTrigger("Attack");
                    attackNow = true;
                    animator.SetBool("run", false);
                    animator.SetBool("Back", false);
                    StartCoroutine(AttackTimeout(21f / 60f)); 
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
                    currentCombo++;
                    if (currentCombo >= maxCombo)
                    {
                        lastAttackTime = Time.time;
                        currentCombo = 0;
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
                lastAttackTime = Time.time;
                break;
        }
    }
    public GameObject Cube;
    public GameObject CountorCube;
    public Vector3 CCube;

    public void EndAttack()
    {
        attackNow = false;
        animator.SetTrigger("EndAttack");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Counter");
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
        if (attackNow)
        {
            Debug.Log("Attackタイムアウトにより強制終了");
            EndAttack();
        }
    }
}
