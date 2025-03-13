using UnityEngine;

public class EyeLineEffect : MonoBehaviour
{
    public ParticleSystem eyeGlowEffect;
    public Animator characterAnimator; // キャラの `Animator` をセット
    public Vector3 eyeOffset = new Vector3(0, 0.2f, 0.1f); // 目のオフセット

    void Update()
    {
        if (characterAnimator != null)
        {
            // **Animator から "Head" のボーンを取得**
            Transform headBone = characterAnimator.GetBoneTransform(HumanBodyBones.Head);
            if (headBone != null)
            {
                transform.position = headBone.position + eyeOffset;
            }
        }
    }

    public void ActivateGlow()
    {
        if (eyeGlowEffect != null)
        {
            eyeGlowEffect.Play();
        }
    }

    public void DeactivateGlow()
    {
        if (eyeGlowEffect != null)
        {
            eyeGlowEffect.Stop();
        }
    }
}
