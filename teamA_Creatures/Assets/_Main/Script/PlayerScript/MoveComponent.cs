using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
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

    public bool FrontStepReady = false;
    private float lastFrontInputTime = 0f;
    private float FrontStepThreshold = 0.2f;
    public bool prevFrontInput = false;

    public float teleportDistance = 5f;
    private float lastTeleportTime = -Mathf.Infinity;
    public float teleportSpeed = 20f; // テレポート中の移動速度

    // シーン内の地形範囲を設定 (必要に応じて調整)
    public Bounds stageBounds;


    private bool isTeleporting = false;

    public GameObject Zanzou;
    private GameObject zan;

    public bool isZanCoroutine = false;

    public bool isguardBreak = false;
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


        bool isFrontInput = (left && Speed < 0) || (!left && Speed > 0);

        if (isFrontInput && !prevFrontInput)
        {
            if (FrontStepReady && Time.time - lastFrontInputTime <= FrontStepThreshold && !DriveGauge.Instance.isDriveGaugeZero && !isZanCoroutine)
            {
                StartCoroutine(FrontStep());
                PlayerHP.Instance.muteki = true;
                DriveGauge.Instance.GaugeDown("avoid");
                FrontStepReady = false;
            }
            lastFrontInputTime = Time.time;
        }
        else if (!isFrontInput && prevFrontInput)
        {
            FrontStepReady = true;
        }
        prevFrontInput = isFrontInput;


        Debug.Log(Speed);
        if (Mathf.Abs(Speed) > Mathf.Abs(Down) || Down > 0)
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
        if (JumpComponent.Instance.jumpFlag && (Speed != 0 || Down != 0))
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

                AudioManager.GetInstance().PlayLoopSE("playerMove", 0);
                // AudioManager.GetInstance().PlayLoopSE("playerMove");  ←これ音止める奴
                ///<summary>
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.clip = runningSound;
                //    audioSource.loop = true; // 音をループ再生
                //    audioSource.Play();

                animator.SetBool("Shield", false);
                if (ATFieldNow)
                {
                    Shield.Instance.OffShield();
                }
            }
            else if (Down < -0.4)
            {
                animator.SetBool("Shield", true);
                moveF = false;
                moveB = false;
                runningNow = false;
                if (audioSource.isPlaying)
                {
                    audioSource.Stop();
                }

                if (!ATFieldNow /*&& !isguardBreak*/)
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
        if (Mathf.Abs(Speed) > 0.4)
        {
            Runwait = 0;

        }

        if (runningNow)
        {
            animator.SetBool("run", true);
        }
        else
        {
            animator.SetBool("run", false);
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
    public float zanntime;
    public float zantime = 0.2f;
    public bool zannnnn;
    public void zann()
    {
        zanntime = Time.time;
        zannnnn = true;
        while (zanntime + zantime > Time.time)
        {
            return;

        }


    }
    public IEnumerator FrontStep()
    {
        isTeleporting = true;
        isZanCoroutine = true;
        PlayerMaterialChange.Instance.ChangeMaterial(3);

        if (left)
        {
            zan = Instantiate(Zanzou, transform.position, Quaternion.Euler(0, 90, 0));
            //zann();

        }
        if (!left)
        {
            zan = Instantiate(Zanzou, transform.position, Quaternion.Euler(0, -90, 0));
            //zann();
        }

        // 走るアニメーションを開始
        animator.SetBool("FowardStep", true);

        // テレポート方向の決定
        Vector3 teleportDirection = transform.forward;


        // デフォルトの移動距離を設定
        float targetDistance = teleportDistance;



        Debug.DrawRay(transform.position, teleportDirection * teleportDistance, Color.red, 1f);

        // Raycast で障害物までの距離を確認
        RaycastHit hit;
        if (Physics.Raycast(transform.position, teleportDirection, out hit, teleportDistance))
        {
            // 障害物がある場合、手前で止まる
            targetDistance = hit.distance - 0.1f;
            Debug.Log($"障害物検出: {hit.collider.name} までの距離: {targetDistance}");
        }

        Vector3 startPosition = transform.position;

        Vector3 targetPosition = startPosition + teleportDirection * teleportDistance;


        float time = 0f;
        float duration = teleportDistance / teleportSpeed; // 時間 = 距離 ÷ 速度

        // Lerpでスムーズに移動
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // 最終位置を補正
        transform.position = targetPosition;


        Destroy(zan);

        // 走るアニメーションを終了
        animator.SetBool("FowardStep", false);
        PlayerHP.Instance.muteki = false;
        PlayerMaterialChange.Instance.ReturnMaterial();

        isZanCoroutine = false;
        isTeleporting = false;
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