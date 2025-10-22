using UnityEngine;

public class ExhibitCheck : MonoBehaviour
{
    public PlayerController playerCnt; //PlayerControllerコンポーネント
    public float rotationSpeed=20.0f; //固定速度

    void LateUpdate()
    {
        //プレイヤーの角度
        float targetAngle = playerCnt.angleZ;

        //ターゲットとなる角度を調節
        Quaternion targetRotation = Quaternion.Euler(0, 0, (targetAngle - 90));
        
        //現在の回転を（寸前の回転）->ターゲットの回転になるように滑らかに補間するQuaternion.Slerpメソッド
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation, rotationSpeed * Time.deltaTime);
    }
}