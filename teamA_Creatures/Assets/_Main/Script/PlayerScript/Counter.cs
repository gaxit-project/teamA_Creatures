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
    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && AttackComponent.Instance.CounterRange)
        {
            
        }
        if (other.gameObject.tag == "Enemy")
        {

        }

        if (other.gameObject.tag == "EnemyPunchAttack")
        {
            PlayerEffect.Instance.CounterAttEffect();
            Debug.Log("Counter");
            HitStopScript.Instance.StartHitStop(0.5f, "Player");
            PlayerMaterialChange.Instance.ChangeMaterial(1);
            NewEnemyMove.Instance.EnemyStanState();
            AttackComponent.Instance.Countered = true;
            //AttackComponent.Instance.counterCamScript.StartCoroutine("tackleCameraCor");

        }

        if (other.gameObject.tag == "EnemyChainAttack")
        {
            PlayerEffect.Instance.CounterAttEffect();
            Debug.Log("Counter");
            HitStopScript.Instance.StartHitStop(0.5f, "Player");
            PlayerMaterialChange.Instance.ChangeMaterial(1);
            AttackComponent.Instance.Countered = true;
            NewEnemyMove.Instance.EnemyStanState();
            //AttackComponent.Instance.counterCamScript.StartCoroutine("tackleCameraCor");


        }

        if (other.gameObject.tag == "EnemyTackleAttack")
        {
            PlayerEffect.Instance.CounterAttEffect();
            Debug.Log("Counter");
            HitStopScript.Instance.StartHitStop(0.5f, "Player");
            PlayerMaterialChange.Instance.ChangeMaterial(1);
            AttackComponent.Instance.Countered = true;
            NewEnemyMove.Instance.EnemyStanState();
            //AttackComponent.Instance.counterCamScript.StartCoroutine("tackleCameraCor");


        }
    }
}
