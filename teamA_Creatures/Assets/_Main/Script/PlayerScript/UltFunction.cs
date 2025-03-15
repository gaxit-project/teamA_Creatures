using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltFunction : MonoBehaviour
{

    public void ultAnim()
    {
        NewEnemyMove.Instance.EnemyPushAnim();
        AudioManager.Instance.PlaySE("playerMove",18);
        AudioManager.Instance.PlaySE("enemyMove", 21);
    }
    public void ultAnim2()
    {
        NewEnemyMove.Instance.EnemyPushAnim2();
    }

    public void WalkSound()
    {
        AudioManager.Instance.PlaySE("playerMove", 26);
    }
}
