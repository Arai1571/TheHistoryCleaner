using UnityEngine;

public class GuardmanSpotLight : MonoBehaviour
{
    public GuardmanController guardmanCnt;   // GuardmanのEnemyController参照
    public float rotationSpeed = 20.0f; // スポットライトの回転速度
    public float patrolRotationSpeed = 5.0f; // パトロール時の回転速度

    void LateUpdate()
    {
        if (guardmanCnt == null) return; // ガードマンがいなければ何もしない

        // ガードマンの向き（角度）を取得
        float targetAngle = guardmanCnt.angleZ;

        // 状態に応じてライトの回転速度を変える
        float speed = guardmanCnt.isActiveAndEnabled ? rotationSpeed : patrolRotationSpeed;

        // 回転の補間
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            speed * Time.deltaTime
        );
    }
}
