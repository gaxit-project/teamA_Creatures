using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
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

    public GameObject Enemy;
    public GameObject Player;

    public Vector3 PlayerPosX;
    public Vector3 EnemyPosX;
    public float dis;
    private float InDis;

    public bool moveF;
    public bool moveB;
    public bool moveBNow;
    public bool moveFNow;

    public float moveFSpeed;
    public float moveBSpeed;
    public float moveFJumpSpeed;
    public float moveBJumpSpeed;

    public bool MoveRun;


    public bool backStepReady = false;
    private float lastBackInputTime = 0f;
    private float backStepThreshold = 0.2f; // バックステップ発動猶予時間
    public bool prevBackInput = false; // 前回の入力状態
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
    public float Speed;
    /// <summary>
    /// DownよりSpeedの値が多ければ移動する
    /// </summary>
    /// <param name="Speed"></param>
    /// <param name="Down"></param>
    public void MoveHorizontal(float Speed, float Down)
    {
        bool isBackInput = (!left && Speed < 0) || (left && Speed > 0); // 後ろ入力判定

        // バックステップ判定
        if (isBackInput && !prevBackInput) // 後ろ入力を新しく押した瞬間
        {
            if (backStepReady && Time.time - lastBackInputTime <= backStepThreshold)
            {
                StartCoroutine(BackStep());
                backStepReady = false;
            }
            lastBackInputTime = Time.time;
        }
        else if (!isBackInput && prevBackInput) // 後ろ入力を離した瞬間
        {
            backStepReady = true;
        }

        prevBackInput = isBackInput;

        // バックステップ中は移動を受け付けない
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("BackStep"))
        {
            return;
        }


        Debug.Log(Speed);
        if (Mathf.Abs(Speed) > Mathf.Abs(Down)||Down>0)
        {
            if (moveF && JumpComponent.Instance.jumpFlag)
            {
                transform.Translate(transform.TransformDirection(new Vector2(Speed, 0) * moveFSpeed * Time.deltaTime));
                Debug.Log("flont");
            }
            else if (moveB && JumpComponent.Instance.jumpFlag)
            {
                transform.Translate(transform.TransformDirection(new Vector2(Speed, 0) * moveBSpeed * Time.deltaTime));
                Debug.Log("Back");
            }
            else if (moveFNow && !JumpComponent.Instance.jumpFlag)
            {
                transform.Translate(transform.TransformDirection(new Vector2(Speed, 0) * moveFJumpSpeed * Time.deltaTime));

            }
            else if (moveBNow && !JumpComponent.Instance.jumpFlag)
            {
                transform.Translate(transform.TransformDirection(new Vector2(Speed, 0) * moveBJumpSpeed * Time.deltaTime));
            }
        }
        if (JumpComponent.Instance.jumpFlag&&(Speed!=0||Down!=0))
        {
            if (Mathf.Abs(Speed) > -Down)
            {
                if (!runningNow)
                {
                    StartCoroutine(RunNow());
                }

                if (!left)
                {
                    if (Speed > 0)
                    {
                        moveF = true;
                        moveB = false;
                    }
                    else if (Speed < 0)
                    {
                        moveF = false;
                        moveB = true;
                        //StartCoroutine(BackStep(Speed));
                    }
                }
                else
                {
                    if (Speed > 0)
                    {
                        moveF = false;
                        moveB = true;
                        //StartCoroutine(BackStep(Speed));
                    }
                    else if (Speed < 0)
                    {
                        moveF = true;
                        moveB = false;
                    }
                }
                
                //AudioManager.GetInstance().PlayLoopSE("playerMove",0);
                ///<summary>
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = runningSound;
                    audioSource.loop = true; // 音をループ再生
                    audioSource.Play();
                }
                animator.SetBool("Shield", false);
                if (ATFieldNow)
                {
                    Shield.Instance.OffShield();
                }
            }
            else if ( Down < -0.4)
            {
                animator.SetBool("Shield", true);
                moveF = false;
                moveB = false; 
                runningNow = false;
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
            moveF = false;
            moveB = false;

            if (!left)
            {
                if (Speed > 0)
                {
                    moveFNow = true;
                    moveBNow = false;
                }
                else if (Speed < 0)
                {
                    moveFNow = false;
                    moveBNow = true;
                }
            }
            else
            {
                if (Speed > 0)
                {
                    moveFNow = false;
                    moveBNow = true;
                }
                else if (Speed < 0)
                {
                    moveFNow = true;
                    moveBNow = false;
                }
            }
            


            animator.SetBool("Shield", false);
            //AudioManager.GetInstance().StopLoopSE("playerMove");
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
            Runwait += 0.25f * Time.deltaTime;
        }
        if (Mathf.Abs(Speed)> 0.4)
        {
            Runwait = 0;
            
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

    public IEnumerator BackStep()
    {
        float backStepSpeed = 8f;
        float backStepTime = 0.2f;

        Vector3 backDir = !left ? Vector3.right : Vector3.left; // 方向決定
        float moveDistance = backStepSpeed * backStepTime;

        // Raycastで後ろ方向に障害物があるかチェック
        if (!Physics.Raycast(transform.position, backDir, moveDistance))
        {
            float timer = 0f;
            while (timer < backStepTime)
            {
                transform.Translate(backDir * backStepSpeed * Time.deltaTime, Space.World);
                timer += Time.deltaTime;
                yield return null;
            }
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
            yield return null;
        }
        runningNow = false;
    }

    void Update()
    {

        if (Player.transform.position.x < Enemy.transform.position.x)
        {
            left = true;
        }
        else if (Player.transform.position.x > Enemy.transform.position.x)
        {
            left = false;
        }

        if (JumpComponent.Instance.jumpFlag)
        {
            Vector3 EnemyPosition = new Vector3(Enemy.transform.position.x, transform.position.y, transform.position.z);
            transform.LookAt(EnemyPosition);
        }


            if (moveF)
            {
                animator.SetBool("run", true);
                animator.SetBool("Buckrun", false);
            }
            else if (moveB)
            {
                animator.SetBool("run", false);
                animator.SetBool("Buckrun", true);
            }
            else
            {
                animator.SetBool("run", false);
                animator.SetBool("Buckrun", false);
            }
        

    }

}
