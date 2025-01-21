using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chain : MonoBehaviour
{
    public static chain Instance;

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

    public GameObject Enemy;
    public GameObject chainCube;
    private GameObject ChainCube;
    public bool chainAttackNow;
    public bool chainAttackDone;
    public Vector3 EnemyChain;
    public Vector3 EnemyChainNow;

    public float ChainSpeed=10f;
    public void ChainAttack()
    {
        EnemyChain = Enemy.transform.position + Enemy.transform.forward * 2f + Enemy.transform.up * 2f;
        ChainCube = Instantiate(chainCube, EnemyChain, Quaternion.identity);
        ChainCube.transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 0, 0));
        chainAttackNow = true;
        ChainCube.AddComponent<chainCubeSCP>();
    }
    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.N)&&!chainAttackDone&&!chainAttackNow)
        {
            ChainAttack();
        }
        if (chainAttackNow)
        {
            EnemyChain += ChainCube.transform.forward * ChainSpeed * Time.deltaTime;
            ChainCube.transform.position = EnemyChain;
        }
        if (chainAttackDone)
        {
            EnemyChain -= ChainCube.transform.forward * ChainSpeed * Time.deltaTime;
            ChainCube.transform.position = EnemyChain;
        }
    }

}
