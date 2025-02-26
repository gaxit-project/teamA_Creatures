using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class NewEnemyMove : MonoBehaviour
{
    // 敵のステータス関連
    public static float enemyHP; // 敵のHP
    public static float enemyInitialHP = 500f; // 敵のHP
    int _enemyGaurd;
    [Header("Enemyステータス")]
    [SerializeField] float speed = 1; // 敵の動くスピード
    [SerializeField] float backSpeed = 2; // 敵の動くスピード
    public bool isFollow = false; // 追従するかどうかのフラグ
    Transform _playerTr; // プレイヤーのTransform
    Rigidbody _rb; // このオブジェクトの Rigidbody
    float _distanceAway = 5f; // プレイヤーから離れる距離
    float _distanceApp = 1.5f; // プレイヤーに近づく距離
    float _distancePtoE;  // エネミーとプレイヤーの距離を入れる変数
    float forwardSpeed = 1.1f;  // 攻撃するときに前進する速度
    float tackleSpeed = 5f;  // 攻撃するときに前進する速度
    float backWallSpeed = 3f;  // プレイヤーが壁際にいるときの後退する速度
    float _acceleration = 3f; // タックル攻撃の加速度
    float stepSpeed = 10f;  // ステップの前進する速度
    float _tackleAcceleration = 0f;
    // プレイヤーとの距離関連
    [Header("Enemy距離計測")]
    public float shortDistance = 2f; // 近距離を測る変数
    public float middleDistance = 6f; // 中距離を測る変数
    public float longDistance = 12f; // 遠距離を測る変数
    private float _targetDistance; // 現在の目標距離
    float _shortTime = 0f;
    float _middleTime = 0f;
    float _longTime = 0f;
    // 現在の状態を保持する変数
    private EnemyState _currentState;
    EnemyState _nextCurrent;
    private bool _isCoroutineRunning = false; // コルーチン実行中かどうか

    Animator _enemyAnim; // Animatorコンポーネント
    bool _isAnimActive = true; // フラグの初期状態


    Vector2 _directionX;


    bool _isAtackEnd = false;
    private EnemyHit cubeController; // EnemyAttack スクリプトの参照

    private float _moveTimer = 0f; // 経過時間
    private float _moveMaxTimer = 3f; // 退却の最大時間

    public chain Chain;
    public EnemyHit2 EnemyHit2;

    public JudgeManager JM;
    public PlayerEffect PE;

    public static bool _isTackle = false;
    bool _shortDistance = false;

    float _attackStiffnessMin = 0.3f;
    float _attackStiffnessMax = 0.5f;

    Coroutine currentCoroutine;

    bool isCoroutineStop = false;
    public static bool isEnemyStanFlag = false;

    public static NewEnemyMove Instance;

    bool isBeastMode = true;
    bool isBMJudge = false;

    bool isGameOverFlag = false;


    // ふっとばし攻撃関連
    public float smashForce = 50f; // 吹っ飛ばす力
    public Vector3 blowDirection = new Vector3(1, 1, 0);
    public bool isPushWall = false;

    float StanMaxTime = 10f;

    public static int damege = 10;

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
    /// <summary>
    /// エネミーの列挙型
    /// </summary>
    enum EnemyState
    {
        Idle,             // 待機
        MiddleFollow,     // 中距離まで追跡
        ShortFollow,      // 近距離まで追跡
        MiddleRetreat,    // 中距離まで退避
        LongRetreat,      // 遠距離まで退避
        Guard,            // ガード
        RightPunch,       // 右近距離攻撃
        LeftPunch,        // 左近距離攻撃
        Smash,            // ガードブレイク攻撃
        Tackle,           // 中距離攻撃
        Chain,            // 遠距離攻撃
        Walk,             // 歩いている
        BackStep,         // 後ろステ
        ForwardStep,      // 前ステ
        BackAttack,       // バックアタック
        HitStan,          // 攻撃ヒット時
        Stan,             // カウンター時のスタン
        BeastMode,        // ビーストモード
        Down,             // 敵死亡時
        Push,             // 吹き飛ばし処理  
        Counter           // カウンター処理
    }
    #region スタートたち
    void Start()
    {
        enemyHP = enemyInitialHP;
        isFollow = false;
        _isTackle = false;
        _isAtackEnd = false;
        _shortDistance = false;
        _isAnimActive = true;
        _isCoroutineRunning = false;
        transform.position = new Vector3(4, 0, 0);
        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        // リジットボディの設定
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezePositionZ  | RigidbodyConstraints.FreezeRotation;
        // エネミーとプレイヤーの距離計測
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
        _enemyAnim = GetComponent<Animator>(); // Animatorを取得
        cubeController = GetComponent<EnemyHit>();
    }
    #endregion

    #region アップデートたち
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            _currentState = EnemyState.Push;
            isCoroutineStop = true;
        }
        if (Input.GetKey(KeyCode.P))
        {
            _currentState = EnemyState.Down;
            isCoroutineStop = true;
        }
        // 互いの距離計測 + 敵の向き交換
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
        Debug.Log("距離計測中");
        _directionX = _playerTr.position - transform.position;

        //DebugKey();
        StateTime();
        switch (_currentState)
        {
            case EnemyState.Idle:
                Debug.Log("アイドルだよ～ん");
                HandleIdle();
                break;
            case EnemyState.MiddleFollow:
                HandleMiddleFollow();
                break;
            case EnemyState.ShortFollow:
                HandleShortFollow();
                break;
            case EnemyState.MiddleRetreat:
                HandleMiddleRetreat();
                break;
            case EnemyState.LongRetreat:
                HandleLongRetreat();
                break;
            case EnemyState.Guard:
                HandleGuard();
                break;
            case EnemyState.RightPunch:
                HandleRightPunch();
                break;
            case EnemyState.LeftPunch:
                HandleLeftPunch();
                break;
            case EnemyState.Smash:
                HandleSmash();
                break;
            case EnemyState.Tackle:
                HandleTackle();
                break;
            case EnemyState.Chain:
                HandleChain();
                break;
            case EnemyState.ForwardStep:
                HandleForwardStep();
                break;
            case EnemyState.BackStep:
                HandleBackStep();
                break;
            case EnemyState.BackAttack:
                HandleBackAttack();
                break;
            case EnemyState.HitStan:
                HandleHitStan();
                break;
            case EnemyState.Stan:
                HandleStan();
                break;
            case EnemyState.BeastMode:
                HandleBeastMode();
                break;
            case EnemyState.Down:
                HandleDown();
                break;
            case EnemyState.Push:
                HandlePush();
                break;
            case EnemyState.Counter:
                HandleCounter();
                break;
        }
    }
    #endregion

    #region 状態遷移を管理するプログラムたち
    void HandleIdle()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyIdle());
    }
    void HandleMiddleFollow()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(PlayerFollow(_targetDistance));
    }
    void HandleShortFollow()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(PlayerFollow(_targetDistance));
    }
    void HandleMiddleRetreat()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(PlayerRetreat(_targetDistance));
    }
    void HandleLongRetreat()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(PlayerRetreat(_targetDistance));
    }
    void HandleGuard()
    {
        EnemyGaurd();
        _currentState = EnemyState.Idle;
    }
    void HandleRightPunch()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyRightPunch());
        //_currentState = EnemyState.Idle;
    }
    void HandleLeftPunch()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyLeftPunch());
        //_currentState = EnemyState.Idle;
    }
    void HandleSmash()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemySmash());
        //_currentState = EnemyState.Idle;
    }
    void HandleTackle()
    {
        Debug.Log("タックル呼び出し処理開始！！！");
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyTackle());
        //_currentState = EnemyState.Idle;
    }
    void HandleChain()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyChain());
        //_currentState = EnemyState.Idle;
    }
    void HandleForwardStep()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyStep("forward"));
    }
    void HandleBackStep()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyStep("back"));
    }
    void HandleBackAttack()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyBackAttack());
    }
    void HandleHitStan()
    {
        if (isCoroutineStop)
        {
            isCoroutineStop = false;
            StopCoroutine(currentCoroutine);
            _isCoroutineRunning = false;
        }
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyHitStan());
    }
    void HandleStan()
    {
        if(isCoroutineStop)
        {
            isCoroutineStop = false;
            StopCoroutine(currentCoroutine);
            _isCoroutineRunning = false;
        }
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyStan());
    }
    void HandleBeastMode()
    {
        if (isCoroutineStop)
        {
            isCoroutineStop = false;
            StopCoroutine(currentCoroutine);
            _isCoroutineRunning = false;
        }
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyBeastMode());
    }
    void HandleDown()
    {
        if (isCoroutineStop)
        {
            isCoroutineStop = false;
            StopCoroutine(currentCoroutine);
            _isCoroutineRunning = false;
        }
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyDown());
    }
    void HandlePush()
    {
        if (isCoroutineStop)
        {
            isCoroutineStop = false;
            StopCoroutine(currentCoroutine);
            _isCoroutineRunning = false;
        }
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyPush());
    }

    void HandleCounter()
    {
        if (isCoroutineStop)
        {
            isCoroutineStop = false;
            StopCoroutine(currentCoroutine);
            _isCoroutineRunning = false;
        }
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        currentCoroutine = StartCoroutine(EnemyCounter());
    }
    /// <summary>
    /// 距離による状態遷移
    /// </summary>
    void UpdateState()
    {
        int randomState = Random.Range(1, 101);
        //Debug.Log("乱数:" + randomState);
        // 距離に基づく状態遷移
        if (_distancePtoE < middleDistance)
        {
            if (EnemyRayCast.isBackWallSmash)
            {
                EnemyRayCast.isBackWallSmash = false;
                _currentState = EnemyState.Smash;
            }
            else if (_shortTime >= 15f)
            {
                if (randomState <= 50 && !EnemyRayCast.isBackWall)
                {
                    _currentState = EnemyState.BackAttack; // バクステパンチ
                    _shortTime = 5f;
                }
                else if (randomState <= 100)
                {
                    _currentState = EnemyState.BackStep; // バクステたっこー
                    _shortTime = 5f;
                }
            }
            else if (randomState <= 65)
            {
                _currentState = EnemyState.RightPunch; // 近距離で攻撃
            }
            else if (randomState <= 70)
            {
                _currentState = EnemyState.Smash; // 近距離で攻撃
            }
            else if (randomState <= 85 && !EnemyRayCast.isBackWall)
            {
                _currentState = EnemyState.BackAttack; // バクステパンチ
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.BackStep; // バクステたっこー
            }
        }
        // 中距離にいるときの処理
        else if (_distancePtoE >= middleDistance && _distancePtoE < longDistance)
        {
            if (randomState <= 40)
            {
                _currentState = EnemyState.ForwardStep; // 前ステ攻撃
                _shortDistance = true;
            }
            else if (randomState <= 80)
            {
                _currentState = EnemyState.BackStep; // バクステたっこー
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.Smash; // 近距離で攻撃
            }
        }
        // 遠距離にいるときの処理
        else if (_distancePtoE >= longDistance)
        {
            if (randomState <= 50)
            {
                //_currentState = EnemyState.Tackle; // チェーン投げで攻撃
                _currentState = EnemyState.ForwardStep; // 前ステップ
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.BackStep; // バクステたっこー
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.ForwardStep; // 中距離まで追跡
                _targetDistance = middleDistance + 2f;
            }
        }
        else
        {
            if (randomState <= 100)
            {
                _currentState = EnemyState.Tackle; // タックルで攻撃
            }
            //_currentState = EnemyState.Idle; // 距離が遠すぎる場合は待機
        }
    }

    void StateTime()
    {
        if (_distancePtoE < middleDistance)
        {
            _middleTime = 0f;
            _longTime = 0f;
            _shortTime += Time.deltaTime;
        }
        else if (_distancePtoE >= middleDistance && _distancePtoE < longDistance)
        {
            _shortTime = 0f;
            _longTime = 0f;
            _middleTime += Time.deltaTime;
        }
        else if (_distancePtoE >= longDistance)
        {
            _shortTime = 0f;
            _middleTime = 0f;
            _longTime += Time.deltaTime;
        }
    }
#endregion

    #region 攻撃の処理たち

    /// <summary>
    /// パンチ攻撃
    /// </summary>
    IEnumerator EnemyRightPunch()
    {
        Debug.Log("右パンチ！");
        _isCoroutineRunning = true;
        _enemyAnim.SetBool("RightPunch", true);
        _enemyAnim.CrossFade("RightPunch", 0.1f);
        AudioManager.GetInstance().PlaySE("enemyAttack", 4);
        while (true)
        {
            // 現在のアニメーションステート情報を取得
            AnimatorStateInfo currentState = _enemyAnim.GetCurrentAnimatorStateInfo(0);
            // 敵を前進させる
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            if (_isAtackEnd)
            {
                break;
            }
            // フレーム間の待機
            yield return null;
        }
        EnemyEndAttack();
        // 現在のアニメーションが終了したかを確認
        _isAtackEnd = false;
        _isAnimActive = false; // フラグをオフにする
        _enemyAnim.SetBool("RightPunch", false);
        Debug.Log("右パンチ終了");
        yield return null;
        _currentState = EnemyState.LeftPunch;
        _isCoroutineRunning = false;
    }

    /// <summary>
    /// パンチ攻撃
    /// </summary>
    IEnumerator EnemyLeftPunch()
    {
        Debug.Log("左パンチ！");
        _isCoroutineRunning = true;
        _enemyAnim.SetBool("LeftPunch", true);
        _enemyAnim.CrossFade("LeftPunch", 0.1f);
        AudioManager.GetInstance().PlaySE("enemyAttack", 4);
        while (true)
        {
            // 現在のアニメーションステート情報を取得
            AnimatorStateInfo currentState = _enemyAnim.GetCurrentAnimatorStateInfo(0);
            // 敵を前進させる
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            if (_isAtackEnd)
            {
                break;
            }
            // フレーム間の待機
            yield return null;
        }
        EnemyEndAttack();
        // 現在のアニメーションが終了したかを確認
        _isAtackEnd = false;
        _isAnimActive = false; // フラグをオフにする

        _enemyAnim.SetBool("LeftPunch", false);
        Debug.Log("左パンチ終了");
        yield return null;
        int smashRnd = Random.Range(1, 10);
        if (smashRnd <= 3)
        {
            _currentState = EnemyState.Smash;
        }
        else
        {
            // 攻撃硬直
            _attackStiffnessMin = 0.5f;
            _attackStiffnessMax = 0.8f;
            _currentState = EnemyState.Idle;
        }
        _isCoroutineRunning = false;
    }
    /// <summary>
    /// ガードブレイク攻撃
    /// </summary>
    IEnumerator EnemySmash()
    {
        Debug.Log("ガードブレイク！");
        _isCoroutineRunning = true;
        _isAnimActive = true;
        _enemyAnim.SetBool("Smash", true);
        _enemyAnim.CrossFade("Smash", 0.1f);
        AudioManager.GetInstance().PlaySE("enemyAttack", 4);
        while (true)
        {
            // 現在のアニメーションステート情報を取得
            AnimatorStateInfo currentState = _enemyAnim.GetCurrentAnimatorStateInfo(0);
            // 敵を前進させる
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            if (_isAtackEnd &&/*currentState.normalizedTime >= 1f &&*/ _isAnimActive)
            {
                break;
            }
            // フレーム間の待機
            yield return null;
            Debug.Log("ガードブレイク中です");
        }
        // 現在のアニメーションが終了したかを確認
        _isAtackEnd = false;
        _isAnimActive = false; // フラグをオフにする
        _enemyAnim.SetBool("Smash", false);
        Debug.Log("ガードブレイク終了");
        yield return null;
        // 攻撃硬直
        _attackStiffnessMin = 1f;
        _attackStiffnessMax = 1.5f;
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false;
        yield return null;
    }
    /// <summary>
    /// タックル攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyTackle()
    {
        Debug.Log("タックル！");
        _isCoroutineRunning = true;
        _isTackle = true;
        _enemyAnim.SetBool("Tackle", true);
        _enemyAnim.CrossFade("Tackle", 0.1f);
        tackleSpeed = 5f;
        AudioManager.GetInstance().PlayLoopSE("enemyMove", 0);
        // 現在のアニメーションが終了したかを確認
        while (true)
        {
            // 敵を前進させる
            transform.position += transform.forward * tackleSpeed * Time.deltaTime;
            _tackleAcceleration += Time.deltaTime;
            // 加速を与える
            if (_tackleAcceleration >= 0.3f)
            {
                tackleSpeed += _acceleration;
                _tackleAcceleration = 0;
            }
            float _distancePtoE2 = Vector2.Distance(transform.position, _playerTr.position);
            if (_distancePtoE2 <= shortDistance + 1f || EnemyRayCast.isTackleWall)
            {
                tackleSpeed = 0f;
                break;
            }
            // フレーム間の待機
            yield return null;
        }
        yield return null;
        AudioManager.GetInstance().StopLoopSE("enemyMove");
        EnemyHit2.DestroyCube();
        EnemyRayCast.isTackleWall = false;
        _isTackle = false;
        _enemyAnim.SetBool("Tackle", false);
        Debug.Log("タックル終了");
        // 攻撃硬直
        _attackStiffnessMin = 0.5f;
        _attackStiffnessMax = 0.8f;
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false;
        yield return null;
    }

    /// <summary>
    /// チェーン攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyChain()
    {
        Debug.Log("チェーン！");
        _isCoroutineRunning = true;
        _enemyAnim.SetBool("Chain", true);
        _enemyAnim.CrossFade("Chain", 0.1f);
        yield return new WaitForSeconds(0.8f);
        Chain.ChainAttack();
        //ChainAtack();

        // 現在のアニメーションが終了したかを確認
        while (true)
        {
            // 現在のアニメーションステート情報を取得
            AnimatorStateInfo currentState = _enemyAnim.GetCurrentAnimatorStateInfo(0);
            if (_isAtackEnd /*&&currentState.normalizedTime >= 1f && _isAnimActive*/)
            {
                break;
            }
            // フレーム間の待機
            yield return null;
        }
        _isAtackEnd = false;
        _isCoroutineRunning = false;
        _enemyAnim.SetBool("Chain", false);
        Debug.Log("チェーン終了");
        _currentState = EnemyState.Idle;

    }

    IEnumerator EnemyStep(string around)
    {
        _isCoroutineRunning = true;
        Debug.Log("前ステップ");
        _enemyAnim.SetBool("ForwardStep", true);
        _enemyAnim.CrossFade("ForwardStep", 0.1f);
        AudioManager.GetInstance().PlaySE("enemyAttack", 5);
        while (true)
        {
            switch(around)
            {
                case "forward":
                    // 敵を前進させる
                    transform.position += transform.forward * stepSpeed * Time.deltaTime;
                    if(_shortDistance)
                    {
                        _nextCurrent = EnemyState.RightPunch;
                        _shortDistance = false;
                    }
                    else
                    {
                        _nextCurrent = EnemyState.Idle;
                    }
                    break;
                case "back":
                    // 敵を前進させる
                    transform.position -= transform.forward * stepSpeed * Time.deltaTime;
                    _nextCurrent = EnemyState.Tackle;
                    break;
            }
           
            if (_isAtackEnd)
            {
                break;
            }
            yield return null;
        }
        Debug.Log("前ステップ終了");
        yield return null;
        _isCoroutineRunning = false;
        _isAtackEnd = false;
        _currentState = _nextCurrent;
        yield return null;
    }
    IEnumerator EnemyBackAttack()
    {
        _isCoroutineRunning = true;
        Debug.Log("前ステップ");
        _enemyAnim.SetBool("BackUpper", true);
        _enemyAnim.CrossFade("BackUpper", 0.1f);
        AudioManager.GetInstance().PlayLoopSE("enemyMove", 0);
        while (true)
        {
            if (_isAtackEnd)
            {
                break;
            }
            yield return null;
        }
        Debug.Log("前ステップ終了");
        yield return null;
        _isCoroutineRunning = false;
        _isAtackEnd = false;
        // 攻撃硬直
        _attackStiffnessMin = 0.5f;
        _attackStiffnessMax = 0.8f;
        _currentState = EnemyState.Idle;
        AudioManager.GetInstance().StopLoopSE("enemyMove");
        yield return null;
    }

    void EnemyBackAttackSE(int num)
    {
        AudioManager.GetInstance().PlaySE("enemyAttack",num);
    }

   
    // プレイヤーが壁際の時に攻撃後後ろに下がる
    public void EnemyBack()
    {
        if(PlayerRayCast.isPlayerBackWall)
        {
            PlayerRayCast.isPlayerBackWall = false;
            StartCoroutine(EnemyBackWall());
        }
    }
    IEnumerator EnemyBackWall()
    {
        float backTime = 0f;
        while(true)
        {
            Debug.Log("敵を後退させる");
            backTime += Time.deltaTime;
            // 敵を後退させる
            transform.position -= transform.forward * backWallSpeed * Time.deltaTime;
            yield return null;
            if(backTime >= 0.5f)
            {
                break;
            }
        }
    }

    public void EnemyPushFanction()
    {
        _currentState = EnemyState.Push;
        isCoroutineStop = true;
    }
    IEnumerator EnemyPush()
    {
        _rb.isKinematic = false;
        yield return null;
        float backTime = 0f;
        _isCoroutineRunning = true;
        while(true)
        {
            if(!AttackComponent.Instance.isRush)
            {
                break;
            }
            yield return null;
        }
        _enemyAnim.SetBool("Fly", true);
        _enemyAnim.CrossFade("Fly", 0.1f);
        if (_directionX.x > 0)
        {
            blowDirection = new Vector3(-1, 1, 0);
        }
        else if (_directionX.x < 0)
        {
            blowDirection = new Vector3(1, 1, 0);
        }
        _rb.AddForce(blowDirection.normalized * smashForce, ForceMode.Impulse);
        while (true)
        {
            backTime += Time.deltaTime;
            yield return null;
            if (backTime >= 2f || isPushWall)
            {
                isPushWall = false;
                break;
            }
        }
        _rb.velocity = Vector3.zero;
        backTime = 0f;
        while (true)
        {
            backTime += Time.deltaTime;
            yield return null;
            if (EnemyRayCast.isGround)
            {
                break;
            }
        }
        yield return new WaitForSeconds(1f);
        _enemyAnim.SetBool("Fly", false);
        _isCoroutineRunning = false;
        _rb.isKinematic = true;
        isPushWall = false;
        _currentState = EnemyState.Idle;
    }

    #endregion

    #region 敵の行動＋特殊演出関連
    IEnumerator EnemyIdle()
    {
        _isCoroutineRunning = true;
        if (_directionX.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            _enemyAnim.SetBool("Mirror", false);
            Debug.Log("プレイヤーは右側にいます");
        }
        else if (_directionX.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            _enemyAnim.SetBool("Mirror", true);
            Debug.Log("プレイヤーは左側にいます");
        }
        float _IdleRnd = Random.Range(_attackStiffnessMin, _attackStiffnessMax);
        yield return new WaitForSeconds(_IdleRnd);
        _isCoroutineRunning = false;
        // 距離に基づく状態遷移
        UpdateState();
        Debug.Log("アイドル状態になりました");
    }

    // ひるみの処理
    public void EnemyHitStanState()
    {
        _rb.velocity = Vector3.zero;
        _enemyAnim.SetBool("HitStan", true);
        _enemyAnim.CrossFade("HitStan", 0f);
        isCoroutineStop = true;
        _currentState = EnemyState.HitStan;
    }
    IEnumerator EnemyHitStan()
    {
        _isCoroutineRunning = true;
        float stanTime = 0f;
        EnemyCancel();
        while (true)
        {
            stanTime += Time.deltaTime;
            Debug.Log("スタン中です！！！！");
            if (stanTime >= 0.1f)
            {
                break;
            }
            yield return null;
        }
        Debug.Log("スタン解除！！");
        _enemyAnim.SetBool("HitStan", false);
        _isCoroutineRunning = false;
        _currentState = EnemyState.Idle;
        yield return null;
    }
    // カウンター決められた時の処理
    public void EnemyStanState()
    {
        _rb.velocity = Vector3.zero;
        _enemyAnim.SetBool("Stan", true);
        _enemyAnim.CrossFade("Stan", 0f);
        isEnemyStanFlag = true;
        isCoroutineStop = true;
        _currentState = EnemyState.Stan;
    }
    IEnumerator EnemyStan()
    {
        _isCoroutineRunning = true;
        float stanTime = 0f;
        EnemyCancel();
        while (true)
        {
            stanTime += Time.deltaTime;
            Debug.Log("スタン中です！！！！");
            if (stanTime >= StanMaxTime)
            {
                break;
            }
            yield return null;
        }
        Debug.Log("スタン解除！！");
        _enemyAnim.SetBool("Stan", false);
        _isCoroutineRunning = false;
        isEnemyStanFlag = false;
        _currentState = EnemyState.Idle;
        yield return null;
    }

    /// <summary>
    /// ビーストモードの処理
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyBeastMode()
    {
        Debug.Log("ビーストモード！！！！");
        _isCoroutineRunning = true;
        PlayerHP.BeastModeHP = 1.5f;
        StanMaxTime = 20f;
        isBMJudge = true;
        float BeastModeTime = 0f;
        EnemyCancel();
        _enemyAnim.SetBool("BeastMode", true);
        _enemyAnim.CrossFade("BeastMode", 0f);
        yield return null;
        while (true)
        {
            Debug.Log("ビーストモードtyuudaze！！！！");
            BeastModeTime += Time.deltaTime;
            if (BeastModeTime >= 20)
            {
                break;
            }
            yield return null;
        }
        PlayerHP.BeastModeHP = 1f;
        StanMaxTime = 10f;
        isBMJudge = false;
        yield return null;
    }
    IEnumerator EnemyDown()
    {
        Debug.Log("ダウン！！");
        if (_directionX.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
            Debug.Log("プレイヤーは右側にいます");
        }
        else if (_directionX.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
            Debug.Log("プレイヤーは左側にいます");
        }
        _isCoroutineRunning = true;
        float downTime = 0f;
        EnemyCancel();
        _enemyAnim.SetBool("Down", true);
        _enemyAnim.CrossFade("Down", 0.1f, 0, 0.35f);
        yield return null;
        _enemyAnim.SetBool("Down", false);
        while (true)
        {
            downTime += Time.deltaTime;
            if (downTime >= 6f)
            {
                break;
            }
            yield return null;
        }
        JM.ChangeClearScene();
    }
    void EnemyBeastModeEnd()
    {
        _enemyAnim.SetBool("BeastMode", false);
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false;
    }


    IEnumerator EnemyCounter()
    {
        _isCoroutineRunning = true;
        EnemyCancel();
        _enemyAnim.SetBool("Counter", true);
        _enemyAnim.CrossFade("Counter", 0f);
        HitStopScript.Instance.StartHitStop(0.5f);
        while(true)
        {
            if(HitStopScript.Instance.isEnemyCounter)
            {
                HitStopScript.Instance.isEnemyCounter = false;
                break;
            }
            yield return null;
        }
        _isCoroutineRunning = false;
        _enemyAnim.SetBool("Counter", false);
        _currentState = EnemyState.RightPunch;
        yield return null;
    }

    #endregion

    #region 当たり判定とHP
    /// <summary>
    /// 攻撃を受けたか+壁に当たったかの判定を返す
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerJab"))
        {
            if (!isGameOverFlag)
            {
                damege = 10;
                int RndCounter = Random.Range(1, 101);
                Debug.Log("プレイヤーの攻撃にあたった");
                //_currentState = EnemyState.HitStan;
                if(EnemyDriveGauge.Instance.isEnemyDriveGaugeMax && RndCounter >= 70)
                {
                    // 敵のカウンター攻撃！！！！！
                    Debug.Log("敵のカウンター攻撃！！！！！！！");
                    isCoroutineStop = true;
                    _currentState = EnemyState.Counter;
                }
                else if (!isEnemyStanFlag && !isBMJudge)
                {
                    EnemyHitStanState();
                }
                else
                {
                    // スタン中は攻撃力アップ
                    damege += 5;
                }
                DriveGauge.Instance.DriveGaugeUP();
                HitStopScript.Instance.StartHitStop(0.2f, "Enemy");
                ReduceEnemyHP(damege);
                //_currentState = EnemyState.Guard; // 状態をガードに変更
            }
        }
    }
    /// <summary>
    /// 敵のHPを減らしたりする
    /// </summary>
    public void ReduceEnemyHP(int _lostHP)
    {
        enemyHP -= _lostHP;
        EnemyHP.Instance.TakeDamage(_lostHP);
        PE.PunchEffect();
        if(enemyHP <= 0)
        {
            // ゲームクリアに移行
            _currentState = EnemyState.Down;
            isCoroutineStop = true;
            isGameOverFlag = true;
            //JM.ChangeClearScene();
        }
        else if (enemyHP <= enemyInitialHP * 0.33 && isBeastMode)
        {
            isCoroutineStop = true;
            isBeastMode = false;
            _currentState = EnemyState.BeastMode;
        }
    }
    #endregion

    #region ガード関連
    /// <summary>
    /// ガードするかどうかの関数
    /// </summary>
    void EnemyGaurd()
    {
        int _gaurdRnd = Random.Range(1, 101);
        if (_distancePtoE > shortDistance && _distancePtoE < middleDistance)
        {
            _enemyGaurd = 50;
        }
        else if (_distancePtoE >= middleDistance && _distancePtoE < longDistance)
        {
            _enemyGaurd = 40;
        }
        else if (_distancePtoE >= longDistance)
        {
            _enemyGaurd = 30;
        }
        else
        {
            _enemyGaurd = 20;
        }
        if (_enemyGaurd >= _gaurdRnd)
        {
            // ガードをするアニメーションを入れる
            Debug.Log("ガードに成功した");
        }
        else
        {
            // HPを減らす関数を呼び出す
            ReduceEnemyHP(10);
            // 被弾アニメーションを再生する
        }
    }
    #endregion

    #region 移動関連
    /// <summary>
    /// プレイヤーを追従する関数
    /// </summary>
    IEnumerator PlayerFollow(float moveDistance)
    {
        _enemyAnim.SetBool("Walk", true);
        _enemyAnim.CrossFade("Walk", 0.1f);
        _isCoroutineRunning = true;
        while (true)
        {
            _moveTimer += Time.deltaTime;
            transform.position = Vector2.MoveTowards(
                            transform.position,
                            new Vector2(_playerTr.position.x, transform.position.y), // X軸だけプレイヤーに追従
                            speed * Time.deltaTime);
            if (_distancePtoE < moveDistance || _moveTimer >= _moveMaxTimer)
            {
                break;
            }
            yield return null;
        }
        Debug.Log("追う終了！！");
        _moveTimer = 0f;
        _currentState = EnemyState.Idle; // 到達後Idleに戻る
        _enemyAnim.SetBool("Walk", false);
        _isCoroutineRunning = false;
    }
    /// <summary>
    /// プレイヤーから逃げる関数
    /// </summary>
    IEnumerator PlayerRetreat(float moveDistance)
    {
        _enemyAnim.SetBool("Walk", true);
        _enemyAnim.CrossFade("Walk", 0.1f);
        _isCoroutineRunning = true;
        while (true)
        {
            _moveTimer += Time.deltaTime;
            // プレイヤーから逃げる方向を計算
            Vector3 directionAwayFromPlayer = (transform.position - _playerTr.position).normalized;
            // 逆方向に移動
            Vector3 newPosition = transform.position + (directionAwayFromPlayer * backSpeed * Time.deltaTime);
            _rb.MovePosition(newPosition);
            if (_distancePtoE >= moveDistance || _moveTimer >= _moveMaxTimer)
            {
                break;
            }
            yield return null;
        }
        Debug.Log("逃げる終了！！");
        _moveTimer = 0f;
        _currentState = EnemyState.Idle; // 到達後Idleに戻る
        _enemyAnim.SetBool("Walk", false);
        _isCoroutineRunning = false;
        yield return null; // 目標距離に到達した場合は退却終了

    }
    #endregion

    #region 終了処理とデバッグキー

    void EnemyCancel()
    {
        // 技の処理をすべて消す
        EnemyAtackEnd();
        _isAtackEnd = false;
        EnemyRayCast.isTackleWall = false;
        _isTackle = false;
        EnemyHit2.DestroyCube();
    }
    void EnemyAtackEnd()
    {
        _enemyAnim.SetBool("Chain", false);
        _enemyAnim.SetBool("RightPunch", false);
        _enemyAnim.SetBool("LeftPunch", false);
        _enemyAnim.SetBool("ForwardStep", false);
        _enemyAnim.SetBool("BackUpper", false);
        _enemyAnim.SetBool("Tackle", false);
        _enemyAnim.SetBool("Smash", false);
        //_enemyAnim.SetBool("Smash", false);
        _isAtackEnd = true;
        Debug.Log("エネミーアタックエンド！！！");
    }
    /// <summary>
    /// デバッグ関連を詰め込んだ関数
    /// </summary>
    void DebugKey()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("isFollow" + isFollow);
            isFollow = true;
        }
        else if (Input.GetKey(KeyCode.X))
        {
            Debug.Log("isFollow" + isFollow);
            isFollow = false;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("isFollow" + isFollow);
            _currentState = EnemyState.Chain; // チェーン投げで攻撃
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("isFollow" + isFollow);
            isFollow = false;
        }
        if (isFollow)
        {
            PlayerRetreat(10f);
        }
        else
        {
            PlayerFollow(10f);
        }
    }

    public void EnemyEndAttack()
    {
        if (cubeController == null)
        {
            Debug.LogError("cubeController が設定されていません！");
            return;
        }
        cubeController.DestroyCube();
    }
    #endregion
}