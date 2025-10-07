using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed = 3.0f;
    public float axisH;
    public float axisV;
    public float angleZ; //向いている角度
    private Rigidbody2D rbody;
    public GameObject spotLight; //ライトの子オブジェクト

    Animator anime;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");
        var dir = new Vector3(axisH, axisV, 0f).normalized;
        transform.position += dir * PlayerSpeed * Time.deltaTime;  // 物理抜きの直書き
    }

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

    //スポットライトの入手フラグが立っていたらライトをつける
    public void SpotLightCheck()
    {
        if (GameManager.hasSpotLight) spotLight.SetActive(true);
    }
    
}
