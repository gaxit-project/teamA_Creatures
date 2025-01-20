using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cubechain : MonoBehaviour
{
    public float heightPos;

    public static Cubechain Instance;
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
    }
    void Start()
    {
        heightPos = 0;

    }
public void OnTriggerEnter(Collider other)
    {
        // Wallタグのみで処理を実行する
        if (other.gameObject.CompareTag("Wall"))
        {
            Debug.Log("Wallに当たった");
            Cubeee.Instance.chainAttackDone = true; // 攻撃終了フラグ
            Cubeee.Instance.chainAttackNow = false; // 攻撃中フラグを解除n
        }
    }

void Update()
    {

        StartCoroutine(ChainLong());
    }

    IEnumerator ChainLong()
    {
        if (Cubeee.Instance.chainAttackNow)
        {
            heightPos += 10f * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, heightPos);
            transform.position += new Vector3(5f, 0, 0) * Time.deltaTime;
        }
        //ここで伸ばし切った後一定時間止めることもできる
        if (Cubeee.Instance.chainAttackDone)
        {
            heightPos -= 10f * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, heightPos);
            transform.position -= new Vector3(5f, 0, 0) * Time.deltaTime;
            if (heightPos < 0)
            {
                heightPos = 0;
                Cubeee.Instance.chainAttackDone = false;
                Cubeee.Instance.chainAttackNow = false;
                Destroy(this.gameObject);
            }
        }
        yield return null;
    }
}
