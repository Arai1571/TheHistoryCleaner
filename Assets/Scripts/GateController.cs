using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("開閉状態")]
    public bool open = false; // true = 開いてる, false = 閉まってる

    [Header("参照")]
    private Animator anim;
    public Collider2D physicalCollider; // 通行制限用コライダー (IsTrigger=false のもの)

    void Start()
    {
        anim = GetComponent<Animator>();

        // Inspectorで未指定なら、自分のコライダーから探す
        if (!physicalCollider)
        {
            Collider2D[] cols = GetComponents<Collider2D>();
            foreach (var c in cols)
            {
                if (!c.isTrigger)  // Triggerじゃない方を優先
                {
                    physicalCollider = c;
                    break;
                }
            }
        }

        // 初期状態を反映
        if (physicalCollider)
            physicalCollider.enabled = !open; // 開いてるときはOFF、閉じてるときはON
    }

    // Sensorから呼ばれる
    public void SetGateOpen(bool value)
    {
        if (open == value) return; // 同じ状態なら何もしない

        open = value;
        Debug.Log(open ? "ゲートが開いた！" : "ゲートが閉じた！");

        // Animator制御
        if (anim) anim.SetBool("open", open);

        // 物理コライダーON/OFF切り替え
        if (physicalCollider)
            physicalCollider.enabled = !open; // 開いたらOFF（通れる）、閉じたらON（ブロック）
    }
}
