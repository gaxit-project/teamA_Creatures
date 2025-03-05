using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaterialChange : MonoBehaviour
{
    [SerializeField] Material[] materials;  // 変更するマテリアルを入れる
    [SerializeField] new Renderer renderer; // マテリアルを変更するレンダー
    public bool isHitStopStop = false;

    public static EnemyMaterialChange Instance;
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
        ReturnMaterial();
    }

    /// <summary>
    /// マテリアルを更新する
    /// </summary>
    /// <param name="i">格納番号</param>
    public void ChangeMaterial(int i)
    {
        isHitStopStop = true;
        renderer.material = materials[i];
    }

    /// <summary>
    /// マテリアルを初期に戻す
    /// </summary>
    public void ReturnMaterial()
    {
        isHitStopStop = false;
        renderer.material = materials[0];
    }
}
