using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static Counter Instance;


    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else if(Instance == this) 
        {
            Destroy(gameObject);
        }
    }
    public void OnTriggerStay(Collider other)
    {

        if (other.gameObject.tag == "Enemy" && AttackComponent.Instance.CounterRange) 
        {
            Debug.Log("Counter");
            AttackComponent.Instance.Countered = true;
        }
    }
}
