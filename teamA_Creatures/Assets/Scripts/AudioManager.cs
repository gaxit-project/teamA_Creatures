using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("")]
    public AudioSource playerAttackAudioSource;
    public AudioSource enemyAttackAudioSource;
    public AudioSource playerMovingAudioSource;
    public AudioSource enemyMovingAudioSource;
    public AudioSource settingSceneAudioSource;
    public AudioSource voiceAudioSource;
    public AudioSource BGMAudioSource;

    [SerializeField] AudioClip[] SE_List;

    private Dictionary<string, int> SE_Index = new Dictionary<string, int>
    {
        { "playerAttack", 0 },
        { "enemyAttack", 1 },
        { "playerMove", 2 },
        { "enemyMove", 3 },
        { "settingScene", 4 },
        { "voice", 5 },
        { "BGM", 6 }
    };

    public void PlaySE(string audioSourceName)
    {
        int index = SE_Index[audioSourceName];

        AudioClip clip = SE_List[index];

        switch (audioSourceName)
        {
            case "playerAttack":
                playerAttackAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "enemyAttack":
                enemyAttackAudioSource.PlayOneShot(SE_List[index]);
                break;
            
            case "settingScene":
                settingSceneAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "voice":
                voiceAudioSource.PlayOneShot(SE_List[index]);
                break;
        }
    }

    public void PlayLoopSE(string audioSourceName)
    {
        int index = SE_Index[audioSourceName];

        AudioClip clip = SE_List[index];

        switch (audioSourceName)
        {
            case "playerMove":
                playerMovingAudioSource.Play();
                break;
            case "enemyMove":
                enemyMovingAudioSource.Play();
                break;
        }
    }

    public void StopLoopSE(string audioSourceName)
    {
        int index = SE_Index[audioSourceName];

        AudioClip clip = SE_List[index];

        switch (audioSourceName)
        {
            case "playerMove":
                playerMovingAudioSource.Stop();
                break;
            case "enemyMove":
                enemyMovingAudioSource.Stop();
                break;
        }
    }

    public void PlayBGM(string audioSourceName)
    {
        int index = SE_Index[audioSourceName];

        AudioClip clip = SE_List[index];

        switch (audioSourceName)
        {
            case "BGM":
                BGMAudioSource.clip = clip;
                BGMAudioSource.Play();
                break;
        }
    }
}
