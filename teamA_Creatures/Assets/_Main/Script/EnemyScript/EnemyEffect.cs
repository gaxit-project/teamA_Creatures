using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    public ParticleSystem PunchEff; // シーン内のパーティクル
    public ParticleSystem PunchFireEff; // シーン内のパーティクル
    public ParticleSystem TackleEff; // シーン内のパーティクル
    public ParticleSystem SmashEff; // シーン内のパーティクル
    public ParticleSystem BurstEff; // シーン内のパーティクル
    public ParticleSystem DamageEff;

    public float spawnDistance = 1.0f; // キャラクターの前方距離
    public float heightOffset = 3.0f; // 高さのオフセット

    public static EnemyEffect Instance;
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

    /// <summary>
    /// パンチエフェクト
    /// </summary>
    public void PunchEffect()
    {
        if (PunchEff != null)
        {
            // �L�����N�^�[�̑O�����v�Z
            Vector3 spawnPos = transform.position + transform.forward * spawnDistance;

            spawnPos.y += 3.0f;

            PunchEff.transform.position = spawnPos;

            PunchEff.transform.rotation = transform.rotation;

            Debug.Log("出した");
            // パーティクルを再生
            PunchEff.Stop();
            PunchEff.Play();
            Debug.Log("出た");
        }
    }

    public void PunchFireEffect()
    {
        if (PunchFireEff != null)
        {
            // �L�����N�^�[�̑O�����v�Z
            Vector3 spawnPos = transform.position + transform.forward * spawnDistance;

            spawnPos.y += 4.5f;

            PunchFireEff.transform.position = spawnPos;

            PunchFireEff.transform.rotation = transform.rotation;

            Debug.Log("出した");
            // パーティクルを再生
            PunchFireEff.Stop();
            PunchFireEff.Play();
            Debug.Log("出た");
        }
    }

    public void TackleEffect()
    {
        if (TackleEff != null)
        {
            // �L�����N�^�[�̑O�����v�Z
            Vector3 spawnPos = transform.position + transform.forward * spawnDistance;

            spawnPos.y += heightOffset;

            TackleEff.transform.position = spawnPos;

            
            TackleEff.transform.rotation = Quaternion.LookRotation(-transform.forward);

            Debug.Log("出した");
            // パーティクルを再生
            TackleEff.Stop();
            TackleEff.Play();
            Debug.Log("出た");
        }
    }

    public void TackleEnd()
    {
        TackleEff.Stop();
    }

    public void SmashEffect()
    {
        if (SmashEff != null)
        {
            // �L�����N�^�[�̑O�����v�Z
            Vector3 spawnPos = transform.position + transform.forward * spawnDistance;

            spawnPos.y += 0f;

            SmashEff.transform.position = spawnPos;


            Debug.Log("出した");
            // パーティクルを再生
            SmashEff.Stop();
            SmashEff.Play();
            Debug.Log("出た");
        }
    }

    public void BurstEffect()
    {
        if (BurstEff != null)
        {
        
            
            BurstEff.Play();
        }
    }

    public void EnemyDamageEffect()
    {
        if (DamageEff != null)
        {

            // パーティクルを再生
            DamageEff.Play();
        }
    }
}
