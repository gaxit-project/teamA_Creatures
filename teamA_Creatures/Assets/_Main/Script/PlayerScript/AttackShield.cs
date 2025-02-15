using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackShield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Wall")
        {

        }
        else
        {
            Debug.Log("shild");
            Destroy(other.gameObject);
        }
    }
}
