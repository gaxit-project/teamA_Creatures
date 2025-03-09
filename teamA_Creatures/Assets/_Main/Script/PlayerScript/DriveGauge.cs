using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveGauge : MonoBehaviour
{
    float driveCnt = 0f;
    [SerializeField] Image[] driveGauge;
    public float gaugeDownSpeed = 0.1f;

    [SerializeField] private float guardGaugeDown = 0.2f; //ガード中の減少量
    [SerializeField] private float avoidMoveGaugeDown = 0.5f; //避け技使用時の減少量
    [SerializeField] private float blowAwayGaugeDown = 0.7f; //吹っ飛ばし使用時の減少量

    [SerializeField] private float timeGaugeUp = 0.05f; // 時間経過での上昇量
    [SerializeField] private float technicalGaugeUp = 0.3f; // カウンター時やストレート時などの特殊な技を使った際の上昇量
    [SerializeField] private float damageTakenGaugeUp = 0.2f; // 被ダメージ時の上昇量

    bool isDGMax = false;
    bool isDriveGaugeUP = false;

    public bool isDriveGaugeMax = false;
    public static bool isDBAttack = false;

    public static DriveGauge Instance;
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
        for (int i = 0; i < driveGauge.Length; i++)
        {
            driveGauge[i] = GameObject.Find($"PlayerDriveGauge{i + 1}").GetComponent<Image>();
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
        else if (!isDBAttack)
        {
            DriveGaugeDown();
        }

        // 時間経過でゲージを増加
        GaugeUp("time");
    }

    void DriveGaugeDown()
    {
        for (int i = driveGauge.Length - 1; i >= 0; i--)
        {
            if (driveGauge[i].fillAmount > 0f)
            {
                driveGauge[i].fillAmount -= gaugeDownSpeed * Time.deltaTime;
                if (driveGauge[i].fillAmount < 0f)
                {
                    driveGauge[i].fillAmount = 0f;
                }
                break;
            }
        }
    }

    public void GaugeDown(string type)
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

    void ReduceGauge(float amount)
    {
        for (int i = driveGauge.Length - 1; i >= 0; i--)
        {
            if (driveGauge[i].fillAmount > 0f)
            {
                driveGauge[i].fillAmount -= amount;
                if (driveGauge[i].fillAmount < 0f)
                {
                    driveGauge[i].fillAmount = 0f;
                }
                break;
            }
        }
    }

    // --- 追加: GaugeUp 関数 ---
    public void GaugeUp(string type)
    {
        float amount = 0f;
        switch (type)
        {
            case "time":
                amount = timeGaugeUp * Time.deltaTime;
                break;
            case "counter":
                amount = technicalGaugeUp;
                break;
            case "damage":
                amount = damageTakenGaugeUp;
                break;
        }
        IncreaseGauge(amount);
    }

    void IncreaseGauge(float amount)
    {
        for (int i = 0; i < driveGauge.Length; i++)
        {
            if (driveGauge[i].fillAmount < 1f)
            {
                driveGauge[i].fillAmount += amount;
                if (driveGauge[i].fillAmount > 1f)
                {
                    driveGauge[i].fillAmount = 1f;
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
        isDriveGaugeMax = false;
    }

    public void DriveGaugeMaxDown()
    {

    }
}
