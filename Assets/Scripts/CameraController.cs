using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 diff;//ターゲットとの距離の差
    public float followSpeed = 5.0f; //カメラの補間スピード

    void Start()
    {
        //プレイヤー情報を取得
        player = GameObject.FindGameObjectWithTag("Player");

        //スタートした瞬間のカメラの現在地
        transform.position = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            -10
        );

        //プレイヤーとカメラの距離感を記憶しておく
        diff = player.transform.position - transform.position;
    }

    void LateUpdate()
    {
        //プレイヤーが見つからない場合は何もしない
        if (player == null) return;
            //線形補間を使って、カメラを目的の場所に動かす
            //Lerpメソッド(今の位置、ゴールとすべき位置、割合）
            transform.position = Vector3.Lerp(transform.position, player.transform.position - diff, followSpeed * Time.deltaTime);
    }
}
