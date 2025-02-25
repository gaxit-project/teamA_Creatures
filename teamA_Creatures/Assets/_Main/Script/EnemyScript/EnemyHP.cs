using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] Image[] hpBars; // HPゲージ
    [SerializeField] Image[] hpMoveBars; // HPゲージ
    [SerializeField] float[] segmentMaxHP; 
    private float[] segmentHP; // 各ゲージの現在のHP
    private int currentSegment; // 現在のゲージのインデックス
    float hpBarSpeed = 0.2f;

    public static EnemyHP Instance;
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
    void Start()
    {
        segmentHP = new float[hpBars.Length];
        segmentMaxHP = new float[hpBars.Length];
        for (int i = 0; i < hpBars.Length; i++)
        {
            segmentMaxHP[i] = NewEnemyMove.enemyInitialHP / hpBars.Length;
            segmentHP[i] = segmentMaxHP[i]; // 各ゲージのHPを最大値で初期化
        }
        currentSegment = 0;
        UpdateHPBar();
    }


    public void TakeDamage(float damage)
    {
        while (damage > 0 && currentSegment < segmentHP.Length)
        {
            if (segmentHP[currentSegment] > damage)
            {
                segmentHP[currentSegment] -= damage;
                damage = 0;
            }
            else
            {
                damage -= segmentHP[currentSegment];
                segmentHP[currentSegment] = 0;
                currentSegment++;
            }
        }
        UpdateHPBar();
        StartCoroutine(SmoothUpdateHPBar());
    }
    
    // HPバーをスムーズに更新
    IEnumerator SmoothUpdateHPBar()
    {
        float[] startValues = new float[hpMoveBars.Length];

        // 初期値を取得
        for (int i = 0; i < hpBars.Length; i++)
        {
            startValues[i] = hpMoveBars[i].fillAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < hpBarSpeed)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / hpBarSpeed;

            for (int i = 0; i < hpBars.Length; i++)
            {
                float targetFill = segmentHP[i] / segmentMaxHP[i];
                hpMoveBars[i].fillAmount = Mathf.Lerp(startValues[i], targetFill, progress);
            }

            yield return null; // 次のフレームまで待つ
        }
    }


    // HPバーを更新
    private void UpdateHPBar()
    {
        for (int i = 0; i < hpBars.Length; i++)
        {
            hpBars[i].fillAmount = segmentHP[i] / segmentMaxHP[i]; // 各ゲージの割合を反映
        }
    }

    public float GetTotalMaxHP()
    {
        float total = 0;
        foreach (float hp in segmentMaxHP)
        {
            total += hp;
        }
        return total;
    }
}
