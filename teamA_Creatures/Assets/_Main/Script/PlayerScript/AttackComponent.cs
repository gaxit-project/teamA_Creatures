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
    public float soundCooldownTime = 2.0f;

    private bool canPlaySound = true;

    public bool attackNow;
    public bool CounterRange;
    private Hit cubeController;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }else if(Instance != this)
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
    public void Attack()
    {
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
                if (JumpComponent.Instance.jumpFlag)
                {
                    Debug.Log("攻撃");
                    animator.SetTrigger("Attack");
                    attackNow = true;

                    if(audioSource != null && attackSound != null && canPlaySound)
                    {
                        audioSource.PlayOneShot(attackSound);
                        canPlaySound = false;
                        StartCoroutine(ResetSoundCooldown());
                    }

                    if (cubeController != null)
                    {
                        cubeController.ShowPunchCube();
                    }
                }

                break;

            case AttackType.counterNow:
                Debug.Log("カウンター");
                CounterRange = true;
                animator.SetTrigger("Counter");
                attackNow = true;
                break;
        }
    }

    public void EndAttack()
    {
        attackNow = false;
        animator.SetTrigger("EndAttack");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Counter");
        if (cubeController != null)
        {
            cubeController.HidePunchCube();
        }
    }
    public void ShieldOut()
    {

        Shield.Instance.OffShield();
        CounterRange = false;
    }

    private IEnumerator ResetSoundCooldown()
    {
        yield return new WaitForSeconds(soundCooldownTime);
        canPlaySound = true;
    }
}
