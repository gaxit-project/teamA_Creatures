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
    public AudioSource playerRunAudioSource;
    public AudioSource enemyRunAudioSource;
    public AudioSource settingSceneAudioSource;
    public AudioSource playerVoiceAudioSource;
    public AudioSource enemyVoiceAudioSource;
    public AudioSource BGMAudioSource;

    [SerializeField] AudioClip[] SE_List;
    [SerializeField] AudioClip[] BGM_List;

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

    public void PlaySE(string audioSourceName,int index)
    {
        switch (audioSourceName)
        {
            case "playerAttack":
                playerAttackAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "enemyAttack":
                enemyAttackAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "playerMove":
                enemyAttackAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "enemyMove":
                enemyAttackAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "settingScene":
                settingSceneAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "playrVoice":
                playerVoiceAudioSource.PlayOneShot(SE_List[index]);
                break;
            case "enemyVoice":
                enemyVoiceAudioSource.PlayOneShot(SE_List[index]);
                break;
        }
    }

    public void PlayLoopSE(string audioSourceName, int index)
    {
        switch (audioSourceName)
        {
            case "playerMove":
                playerRunAudioSource.clip = SE_List[index];
                playerRunAudioSource.Play();
                break;
            case "enemyMove":
                enemyRunAudioSource.clip = SE_List[index];
                enemyRunAudioSource.Play();
                break;
        }
    }

    public void StopLoopSE(string audioSourceName)
    {
        switch (audioSourceName)
        {
            case "playerMove":
                playerRunAudioSource.Stop();
                break;
            case "enemyMove":
                enemyRunAudioSource.Stop();
                break;
        }
    }

    public void PlayBGM(string audioSourceName, int index)
    {
        switch (audioSourceName)
        {
            case "BGM":
                BGMAudioSource.clip = BGM_List[index];
                BGMAudioSource.Play();
                break;
        }
    }


    public static AudioManager Instance = null;
    public static AudioManager GetInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<AudioManager>();
        }
        return Instance;
    }
}
