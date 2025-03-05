using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMaterialChange : MonoBehaviour
{
    [SerializeField] Material[] backpackMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] bootsMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] capMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] headMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] maleMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] pantsMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] shirtMaterials;  // 変更するマテリアルを入れる
    [SerializeField] Material[] vestMaterials;  // 変更するマテリアルを入れる
    [SerializeField] new Renderer[] renderer; // マテリアルを変更するレンダー

    public static PlayerMaterialChange Instance;
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
    public void ChangeMaterial(int i)
    {
        renderer[0].material = backpackMaterials[i];
        renderer[1].material = bootsMaterials[i];
        renderer[2].material = capMaterials[i];
        renderer[3].material = headMaterials[i];
        renderer[4].material = maleMaterials[i];
        renderer[5].material = pantsMaterials[i];
        renderer[6].material = shirtMaterials[i];
        renderer[7].material = vestMaterials[i];
    }

    /// <summary>
    /// マテリアルを初期に戻す
    /// </summary>
    public void ReturnMaterial()
    {
        renderer[0].material = backpackMaterials[0];
        renderer[1].material = bootsMaterials[0];
        renderer[2].material = capMaterials[0];
        renderer[3].material = headMaterials[0];
        renderer[4].material = maleMaterials[0];
        renderer[5].material = pantsMaterials[0];
        renderer[6].material = shirtMaterials[0];
        renderer[7].material = vestMaterials[0];
    }
}
