using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GuardmanSpotLight : MonoBehaviour
{
    public GuardmanController guardmanCnt;   // GuardmanのEnemyController参照
    public float rotationSpeed = 20.0f; // スポットライトの回転速度
    public float patrolRotationSpeed = 5.0f; // パトロール時の回転速度

    [Header("光が壁を貫通しないようにする用")]
    public Light2D spotLight; //スポットライト
    public LayerMask wallLayer; //壁レイヤーを指定
    public float maxDistance = 5f;      // 最大の照射距離

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

        // Raycastで壁までの距離を取得
        Vector2 dir = (Quaternion.Euler(0, 0, targetAngle) * Vector2.right).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, maxDistance, wallLayer);

        // 壁がある場合はそこまでの距離、なければ最大距離
        float distance = hit.collider ? hit.distance : maxDistance;
        spotLight.pointLightOuterRadius = distance;

    }
}
