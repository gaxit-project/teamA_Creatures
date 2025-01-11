using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    // 敵のステータス関連
    public float enemyHP; // 敵のHP
    int _enemyGaurd;
    [SerializeField] float speed = 4; // 敵の動くスピード
    [SerializeField] float backSpeed = 3; // 敵の動くスピード

    public bool isFollow = false; // 追従するかどうかのフラグ

    Transform _playerTr; // プレイヤーのTransform
    Rigidbody _rb; // このオブジェクトの Rigidbody

    float _distanceAway = 5f; // プレイヤーから離れる距離
    float _distanceApp = 1.5f; // プレイヤーに近づく距離
    float _distancePtoE;  // エネミーとプレイヤーの距離を入れる変数
    float _directionX;    // エネミーとプレイヤーの向きを入れる変数
    float _previousDirectionX = 0f;

    // プレイヤーとの距離関連
    public float shortDistance; // 近距離を測る変数
    public float middleDistance; // 中距離を測る変数
    public float longDistance; // 遠距離を測る変数
    private float _targetDistance; // 現在の目標距離

    // 現在の状態を保持する変数
    private EnemyState _currentState;

    private float stateCooldown = 0f; // 状態遷移のクールダウンタイマー
    private float stateCooldownDuration = 1f; // クールダウン時間（1秒）

    private bool _isCoroutineRunning = false; // コルーチン実行中かどうか

    private Animator _enemyAnim;  //Animatorをanimという変数で定義する



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
        ShortAttack,      // 近距離攻撃
        MiddleAttack,     // 中距離攻撃
        LongAttack,       // 遠距離攻撃
        RightTurn,        // 回転させる
        LeftTurn          // 回転させる
    }
    #region スタートたち
    void Start()
    {
        transform.position = new Vector3(4, 0, 0);
        _playerTr = GameObject.FindGameObjectWithTag("Player").transform;
        // リジットボディの設定
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        // エネミーとプレイヤーの距離計測
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
    }
    #endregion

    #region アップデートたち

    void Update()
    {
        // 互いの距離計測 + 敵との向き
        _distancePtoE = Vector2.Distance(transform.position, _playerTr.position);
        _directionX = _playerTr.position.x - transform.position.x;
        if (_previousDirectionX >= 0 && _directionX < 0)
        {
            Debug.Log("プラスからマイナスに変化しました");
            _currentState = EnemyState.RightTurn;
        }
        else if (_previousDirectionX <= 0 && _directionX > 0)
        {
            Debug.Log("マイナスからプラスに変化しました");
            _currentState = EnemyState.LeftTurn;
        }
        _previousDirectionX = _directionX;

        //DebugKey();
        switch (_currentState)
        {
            case EnemyState.Idle:
                HandleIdle();
                // 距離に基づく状態遷移
                UpdateState();
                Debug.Log("アイドル状態になりました");
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
            case EnemyState.ShortAttack:
                HandleShortAttack();
                break;
            case EnemyState.MiddleAttack:
                HandleMiddleAttack();
                break;
            case EnemyState.LongAttack:
                HandleLongAttack();
                break;
            case EnemyState.LeftTurn:
                EnemyTurn();
                break;
            case EnemyState.RightTurn:
                EnemyTurn();
                break;
        }
    }

    #endregion






    #region 状態遷移を管理するプログラムたち
    void HandleIdle()
    {
        _currentState = EnemyState.Idle;
    }
    void HandleMiddleFollow()
    {
        PlayerFollow(_targetDistance);
        if (_distancePtoE <= _targetDistance)
        {
            _currentState = EnemyState.Idle; // 到達後Idleに戻る
        }
    }
    void HandleShortFollow()
    {
        PlayerFollow(_targetDistance);
        if (_distancePtoE <= _targetDistance)
        {
            _currentState = EnemyState.Idle; // 到達後Idleに戻る
        }
    }
    void HandleMiddleRetreat()
    {
        PlayerRetreat(_targetDistance);
        if (_distancePtoE <= _targetDistance)
        {
            _currentState = EnemyState.Idle; // 到達後Idleに戻る
        }
    }
    void HandleLongRetreat()
    {
        PlayerRetreat(_targetDistance);
        if (_distancePtoE <= _targetDistance)
        {
            _currentState = EnemyState.Idle; // 到達後Idleに戻る
        }
    }
    void HandleGuard()
    {
        EnemyGaurd();
        _currentState = EnemyState.Idle;
    }
    void HandleShortAttack()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyShortAttack());
        //_currentState = EnemyState.Idle;
    }
    void HandleMiddleAttack()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyMiddleAttack());
        //_currentState = EnemyState.Idle;
    }
    void HandleLongAttack()
    {
        if (_isCoroutineRunning) return; // 実行中なら新しいコルーチンは呼び出さない
        StartCoroutine(EnemyLongAttack());
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
            if (randomState <= 80)
            {
                Debug.Log("近距離攻撃");
                _currentState = EnemyState.ShortAttack; // 近距離で攻撃
            }
            else if (randomState <= 100)
            {
                Debug.Log("中距離まで退避");
                _currentState = EnemyState.MiddleRetreat; // 中距離まで退避
                _targetDistance = middleDistance + 3f;
            }
            else
            {

            }

        }
        else if (_distancePtoE >= middleDistance && _distancePtoE < longDistance)
        {
            if (randomState <= 60)
            {
                Debug.Log("中距離攻撃");
                _currentState = EnemyState.MiddleAttack; // 中距離で攻撃
            }
            else if (randomState <= 80)
            {
                Debug.Log("近距離まで追跡");
                _currentState = EnemyState.ShortFollow; // 小距離まで追跡
                _targetDistance = shortDistance + 2f;
            }
            else if (randomState <= 100)
            {
                Debug.Log("遠距離まで退避");
                _currentState = EnemyState.LongRetreat; // 遠距離まで退避
                _targetDistance = longDistance + 3f;
            }

        }
        else if (_distancePtoE >= longDistance)
        {
            if (randomState <= 50)
            {
                Debug.Log("遠距離攻撃");
                _currentState = EnemyState.LongAttack; // 遠距離で攻撃
            }
            else if (randomState <= 100)
            {
                Debug.Log("中距離まで追跡");
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


    void EnemyTurn()
    {
        // 向きの変更を適用
        if (Attack.Instance.left)
        {
            transform.rotation = Quaternion.Euler(0, -90, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        _currentState = EnemyState.Idle;
    }


    #region 攻撃の処理たち
    IEnumerator EnemyShortAttack()
    {
        _isCoroutineRunning = true; // 実行中フラグを立てる
        Debug.Log("近距離攻撃中");
        yield return new WaitForSeconds(5);
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false; // 実行中フラグを立てる
    }

    IEnumerator EnemyMiddleAttack()
    {
        _isCoroutineRunning = true; // 実行中フラグを立てる
        yield return new WaitForSeconds(3);
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false; // 実行中フラグを立てる
    }
    IEnumerator EnemyLongAttack()
    {
        _isCoroutineRunning = true; // 実行中フラグを立てる
        yield return new WaitForSeconds(3);
        _currentState = EnemyState.Idle;
        _isCoroutineRunning = false; // 実行中フラグを立てる
    }
    IEnumerator EnemyIdle()
    {
        yield return new WaitForSeconds(3);
    }
    #endregion

    /// <summary>
    /// 攻撃を受けたかの判定を返す
    /// </summary>
    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("PlayerHit"))
        {
            Debug.Log("プレイヤーの攻撃にあたった");
            //_currentState = EnemyState.Guard; // 状態をガードに変更
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
    public void PlayerFollow(float moveDistance)
    {
        // プレイヤーとの距離が指定の距離になったら実行しない
        if (_distancePtoE < moveDistance)
        {
            return;
        }
        // プレイヤーに向けて進む
        transform.position = Vector2.MoveTowards(
                            transform.position,
                            new Vector2(_playerTr.position.x, transform.position.y), // X軸だけプレイヤーに追従
                            speed * Time.deltaTime);
    }

    /// <summary>
    /// プレイヤーから逃げる関数
    /// </summary>
    public void PlayerRetreat(float moveDistance)
    {
        if (_distancePtoE >= moveDistance) return; // 目標距離に到達した場合は退却終了
        // 指定の距離離れるまで実行
        if (_distancePtoE < moveDistance)
        {
            // プレイヤーから逃げる方向を計算
            Vector3 directionAwayFromPlayer = (transform.position - _playerTr.position).normalized;
            // 逆方向に移動
            Vector3 newPosition = transform.position + (directionAwayFromPlayer * backSpeed * Time.deltaTime);
            _rb.MovePosition(newPosition);
        }
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
        if (isFollow)
        {
            PlayerRetreat(10f);
        }
        else
        {
            PlayerFollow(10f);
        }
    }
}