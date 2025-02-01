using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class EnemyMove : MonoBehaviour
{
    // 敵のステータス関連
    public float enemyHP; // 敵のHP
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
    float _acceleration = 3f; // タックル攻撃の加速度
    float _tackleAcceleration = 0f;
    // プレイヤーとの距離関連
    public float shortDistance = 2f; // 近距離を測る変数
    public float middleDistance = 6f; // 中距離を測る変数
    public float longDistance = 12f; // 遠距離を測る変数
    private float _targetDistance; // 現在の目標距離
    float _previousValue = 0f; // 前フレームの値を記録する変数
    // 現在の状態を保持する変数
    private EnemyState _currentState;
    private float stateCooldown = 0f; // 状態遷移のクールダウンタイマー
    private float stateCooldownDuration = 1f; // クールダウン時間（1秒）
    private bool _isCoroutineRunning = false; // コルーチン実行中かどうか

    Animator _enemyAnim; // Animatorコンポーネント
    bool _isAnimActive = true; // フラグの初期状態


    Vector2 _directionX;

    bool _isWallFlag = false;


    bool _isAtackEnd = false;
    private EnemyHit cubeController; // EnemyAttack スクリプトの参照

    private float _moveTimer = 0f; // 経過時間
    private float _moveMaxTimer = 3f; // 退却の最大時間

    public chain Chain;
    public EnemyHit2 EnemyHit2;


    AudioSource WalkAudioSource;
    public AudioClip WalkSound;
    AudioSource RunAudioSource;
    public AudioClip RunSound;
    AudioSource PunchAudioSource;
    public AudioClip PunchSound;
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
        Walk              // 歩いている
    }
    #region スタートたち
    void Start()
    {
        transform.position = new Vector3(4, 0, 0);
        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        // リジットボディの設定
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
        // エネミーとプレイヤーの距離計測
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
        _enemyAnim = GetComponent<Animator>(); // Animatorを取得
        cubeController = GetComponent<EnemyHit>();
        WalkAudioSource = gameObject.AddComponent<AudioSource>();
        RunAudioSource = gameObject.AddComponent<AudioSource>();
        PunchAudioSource = gameObject.AddComponent<AudioSource>();
    }
    #endregion
    #region アップデートたち
    void Update()
    {
        // 互いの距離計測 + 敵の向き交換
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
        Debug.Log("距離計測中");
        _directionX = _playerTr.position - transform.position;

        //DebugKey();
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
        }
    }
    #endregion
    #region 状態遷移を管理するプログラムたち
    void HandleIdle()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyIdle());
    }
    void HandleMiddleFollow()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(PlayerFollow(_targetDistance));
    }
    void HandleShortFollow()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(PlayerFollow(_targetDistance));
    }
    void HandleMiddleRetreat()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(PlayerRetreat(_targetDistance));
    }
    void HandleLongRetreat()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(PlayerRetreat(_targetDistance));
    }
    void HandleGuard()
    {
        EnemyGaurd();
        _currentState = EnemyState.Idle;
    }
    void HandleRightPunch()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyRightPunch());
        //_currentState = EnemyState.Idle;
    }
    void HandleLeftPunch()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyLeftPunch());
        //_currentState = EnemyState.Idle;
    }
    void HandleSmash()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemySmash());
        //_currentState = EnemyState.Idle;
    }
    void HandleTackle()
    {
        Debug.Log("タックル呼び出し処理開始！！！");
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyTackle());
        //_currentState = EnemyState.Idle;
    }
    void HandleChain()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyChain());
        //_currentState = EnemyState.Idle;
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
            if (randomState <= 75)
            {
                _currentState = EnemyState.RightPunch; // 近距離で攻撃
            }
            else if (randomState <= 90)
            {
                _currentState = EnemyState.Smash; // 近距離で攻撃
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.MiddleRetreat; // 中距離まで退避
                _targetDistance = middleDistance + 3f;
            }
        }
        else if (_distancePtoE >= middleDistance && _distancePtoE < longDistance)
        {
            if (randomState <= 60)
            {
                _currentState = EnemyState.Tackle; // タックルで攻撃
            }
            else if (randomState <= 80)
            {
                _currentState = EnemyState.ShortFollow; // 小距離まで追跡
                _targetDistance = shortDistance + 2f;
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.LongRetreat; // 遠距離まで退避
                _targetDistance = longDistance + 3f;
            }
        }
        else if (_distancePtoE >= longDistance)
        {
            if (randomState <= 50)
            {
                _currentState = EnemyState.Tackle; // チェーン投げで攻撃
            }
            else if (randomState <= 80)
            {
                _currentState = EnemyState.Tackle; // タックルで攻撃
            }
            else if (randomState <= 100)
            {
                _currentState = EnemyState.MiddleFollow; // 中距離まで追跡
                _targetDistance = middleDistance + 2f;
            }
        }
        else
        {
            //_currentState = EnemyState.Idle; // 距離が遠すぎる場合は待機
        }
        // ガードの条件（例: プレイヤー攻撃を受けた場合）
        //if (/* && 被弾フラグ*/) // 被弾フラグは別途用意
        //{
        //    _currentState = EnemyState.Guard;
        //}
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
        _isAnimActive = true;
        _enemyAnim.SetBool("RightPunch", true);
        _enemyAnim.CrossFade("RightPunch", 0.1f);
        PunchAudioSource.PlayOneShot(PunchSound);
        //cubeController.Punch();
        while (true)
        {
            // 現在のアニメーションステート情報を取得
            AnimatorStateInfo currentState = _enemyAnim.GetCurrentAnimatorStateInfo(0);
            // 敵を前進させる
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            if (_isAtackEnd &&/*currentState.normalizedTime >= 1f &&*/ _isAnimActive)
            if (currentState.normalizedTime >= 1f && _isAnimActive )
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
        _isAnimActive = true;
        _enemyAnim.SetBool("LeftPunch", true);
        _enemyAnim.CrossFade("LeftPunch", 0.1f);
        PunchAudioSource.PlayOneShot(PunchSound);
        //cubeController.Punch();
        while (true)
        {

            // 現在のアニメーションステート情報を取得
            AnimatorStateInfo currentState = _enemyAnim.GetCurrentAnimatorStateInfo(0);
            // 敵を前進させる
            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            if (_isAtackEnd &&/*currentState.normalizedTime >= 1f &&*/ _isAnimActive)
                if (currentState.normalizedTime >= 1f && _isAnimActive)
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
            _currentState = EnemyState.Idle;
        }
        _isCoroutineRunning = false;
    }
    /// <summary>
    /// ガードブレイク攻撃
    /// </summary>
    IEnumerator EnemySmash()
    {
        yield return null;
        Debug.Log("ガードブレイク！");
        _isCoroutineRunning = true;
        _isAnimActive = true;
        _enemyAnim.SetBool("Smash", true);
        _enemyAnim.CrossFade("Smash", 0.1f);
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
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false;
    }
    /// <summary>
    /// タックル攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyTackle()
    {
        Debug.Log("タックル！");
        _isCoroutineRunning = true;
        _enemyAnim.SetBool("Tackle", true);
        _enemyAnim.CrossFade("Tackle", 0.1f);
        tackleSpeed = 5f;
        RunAudioSource.clip = RunSound;
        RunAudioSource.Play();
        // 現在のアニメーションが終了したかを確認
        while (true)
        {
            // 敵を前進させる
            transform.position += transform.forward * tackleSpeed * Time.deltaTime;
            _tackleAcceleration += Time.deltaTime;
            if(_tackleAcceleration >= 0.3f)
            {
                tackleSpeed += _acceleration;
                _tackleAcceleration = 0;
            }
            float _distancePtoE2 = Vector2.Distance(transform.position, _playerTr.position);
            if (_distancePtoE2 <= shortDistance + 1f || _isWallFlag)
            {
                tackleSpeed = 0f;
                break;
            }
            // フレーム間の待機
            yield return null;
        }
        EnemyHit2.DestroyCube();
        RunAudioSource.Stop();
        _isWallFlag = false;
        _enemyAnim.SetBool("Tackle", false);
        Debug.Log("タックル終了");
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false;
    }

    /// <summary>
    /// チェーン攻撃
    /// </summary>
    /// <returns></returns>
    IEnumerator EnemyChain()
    {
        Debug.Log("チェーン！");
        _isCoroutineRunning = true;
        _isAnimActive = true;
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
        _isAnimActive = false; // フラグをオフにする
        _isCoroutineRunning = false;
        _enemyAnim.SetBool("Chain", false);
        Debug.Log("チェーン終了");
        _currentState = EnemyState.Idle;

    }


    IEnumerator EnemyIdle()
    {
        _isCoroutineRunning = true;
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
        float _IdleRnd = Random.Range(1, 3);
        yield return new WaitForSeconds(_IdleRnd);
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false;
        // 距離に基づく状態遷移
        UpdateState();
        Debug.Log("アイドル状態になりました");
    }
    #endregion

    /// <summary>
    /// 攻撃を受けたか+壁に当たったかの判定を返す
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerJab"))
        {
            Debug.Log("プレイヤーの攻撃にあたった");
            ReduceEnemyHP(10);
            _currentState = EnemyState.Guard; // 状態をガードに変更
        }
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("壁に当たった");
            _isWallFlag = true;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.CompareTag("Wall"))
        {
            Debug.Log("壁に当たった");
            _isWallFlag = true;
        }
    }
    /// <summary>
    /// 敵のHPを減らしたりする
    /// </summary>
    void ReduceEnemyHP(int _lostHP)
    {
        enemyHP -= _lostHP;
        Debug.Log("HPが減ったしまった！現在のHP：" + enemyHP);
    }
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
    /// <summary>
    /// プレイヤーを追従する関数
    /// </summary>
    IEnumerator PlayerFollow(float moveDistance)
    {
        //_moveTimer += Time.deltaTime;
        //// プレイヤーとの距離が指定の距離になったら実行しない
        //if (_distancePtoE < moveDistance || _moveTimer < _moveMaxTimer)
        //{
        //    Debug.Log("追う終了！！");
        //    _currentState = EnemyState.Idle; // 到達後Idleに戻る
        //    _enemyAnim.SetBool("Walk", false);
        //    _enemyAnim.CrossFade("Walk", 0.1f);
        //    yield return null;
        //}
        //_enemyAnim.SetBool("Walk", true);
        //Debug.Log("追う！！");
        //// プレイヤーに向けて進む
        //transform.position = Vector2.MoveTowards(
        //                    transform.position,
        //                    new Vector2(_playerTr.position.x, transform.position.y), // X軸だけプレイヤーに追従
        //                    speed * Time.deltaTime);
        _enemyAnim.SetBool("Walk", true);
        _enemyAnim.CrossFade("Walk", 0.1f);
        _isCoroutineRunning = true;
        WalkAudioSource.clip = WalkSound;
        WalkAudioSource.Play();
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
        WalkAudioSource.Stop();
        _currentState = EnemyState.Idle; // 到達後Idleに戻る
        _enemyAnim.SetBool("Walk", false);
        _isCoroutineRunning = false;
    }
    /// <summary>
    /// プレイヤーから逃げる関数
    /// </summary>
    IEnumerator PlayerRetreat(float moveDistance)
    {
        //if (_distancePtoE >= moveDistance)
        //{
        //    Debug.Log("逃げる終了！！");
        //    _currentState = EnemyState.Idle; // 到達後Idleに戻る
        //    _enemyAnim.SetBool("Walk", false);
        //    yield return null; // 目標距離に到達した場合は退却終了
        //}
        //// 指定の距離離れるまで実行
        //if (_distancePtoE < moveDistance || _moveTimer >= _moveMaxTimer)
        //{
        //    Debug.Log("逃げる！！");
        //    _enemyAnim.SetBool("Walk", true);
        //    _enemyAnim.CrossFade("Walk", 0.1f);
        //    // プレイヤーから逃げる方向を計算
        //    Vector3 directionAwayFromPlayer = (transform.position - _playerTr.position).normalized;
        //    // 逆方向に移動
        //    Vector3 newPosition = transform.position + (directionAwayFromPlayer * backSpeed * Time.deltaTime);
        //    _rb.MovePosition(newPosition);
        //}
        _enemyAnim.SetBool("Walk", true);
        _enemyAnim.CrossFade("Walk", 0.1f);
        WalkAudioSource.clip = WalkSound;
        WalkAudioSource.Play();
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
        WalkAudioSource.Stop();
        _currentState = EnemyState.Idle; // 到達後Idleに戻る
        _enemyAnim.SetBool("Walk", false);
        _isCoroutineRunning = false;
        yield return null; // 目標距離に到達した場合は退却終了

    }

    void EnemyAtackEnd()
    {
        _enemyAnim.SetBool("Chain", false);
        _enemyAnim.SetBool("RightPunch", false);
        _enemyAnim.SetBool("LeftPunch", false);
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
}