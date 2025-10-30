using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class PlayerController : MonoBehaviour
{
    [Header("基本設定")]
    public float playerSpeed = 3.0f;
    public float axisH;  //縦方向の入力状況
    public float axisV;  //横方向の入力状況
    public float angleZ = -90f; //プレイヤーの角度計算用
    const int WALK_HP_DRAIN = 1;   // 歩行で消費する電力量
    const float WALK_INTERVAL = 1f;  // １秒ごと
    float walkTimer = 0f;


    [Header("参照")]
    public GameObject spotLight; //子オブジェクトのSpotLight
    private Rigidbody2D rbody;
    Animator anime;

    [Header("基本装備")]
    bool inAttack; //モップアタック
    bool inBleach; //ブリーチスプレー

    [Header("展示品判定用")]
    bool isPottery; //陶器に触れている
    bool isPainting; //絵に触れている
    GameObject touchObject; //触れている相手

    [Header("足音判定用")]
    float footstepInterval = 0.3f; //足音間隔
    float footstepTimer; //時間計測

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        //スポットライトを所持していればスポットライト表示
        if (GameManager.hasSpotLight) spotLight.SetActive(true);//表示
    }

    //スポットライトの入手フラグが立っていたらライトをつける
    public void SpotLightCheck()
    {
        if (GameManager.hasSpotLight) spotLight.SetActive(true);
    }

    void Update()
    {
        //プレイ中でない、またはエンディング中でないならば何もしない
        if (!(GameManager.gameState == GameState.playing || GameManager.gameState == GameState.ending)) return;

        if (Input.GetButtonDown("Jump"))
        {
            // すでに何かの攻撃中なら何もしない
            if (inAttack || inBleach) return;
            // 触れている対象がないなら何もしない
            if (!touchObject) return;

            if (!inAttack && isPainting)
            {
                int dir = RefreshDirection();     // 0:Front 1:Back 2/3:Side
                if (dir == 0) //正面を向いているなら
                {
                    Debug.Log("Front では Bleach 不可");
                    return; // コルーチンを抜ける
                }
                else
                {
                    StartCoroutine(BleachSpray(touchObject));
                    Debug.Log("絵をブリーチ！");
                }
            }
            else if (!inAttack && isPottery)
            {
                StartCoroutine(MopAttack(touchObject));
                Debug.Log("陶器こわす！");
            }
        }

        Move(); //上下左右の入力値の取得
        angleZ = GetAngle();//その時の角度を変数angleZに反映
        Animation();  //angleZを利用してアニメーション
        HandleFootsteps();    // 足音処理

        // 歩行中なら1秒ごとにHP-1
        bool isWalking = anime.GetBool("walk");
        if (isWalking)
        {
            walkTimer += Time.deltaTime;
            if (walkTimer >= WALK_INTERVAL)
            {
                walkTimer -= WALK_INTERVAL;
                GameManager.playerHP -= WALK_HP_DRAIN;
                if (GameManager.playerHP <= 0) GameOver();
            }
        }
        else
        {
            // 止まっている間はタイマーをリセット（好みで0固定）
            walkTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        //プレイ中でない、またはエンディング中でないならば何もしない
        if (!(GameManager.gameState == GameState.playing || GameManager.gameState == GameState.ending)) return;
        // 入力状況に応じてPlayerを動かす
        rbody.linearVelocity = (new Vector2(axisH, axisV)).normalized * playerSpeed;
    }

    //上下左右の入力値の取得
    public void Move()
    {
        //axisHとaxisVに入力状況を代入する
        axisH = Input.GetAxisRaw("Horizontal"); // 左右キー
        axisV = Input.GetAxisRaw("Vertical");   // 上下キー
    }

    //その時のプレイヤーの角度を取得するメソッド
    public float GetAngle()
    {
        //現在の座標を取得
        Vector2 fromPos = transform.position;

        //その瞬間のキー入力値(axisH,axisV)に応じた予測座標の取得
        Vector2 toPos = new Vector2(fromPos.x + axisH, fromPos.y + axisV);

        float angle;  //returnされる値の用意

        //もし水平か垂直のキー入力があればあらためて角度を算出
        if (axisH != 0 || axisV != 0)
        {
            float dirX = toPos.x - fromPos.x;
            float dirY = toPos.y - fromPos.y;

            //Atan2でラジアン角を取得->度に変換する
            angle = Mathf.Atan2(dirY, dirX) * Mathf.Rad2Deg;
        }
        else //何の入力もなければ１フレーム前の角度を維持
        {
            angle = angleZ;
        }
        return angle; //算出した角度をreturnする
    }

    void Animation()
    {
        // モップアタック・ブリーチスプレー中は方向＆walkをいじらない
        if (anime.GetBool("attack") || anime.GetBool("bleach")) return;

        //何らかの入力がある場合
        if (axisH != 0 || axisV != 0)
        {
            //ひとまずwalkアニメを走らせる
            anime.SetBool("walk", true);

            //angleZを利用して方角を決める.  パラメータnewDirection int型
            //int型のnewDirection 下：０、上：１、右：２、左：それ以外(3)

            if (angleZ > -135f && angleZ < -45f)  //下方向
            {
                anime.SetInteger("newDirection", 0);
            }
            else if (angleZ >= -45f && angleZ <= 45f) //右方向
            {
                anime.SetInteger("newDirection", 2);
                transform.localScale = new Vector2(1, 1);
            }
            else if (angleZ > 45f && angleZ < 135f) //上方向
            {
                anime.SetInteger("newDirection", 1);
            }
            else //左方向
            {
                anime.SetInteger("newDirection", 3);
                transform.localScale = new Vector2(-1, 1);
            }
        }

        else//入力がない場合
        {
            anime.SetBool("walk", false);  //走るフラグをOff
        }
    }

    int RefreshDirection()
    {
        int dirId;
        if (angleZ > -135f && angleZ < -45f) dirId = 0;        // 下
        else if (angleZ >= -45f && angleZ <= 45f) { dirId = 2; transform.localScale = new Vector2(1, 1); } // 右
        else if (angleZ > 45f && angleZ < 135f) dirId = 1;     // 上
        else { dirId = 3; transform.localScale = new Vector2(-1, 1); } // 左
        anime.SetInteger("newDirection", dirId);
        return dirId;
    }

    //攻撃のアニメーション
    string atkFront = "PlayerAttack_front";
    string atkSide = "PlayerAttack_side";
    string atkBack = "PlayerAttack_back";
    string bleSide = "PlayerBleach_side";
    string bleBack = "PlayerBleach_back";

    IEnumerator MopAttack(GameObject targetGO)
    {
        if (inAttack || !targetGO) yield break;

        inAttack = true; //攻撃中
        anime.SetBool("walk", false);   // 歩行OFF
        int dir = RefreshDirection();   // 向き更新
        anime.SetBool("attack", true);  // 攻撃ON

        // 保険：必ず再生されるかを切り分ける
        string s = (dir == 1) ? atkBack : (dir == 0) ? atkFront : atkSide;
        anime.CrossFade(s, 0.05f, 0, 0f);

        GameManager.playerHP -= 5;//HPを５減らす
        yield return new WaitForSeconds(0.5f); //時間差で発動

        //時間差で接触した相手の相手の姿や状態を替える
        if (targetGO && targetGO.TryGetComponent(out ExhibitBreakController ex)) ex.HitOnce();

        inAttack = false; //モップアタック中フラグをOFFにする
        anime.SetBool("attack", false);
    }

    IEnumerator BleachSpray(GameObject targetGO)
    {
        if (inBleach || !targetGO) yield break;

        inBleach = true; //攻撃中
        anime.SetBool("walk", false);   // 歩行OFF
        int dir = RefreshDirection();   // 向き更新

        if (dir == 0) { inBleach = false; yield break; } // 正面は禁止（保険）
        anime.SetBool("bleach", true);  // 攻撃ON

        SoundManager.instance.SEPlay(SEType.Spray); //スプレー音を鳴らす

        // Back,Sideのアニメ再生
        string s = (dir == 1) ? bleBack : bleSide;
        anime.CrossFade(s, 0.05f, 0, 0f);

        GameManager.playerHP -= 5;//HPを５減らす
        yield return new WaitForSeconds(0.5f); //時間差で発動

        //時間差で接触した相手の相手の姿や状態を替える
        if (targetGO && targetGO.TryGetComponent(out ExhibitBreakController ex)) ex.HitOnce();

        inBleach = false; //モップアタック中フラグをOFFにする
        anime.SetBool("bleach", false);
    }

    void GameOver() //HP0によるゲームオーバー
    {
        if (GameManager.playerHP <= 0)
        {   
            GameManager.gameState = GameState.gameover;//ゲームStateを変える

            SoundManager.instance.SEPlay(SEType.HP0); //HP0になった音を鳴らす

            //ゲームオーバー演出.Playerが持っている当たり判定のコンポーネント[Collider2D]の無効化
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rbody.linearVelocity = Vector2.zero;   // 動きを止める
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//ガードマンに捕まった場合のゲームオーバー
    {
        // すでにゲームオーバーなら何もしない
        if (GameManager.gameState == GameState.gameover) return;

        // Guardman に触れたらゲームオーバー
        if (collision.gameObject.CompareTag("Guardman"))
        {
            Debug.Log("Guardman に触れた！ゲームオーバー");
            GameManager.gameState = GameState.gameover;

            //死亡演出
            anime.SetTrigger("dead");        // 死亡アニメ再生
            Invoke(nameof(PlayDeadSE), 0.5f); // 捕まってワンテンポ置いてから音を鳴らす
            rbody.linearVelocity = Vector2.zero;   // 動きを止める
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false; // 当たり判定無効

            // 触れたガードマンだけ止める
            if (collision.TryGetComponent(out GuardmanController g)) g.FreezeToIdleFront();
        }

        //相手が陶器タグ
        if (collision.gameObject.CompareTag("Pottery"))
        {
            Debug.Log("Pottery に触れた！");
            isPottery = true; //陶器接触フラグON
            touchObject = collision.gameObject; //触れている相手のゲームオブジェクト情報を一時記憶

            // ExhibitHPBar を ON にする
            var hpBar = collision.GetComponentInChildren<ExhibitHPBar>(true);
            if (hpBar)
            {
                hpBar.gameObject.SetActive(true);
                hpBar.RefreshNow(); // 現在HPを反映して初期化
            }
        }

        //相手が絵タグ
        if (collision.gameObject.CompareTag("Painting"))
        {
            Debug.Log("Paintingに触れた！");
            isPainting = true; //絵接触フラグON
            touchObject = collision.gameObject; //触れている相手のゲームオブジェクト情報を一時記憶

            // ExhibitHPBar を ON にする
            var hpBar = collision.GetComponentInChildren<ExhibitHPBar>(true);
            if (hpBar)
            {
                hpBar.gameObject.SetActive(true);
                hpBar.RefreshNow(); // 現在HPを反映して初期化
            }
        }
    }

    //死亡したときに鳴らすSE
    void PlayDeadSE()
    {
        SoundManager.instance.SEPlay(SEType.Dead);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //相手が陶器タグ
        if (collision.gameObject.CompareTag("Pottery"))
        {
            isPottery = false; //陶器接触フラグOFF
            touchObject = null; //触れている相手のゲームオブジェクト情報をなしに

            var hpBar = collision.GetComponentInChildren<ExhibitHPBar>(true);
            if (hpBar)
            {
                hpBar.gameObject.SetActive(false);
            }
        }

        //相手が絵タグ
        if (collision.gameObject.CompareTag("Painting"))
        {
            isPainting = false; //絵接触フラグOFF
            touchObject = null; //触れている相手のゲームオブジェクト情報をなしに

            var hpBar = collision.GetComponentInChildren<ExhibitHPBar>(true);
            if (hpBar)
            {
                hpBar.gameObject.SetActive(false);
            }
        }
    }

    //足音
    void HandleFootsteps()
    {
        //プレイヤーが動いていれば
        if (axisH != 0 || axisV != 0)
        {
            footstepTimer += Time.deltaTime; //時間計測

            if (footstepTimer >= footstepInterval) //インターバルチェック
            {
                SoundManager.instance.SEPlay(SEType.Walk);
                footstepTimer = 0;
            }
        }
        else //動いていなければ時間計測リセット
        {
            footstepTimer = 0f;
        }
    }
}
