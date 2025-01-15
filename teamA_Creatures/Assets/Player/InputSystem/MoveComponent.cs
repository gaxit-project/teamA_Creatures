using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveComponent : MonoBehaviour
{
    public static MoveComponent Instance;

    Animator animator;
    AudioSource audioSource;

    public float moveSpeed = 10f;
    public bool left;
    public bool ATFieldNow;
    public AudioClip runningSound;
    public bool runningNow;
    public float RunMWait;
    public float Runwait;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        left = false;
        ATFieldNow = false;
        runningNow = false;
    }

    /// <summary>
    /// Down‚æ‚èSpeed‚Ì’l‚ª‘½‚¯‚ê‚ÎˆÚ“®‚·‚é
    /// </summary>
    /// <param name="Speed"></param>
    /// <param name="Down"></param>
    public void MoveHorizontal(float Speed, float Down)
    {
        if (Mathf.Abs(Speed) > Mathf.Abs(Down)||Down>0)
        {
            transform.Translate(transform.TransformDirection(new Vector2(Speed, 0) * moveSpeed * Time.deltaTime));

        }
        if (JumpComponent.Instance.jumpFlag&&(Speed!=0||Down!=0))
        {
            if (Mathf.Abs(Speed) > -Down)
            {
                if (!runningNow)
                {
                    StartCoroutine(RunNow());
                }


                Debug.Log("run");
                animator.SetBool("run", true);
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = runningSound;
                    audioSource.loop = true; // ‰¹‚ðƒ‹[ƒvÄ¶
                    audioSource.Play();
                }
                animator.SetBool("Shield", false);
                if (ATFieldNow)
                {
                    Shield.Instance.OffShield();
                }
            }
            else
            {
                Debug.Log("Shield");
                animator.SetBool("Shield", true);
                animator.SetBool("run", false);

                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                if (!ATFieldNow)
                {
                    Shield.Instance.OnShield();
                }
            }
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("Shield", false);

            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            if (ATFieldNow)
            {
                Shield.Instance.OffShield();
            }
        }
        if (Mathf.Abs(Speed) == 0)
        {
            Runwait += 0.1f*Time.deltaTime;
        }
        if (Mathf.Abs(Speed)> 0.4)
        {
            Runwait = 0;
            left = Speed > 0;
            transform.rotation = Quaternion.Euler(0, left ? -90 : 90, 0);
        }
        if (runningNow)
        {
            animator.SetBool("run",true);
        }
        else
        {
            animator.SetBool("run",false);
        }
    }

    private IEnumerator RunNow()
    {
        runningNow = true;
        while (RunMWait > Runwait)
        {
            if (Runwait == 0f)
            {
                yield return null;
                continue;
            }
            Runwait += Time.deltaTime;
            yield return null;
        }
        runningNow = false;
    }
}
