using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chainCubeSCP : MonoBehaviour
{

    public void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {

    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy"&&chain.Instance.chainAttackDone)
        {
            chain.Instance.chainAttackDone = false;
            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("“–‚½‚Á‚½");
            chain.Instance.chainAttackDone = true;
            chain.Instance.chainAttackNow = false;
        }
    }
}
