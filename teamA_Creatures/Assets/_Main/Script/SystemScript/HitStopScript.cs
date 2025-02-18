using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStopScript : MonoBehaviour
{
    public static HitStopScript Instance; // 静的インスタンス（シングルトン風）

    public List<Animator> charAnim = new List<Animator>();
    [SerializeField] PlayerHP PlayerHP;
    void Awake()
    {
        Instance = this; // 自動で自身を格納
    }

    public void StartHitStop(float num,string name)
    {
        StartCoroutine(HitStop(num,name));
    }

    IEnumerator HitStop(float num,string name)
    {
        // 攻撃を受けたときにストップする処理
        if(name.Equals("Enemy"))
        {
            charAnim[0].speed = 0.2f;
            charAnim[1].speed = 0f;
        }
        else if(name.Equals("Player"))
        {
            charAnim[0].speed = 0f;
            charAnim[1].speed = 0.2f;
        }

        yield return new WaitForSecondsRealtime(num);

        // ストップを解除する
        charAnim[0].speed = 1f;
        charAnim[1].speed = 1f;


        //// 攻撃硬直後の処理
        //yield return new WaitForSecondsRealtime(num);

        //if (name.Equals("Enemy"))
        //{
        //}
        //else if (name.Equals("Player"))
        //{
        //    // 弱パン
        //    if(num == 0.2f)
        //    {
        //        PlayerHP.Stand();
        //    }
        //    // スマッシュ時
        //    else if(num == 0.5f)
        //    {

        //    }
        //}
    }
}
