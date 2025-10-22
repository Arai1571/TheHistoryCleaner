using UnityEngine;
using static GameManager;

public class GuardmanController : MonoBehaviour
{
    public float speed = 0.5f;               // 追跡時スピード
    public float patrolSpeed = 0.2f;         // パトロール時スピード
    public float reactionDistance = 4.0f;    // プレイヤー検知距離
    public float changeDirectionInterval = 2.5f; // ランダム方向変更の間隔

    float axisH;
    float axisV;
    float directionChangeTimer; // パトロール方向切り替え用タイマー

    Rigidbody2D rbody;
    Animator animator;
    bool isActive = false;
    public bool onSmoke; //消化器の足止めエフェクト「Smoke」にあたっているかどうか

    GameObject player;
    public float angleZ; // スポットライト用（向き）

    GuardmanController guardmanCnt;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>(); // 子オブジェクトのAnimatorを取得
        player = GameObject.FindGameObjectWithTag("Player");

        // 最初のパトロール方向をランダムに設定
        SetRandomPatrolDirection();
    }

    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;
        if (onSmoke) return;
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.transform.position);

        // プレイヤーが近づいたら追跡モードに切り替え
        isActive = (dist < reactionDistance);

        // Animatorパラメータに反映
        animator.SetBool("isActive", isActive);

        if (isActive)
        {
            // プレイヤーを追いかける
            float dx = player.transform.position.x - transform.position.x;
            float dy = player.transform.position.y - transform.position.y;
            float rad = Mathf.Atan2(dy, dx);
            angleZ = rad * Mathf.Rad2Deg;

            int newDirection;
            if (angleZ > -45.0f && angleZ <= 45.0f)
                newDirection = 3;   //右
            else if (angleZ > 45.0f && angleZ <= 135.0f)
                newDirection = 2;   //上
            else if (angleZ >= -135.0f && angleZ <= -45.0f)
                newDirection = 0;   //下
            else
                newDirection = 1;   //左

            animator.SetInteger("newDirection", newDirection);
            axisH = Mathf.Cos(rad);
            axisV = Mathf.Sin(rad);
        }
        else
        {
            // パトロール動作
            Patrol();
        }
    }

    void Patrol()
    {
        // 一定時間ごとに方向を変える
        directionChangeTimer += Time.deltaTime;
        if (directionChangeTimer >= changeDirectionInterval)
        {
            SetRandomPatrolDirection();
            directionChangeTimer = 0f;
        }

        // Animator用に向きを反映
        float angle = Mathf.Atan2(axisV, axisH) * Mathf.Rad2Deg;
        int newDirection;
        if (angle > -45.0f && angle <= 45.0f)
            newDirection = 3;
        else if (angle > 45.0f && angle <= 135.0f)
            newDirection = 2;
        else if (angle >= -135.0f && angle <= -45.0f)
            newDirection = 0;
        else
            newDirection = 1;

        animator.SetInteger("newDirection", newDirection);

        angleZ = angle;
    }

    void SetRandomPatrolDirection()
    {
        // -1〜1のランダムな方向を取得
        axisH = Random.Range(-1f, 1f);
        axisV = Random.Range(-1f, 1f);

        // 正規化
        Vector2 dir = new Vector2(axisH, axisV).normalized;
        axisH = dir.x;
        axisV = dir.y;
    }

    void FixedUpdate()
    {
        if (GameManager.gameState != GameState.playing) return;
        if (onSmoke) { rbody.linearVelocity = Vector2.zero; return; }
        if (player == null) return;

        float moveSpeed = isActive ? speed : patrolSpeed;
        rbody.linearVelocity = new Vector2(axisH, axisV) * moveSpeed;
    }

    //消化器をぶつけられた時
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Smoke"))
        {
            onSmoke = true;
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Smoke"))
            onSmoke = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, reactionDistance);
    }

    // PlayerをつかまえたらアニメーションをIdolにして止まる
    public void FreezeToIdleFront()
    {
        // 以後は完全停止
        onSmoke = true;
        isActive = false;
        axisH = axisV = 0f;
        if (!rbody) rbody = GetComponent<Rigidbody2D>();
        if (rbody) rbody.linearVelocity = Vector2.zero;

        // 「正面」向きにしてアイドルへ（あなたのマッピングで 0=下=正面）
        if (!animator) animator = GetComponentInChildren<Animator>();
        animator.SetBool("isActive", false);
        animator.SetInteger("newDirection", 0); // 正面
        animator.CrossFade("GuardmanIdol_front", 0.05f, 0, 0f);
    }

}
