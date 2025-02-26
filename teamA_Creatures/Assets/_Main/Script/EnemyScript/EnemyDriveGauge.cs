using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDriveGauge : MonoBehaviour
{
    float driveCnt = 0f;
    [SerializeField] Image[] enemyDriveGauge;
    public float gaugeDownSpeed = 0.1f;

    bool isDGMax = false;
    bool isDriveGaugeUP = false;

    public bool isEnemyDriveGaugeMax = false;
    public bool isEnemyDBAttack = false;

    public float stopGauge = 2f;

    public static EnemyDriveGauge Instance;
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
    // Start is called before the first frame update
    void Start()
    {
        driveCnt = 0f;
        enemyDriveGauge[0] = GameObject.Find("EnemyDriveGauge6").GetComponent<Image>();
        enemyDriveGauge[1] = GameObject.Find("EnemyDriveGauge5").GetComponent<Image>();
        enemyDriveGauge[2] = GameObject.Find("EnemyDriveGauge4").GetComponent<Image>();
        enemyDriveGauge[3] = GameObject.Find("EnemyDriveGauge3").GetComponent<Image>();
        enemyDriveGauge[4] = GameObject.Find("EnemyDriveGauge2").GetComponent<Image>();
        enemyDriveGauge[5] = GameObject.Find("EnemyDriveGauge1").GetComponent<Image>();

    }
    private void Update()
    {
        if (isDGMax)
        {
            driveCnt += Time.deltaTime;
            if (driveCnt >= stopGauge)
            {
                stopGauge = 2f;
                driveCnt = 0f;
                isDGMax = false;
                isDriveGaugeUP = false;
                isEnemyDriveGaugeMax = false;
            }
        }
        else if (isDriveGaugeUP)
        {
            driveCnt += Time.deltaTime;
            if (driveCnt >= 1f)
            {
                driveCnt = 0f;
                isDGMax = false;
                isDriveGaugeUP = false;
            }
        }
        else if (!isEnemyDBAttack)
        {
            DriveGaugeDown();
        }
    }

    #region ÉQÅ[ÉWÇè„Ç∞ÇÈ
    public void DriveGaugeUP(float num)
    {
        isDriveGaugeUP = true;
        driveCnt = 0f;
        if (enemyDriveGauge[0].fillAmount < 1f)
        {
            DriveGaugeSetting(0, num);
        }
        else if (enemyDriveGauge[1].fillAmount < 1f)
        {
            DriveGaugeSetting(1, num);
        }
        else if (enemyDriveGauge[2].fillAmount < 1f)
        {
            DriveGaugeSetting(2, num);
        }
        else if (enemyDriveGauge[3].fillAmount < 1f)
        {
            DriveGaugeSetting(3, num);
        }
        else if (enemyDriveGauge[4].fillAmount < 1f)
        {
            DriveGaugeSetting(4, num);
        }
        else if (enemyDriveGauge[5].fillAmount < 1f)
        {
            DriveGaugeSetting(5, num);
        }
    }
    void DriveGaugeSetting(int i, float num)
    {
        enemyDriveGauge[i].fillAmount += num;
        if (enemyDriveGauge[i].fillAmount >= 1f)
        {
            isDGMax = true;
            if (i == 5)
            {
                AudioManager.GetInstance().PlaySE("playerAttack", 8);
                isEnemyDriveGaugeMax = true;
                stopGauge = 5f;
            }
            else
            {
                AudioManager.GetInstance().PlaySE("playerAttack", 7);
            }
        }
    }
    #endregion

    #region ÉQÅ[ÉWÇâ∫Ç∞ÇÈ
    void DriveGaugeDown()
    {
        if (enemyDriveGauge[5].fillAmount > 0f)
        {
            enemyDriveGauge[5].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (enemyDriveGauge[4].fillAmount > 0f)
        {
            enemyDriveGauge[4].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (enemyDriveGauge[3].fillAmount > 0f)
        {
            enemyDriveGauge[3].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (enemyDriveGauge[2].fillAmount > 0f)
        {
            enemyDriveGauge[2].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (enemyDriveGauge[1].fillAmount > 0f)
        {
            enemyDriveGauge[1].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (enemyDriveGauge[0].fillAmount > 0f)
        {
            enemyDriveGauge[0].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
    }
    public void DriveGaugeMaxDown()
    {
        for (int j = 0; j < 0; j++)
        {
            enemyDriveGauge[j].fillAmount = 0f;
        }
    }
    #endregion
}
