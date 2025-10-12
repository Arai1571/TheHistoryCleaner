using UnityEngine;

public class SpotLight : MonoBehaviour
{
    public PlayerController playerCnt; //PlayerControllerコンポーネント
    public float rotationSpeed=20.0f; //スポットライトの固定速度

    void LateUpdate()
    {
        //寸前までのスポットライトの回転値（Z軸のみ取得）
        //float currentAngle = transform.eulerAngles.z;

        //プレイヤーの角度
        float targetAngle = playerCnt.angleZ;

        //ターゲットとなる角度を調節
        Quaternion targetRotation = Quaternion.Euler(0, 0, (targetAngle - 90));
        
        //現在の回転を（寸前の回転）->ターゲットの回転になるように滑らかに補間するQuaternion.Slerpメソッド
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
    }
}
