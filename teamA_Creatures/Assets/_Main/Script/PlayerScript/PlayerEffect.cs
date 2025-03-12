using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    public ParticleSystem DamageEff;
    public ParticleSystem PunchEff; // シーン内のパーティクル
    public ParticleSystem PunchFireEff;
    public ParticleSystem CounterEff;

   

    public static PlayerEffect Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }
    }
    public void DamageEffect()
    {
        if (DamageEff != null)
        {
            
            // パーティクルを再生
            DamageEff.Play();
        }
    }


    public void PunchEffect()
    {
        if (PunchEff != null)
        {
            
            PunchEff.Play();
        }
    }

    public void PunchFireEffect()
    {
        if (PunchFireEff != null)
        {
            PunchFireEff.Play();
        }
    }

    public void CounterAttEffect()
    {
        if (CounterEff != null)
        {
            CounterEff.Play();
        }
    }
}
