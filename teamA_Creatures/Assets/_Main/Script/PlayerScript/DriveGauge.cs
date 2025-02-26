using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DriveGauge : MonoBehaviour
{
    float driveCnt = 0f;
    [SerializeField] Image[] driveGauge;
    public float gaugeDownSpeed = 0.1f;

    bool isDGMax = false;
    bool isDriveGaugeUP = false;

    public static bool isDriveGaugeMax = false;
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
            Destroy(Instance);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        driveCnt = 0f;
        driveGauge[0] = GameObject.Find("PlayerDriveGauge1").GetComponent<Image>();
        driveGauge[1] = GameObject.Find("PlayerDriveGauge2").GetComponent<Image>();
        driveGauge[2] = GameObject.Find("PlayerDriveGauge3").GetComponent<Image>();
        driveGauge[3] = GameObject.Find("PlayerDriveGauge4").GetComponent<Image>();
        driveGauge[4] = GameObject.Find("PlayerDriveGauge5").GetComponent<Image>();
        driveGauge[5] = GameObject.Find("PlayerDriveGauge6").GetComponent<Image>();

    }
    private void Update()
    {
        if (isDGMax)
        {
            driveCnt += Time.deltaTime;
            if (driveCnt >= 2f)
            {
                driveCnt = 0f;
                isDGMax = false;
                isDriveGaugeUP = false;
                isDriveGaugeMax = false;
            }
        }
        else if(isDriveGaugeUP)
        {
            driveCnt += Time.deltaTime;
            if (driveCnt >= 1f)
            {
                driveCnt = 0f;
                isDGMax = false;
                isDriveGaugeUP = false;
            }
        }
        else if(!isDBAttack)
        {
            DriveGaugeDown();
        }
    }

    #region ÉQÅ[ÉWÇè„Ç∞ÇÈ
    public void DriveGaugeUP()
    {
        isDriveGaugeUP = true;
        driveCnt = 0f;
        if (driveGauge[0].fillAmount < 1f)
        {
            DriveGaugeSetting(0);
        }
        else if(driveGauge[1].fillAmount < 1f)
        {
            DriveGaugeSetting(1);
        }
        else if (driveGauge[2].fillAmount < 1f)
        {
            DriveGaugeSetting(2);
        }
        else if (driveGauge[3].fillAmount < 1f)
        {
            DriveGaugeSetting(3);
        }
        else if (driveGauge[4].fillAmount < 1f)
        {
            DriveGaugeSetting(4);
        }
        else if (driveGauge[5].fillAmount < 1f)
        {
            DriveGaugeSetting(5);
        }
    }
    void DriveGaugeSetting(int i)
    {
        driveGauge[i].fillAmount += 0.5f;
        if (driveGauge[i].fillAmount >= 1f)
        {
            isDGMax = true;
            if(i == 5)
            {
                AudioManager.GetInstance().PlaySE("playerAttack", 8);
                isDriveGaugeMax = true;
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
        if (driveGauge[5].fillAmount > 0f)
        {
            driveGauge[5].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (driveGauge[4].fillAmount > 0f)
        {
            driveGauge[4].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (driveGauge[3].fillAmount > 0f)
        {
            driveGauge[3].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (driveGauge[2].fillAmount > 0f)
        {
            driveGauge[2].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (driveGauge[1].fillAmount > 0f)
        {
            driveGauge[1].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
        else if (driveGauge[0].fillAmount > 0f)
        {
            driveGauge[0].fillAmount -= gaugeDownSpeed * Time.deltaTime;
        }
    }
    public void DriveGaugeMaxDown()
    {
        for(int j = 0; j < 0; j++)
        {
            driveGauge[j].fillAmount = 0f;
        }
    }
    #endregion
}
