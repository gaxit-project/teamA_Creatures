using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;
using UnityEngine.UI;

public class KnockOutEffect : MonoBehaviour
{
    public Image winImage;
    public Image loseImage;

    public AudioClip winSound;
    public AudioClip loseSound;

    private AudioSource audioSource;
    private bool isLosing = false;

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
        if (isLosing == false)
        {
            StartCoroutine(Lose());
        }
    }

    public void ShowWinEffect()
    {
        StartCoroutine(Win());
       
    }

    IEnumerator Win()
    {
        yield return new WaitForSeconds(4f);
        winImage.enabled = true;
        if (winSound != null)
        {
            audioSource.PlayOneShot(winSound);
        }
    }

    IEnumerator Lose()
    {
        isLosing = true;
        yield return new WaitForSeconds(2f);
        loseImage.enabled = true;
        if (loseSound != null)
        {
            audioSource.PlayOneShot(loseSound);
        }
    }

    public void ResetEffect()
    {
        isLosing = false;
        winImage.enabled = false;
        loseImage.enabled = false;
    }

}
