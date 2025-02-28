using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnockOutEffect : MonoBehaviour
{
    public Image winImage;
    public Image loseImage;

    public AudioClip winSound;
    public AudioClip loseSound;

    private AudioSource audioSource;

    void Start()
    {
        winImage.enabled = false;
        loseImage.enabled = false;

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    public void ShowLoseEffect()
    {
        loseImage.enabled = true;
        if (loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }
    }

    public void ShowWinEffect()
    {
        winImage.enabled = true;
        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }
}
