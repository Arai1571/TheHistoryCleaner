using Unity.VisualScripting;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    PlayerController playerCnt;
    public GameObject extinguisherPrefab;//Instantiateする消化器
    public float shootSpeed;
    public float shootDelay;
    bool inShoot; //消化器攻撃中ならtrue
    GameObject extinguisherObj;

    void Start()
    {
        playerCnt = GetComponent<PlayerController>(); //コンポーネント取得
    }

    void Update()
    {
        if ((inShoot == true) || GameManager.Extinguisher <= 0) return;
        //左シフトキーを押したら消化器を出現させ、噴射
        if (Input.GetButtonDown("Fire3")) Shoot();
    }

    public void Shoot()
    {
        if ((inShoot == true) || GameManager.Extinguisher <= 0) return;
        GameManager.Extinguisher--;
        inShoot = true; //消化器噴射フラグを立てる
        //プレイヤーの角度を入手
        float angleZ = playerCnt.angleZ;
        //Rotationが扱っているQuaternion型として準備
        Quaternion q = Quaternion.Euler(0, 0, angleZ);
        //消化器を生成
        extinguisherObj = Instantiate(extinguisherPrefab, transform.position, q);
        //生成した消化器オブジェクトのRigidBody2Dの情報を取得
        Rigidbody2D rbody = extinguisherObj.GetComponent<Rigidbody2D>();
        //噴射するベクトルを作る
        float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);  //角度に対する底辺 X軸の方向
        float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);  //角度に対する底辺 Y軸の方向
        Vector2 v = (new Vector2(x, y)).normalized * shootSpeed;
        //煙に力を加える
        rbody.AddForce(v, ForceMode2D.Impulse);

        //時間差で攻撃中フラグを解除
        Invoke("StopShoot", shootDelay);
    }

    void StopShoot()
    {
        inShoot = false; //攻撃中フラグをOFFにする
    }
}
