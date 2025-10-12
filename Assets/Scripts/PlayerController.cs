using Unity.VisualScripting;
using UnityEngine;
using static GameManager;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 3.0f;
    public float axisH;  //縦方向の入力状況
    public float axisV;  //横方向の入力状況
    public float angleZ = -90f; //プレイヤーの角度計算用

    public GameObject spotLight; //子オブジェクトのSpotLight

    private Rigidbody2D rbody;
    Animator anime;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();

        //スポットライトを所持していればスポットライト表示
        if (GameManager.hasSpotLight)
        {
            spotLight.SetActive(true);//表示
        }

        rbody = GetComponent<Rigidbody2D>();
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

        Move(); //上下左右の入力値の取得
        angleZ = GetAngle();//その時の角度を変数angleZに反映
        Animation();  //angleZを利用してアニメーション
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

    void GameOver()
    {
        if (GameManager.playerHP <= 0)
        {
            //ゲームStateを変える
            GameManager.gameState = GameState.gameover;

            //ゲームオーバー演出.Playerが持っている当たり判定のコンポーネント[Collider2D]の無効化
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rbody.linearVelocity = Vector2.zero; //動きを止める
            rbody.gravityScale = 1.0f;  //重力の復活
            anime.SetTrigger("dead");   //死亡アニメクリップの発動
            Destroy(gameObject, 1.0f); //１秒後に存在を消去
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // すでにゲームオーバーなら何もしない
        if (GameManager.gameState == GameState.gameover) return;

        // Guardman に触れたらゲームオーバー
        if (collision.gameObject.CompareTag("Guardman"))
        {
            Debug.Log("Guardman に触れた！ゲームオーバー");
            GameManager.gameState = GameState.gameover;

            // アニメーションや物理停止など必要なら追加
            anime.SetTrigger("dead");              // 死亡アニメ再生（任意）
            rbody.linearVelocity = Vector2.zero;   // 動きを止める
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false; // 当たり判定無効
            Destroy(gameObject, 1.0f);             // 少し待って削除（任意）
        }
    }
}
