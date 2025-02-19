using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static Counter Instance;
    Animator animator;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        {
            Destroy(gameObject);
        }
        animator = GetComponent<Animator>();
    }
    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Enemy")
        {
            
            Debug.Log("Counter");
        }
        else
        {

        }

        if (other.gameObject.tag == "EnemyPunchAttack")
        {
            Debug.Log("Counter");
            AttackComponent.Instance.Countered = true;
        }

        if (other.gameObject.tag == "EnemyChainAttack")
        {
            Debug.Log("Counter");
            AttackComponent.Instance.Countered = true;
        }

        if (other.gameObject.tag == "EnemyTackleAttack")
        {
            Debug.Log("Counter");
            AttackComponent.Instance.Countered = true;
        }
    }
}
