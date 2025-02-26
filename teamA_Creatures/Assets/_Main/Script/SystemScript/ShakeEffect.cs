using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoiseEffect : MonoBehaviour
{
    public TMP_Text textMeshPro;
    public float shakeIntensity = 2f; // 揺れの強さ
    public float shakeSpeed = 10f;    // 揺れの速さ

    void Start()
    {
        StartCoroutine(ShakeText());
    }

    IEnumerator ShakeText()
    {
        TMP_TextInfo textInfo = textMeshPro.textInfo;

        while (true)
        {
            textMeshPro.ForceMeshUpdate(); // メッシュ情報を更新
            textInfo = textMeshPro.textInfo; // 更新後のテキスト情報を取得

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                Vector3[] vertices = textInfo.meshInfo[i].vertices;

                for (int j = 0; j < textInfo.characterCount; j++)
                {
                    if (!textInfo.characterInfo[j].isVisible) continue;

                    int vertexIndex = textInfo.characterInfo[j].vertexIndex;

                    float noise = Mathf.Sin(Time.time * shakeSpeed + j) * shakeIntensity;
                    for (int k = 0; k < 4; k++) // 各文字の4つの頂点を揺らす
                    {
                        vertices[vertexIndex + k] += new Vector3(noise, noise, 0);
                    }
                }

                textMeshPro.UpdateVertexData(TMP_VertexDataUpdateFlags.All); // メッシュの更新
            }

            yield return null;
        }
    }
}
