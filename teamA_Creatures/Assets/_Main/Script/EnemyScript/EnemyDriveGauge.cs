using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDriveGauge : MonoBehaviour
{
    float driveCnt = 0f;
    [SerializeField] Image[] enemyDriveGauge;
    public float gaugeDownSpeed = 0.1f;

    [SerializeField] private float guardGaugeDown = 0.2f; //ガード中の減少量
    [SerializeField] private float avoidMoveGaugeDown = 0.5f; //避け技使用時の減少量
    [SerializeField] private float blowAwayGaugeDown = 0.7f; //吹っ飛ばし使用時の減少量

    [SerializeField] private float timeGaugeUp = 0.05f; // 時間経過での上昇量
    [SerializeField] private float technicalGaugeUp = 0.3f; // カウンター時やストレート時などの特殊な技を使った際の上昇量
    [SerializeField] private float damageTakenGaugeUp = 0.2f; // 被ダメージ時の上昇量

    bool isDGMax = false;
    bool isDriveGaugeUP = false;

    public bool isEnemyDriveGaugeMax = false;
    public bool isEnemyDriveGaugeZero = false;
    public static bool isDBAttack = false;

    public static EnemyDriveGauge Instance;
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        driveCnt = 0f;
        for (int i = 0; i < enemyDriveGauge.Length; i++)
        {
            enemyDriveGauge[i] = GameObject.Find($"EnemyDriveGauge{6-i}").GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (isDGMax)
        {
            PlayerMaterialChange.Instance.ChangeMaterial(3);
            driveCnt += Time.deltaTime;
            if (driveCnt >= 2f)
            {
                ResetGaugeStatus();
            }
        }
        else if (isDriveGaugeUP)
        {
            driveCnt += Time.deltaTime;
            if (driveCnt >= 1f)
            {
                ResetGaugeStatus();
            }
        }

        if (enemyDriveGauge[5].fillAmount >= 1f)
        {
            isEnemyDriveGaugeMax = true;
        }
        if (enemyDriveGauge[0].fillAmount < 1f)
        {
            isEnemyDriveGaugeZero = true;
        }
        else
        {
            isEnemyDriveGaugeZero = false;
        }
        // 時間経過でゲージを増加
        EnemyGaugeUp("time");
    }

    public void DriveGaugeDown()
    {
        ReduceGauge(gaugeDownSpeed * Time.deltaTime);
    }

    void ReduceGauge(float amount)
    {
        for (int i = enemyDriveGauge.Length - 1; i >= 0 && amount > 0; i--)
        {
            if (enemyDriveGauge[i].fillAmount > 0f)
            {
                float decrease = Mathf.Min(enemyDriveGauge[i].fillAmount, amount);
                enemyDriveGauge[i].fillAmount -= decrease;
                amount -= decrease;
            }
        }
    }

    public void EnemyGaugeDown(string type)
    {
        float amount = 0f;
        switch (type)
        {
            case "guard":
                amount = guardGaugeDown;
                break;
            case "avoid":
                amount = avoidMoveGaugeDown;
                break;
            case "blowAway":
                amount = blowAwayGaugeDown;
                break;
        }
        ReduceGauge(amount);
    }




    // --- 追加: GaugeUp 関数 ---
    public void EnemyGaugeUp(string type)
    {
        float amount = 0f;
        switch (type)
        {
            case "time":
                amount = timeGaugeUp * Time.deltaTime;
                break;
            case "Smash":
                amount = technicalGaugeUp;
                break;
            case "GuardBreak":
                amount = damageTakenGaugeUp;
                break;
        }
        IncreaseGauge(amount);
    }

    void IncreaseGauge(float amount)
    {
        for (int i = 0; i < enemyDriveGauge.Length; i++)
        {
            if (enemyDriveGauge[i].fillAmount < 1f)
            {
                enemyDriveGauge[i].fillAmount += amount;
                if (enemyDriveGauge[i].fillAmount > 1f)
                {
                    enemyDriveGauge[i].fillAmount = 1f;
                }
                break;
            }
        }
    }

    void ResetGaugeStatus()
    {
        PlayerMaterialChange.Instance.ReturnMaterial();
        driveCnt = 0f;
        isDGMax = false;
        isDriveGaugeUP = false;
        isEnemyDriveGaugeMax = false;
    }

    public void DriveGaugeMaxDown()
    {

    }
}
