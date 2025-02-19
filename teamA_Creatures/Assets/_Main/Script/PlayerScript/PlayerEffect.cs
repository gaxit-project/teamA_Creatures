using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    [SerializeField] GameObject DamageEff;

    private GameObject DamageEf;

    private Vector3 offset = new Vector3(0, 2.0f, 1.5f); // キューブの初期オフセット位置

    public void Damage()
    {
        if (DamageEff != null)
        {

            if (DamageEf == null)
            {

                DamageEffect();

            }

        }
    }

    private void DamageEffect()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y = transform.position.y + offset.y;
        DamageEf = Instantiate(DamageEff, spawnPosition, Quaternion.identity);
    }
}
