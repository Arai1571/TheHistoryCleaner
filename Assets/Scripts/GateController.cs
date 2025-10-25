using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("開閉状態")]
    public bool open = false;

    [Header("参照")]
    private Animator anim;
    public Collider2D physicalCollider;//実際に通行を制限するコライダー

    void Start()
    {
        anim = GetComponent<Animator>();
        if (!physicalCollider) physicalCollider = GetComponent<Collider2D>(); //自身のコライダーを参照
    }

    // Sensorから呼ばれる
    public void SetGateOpen(bool value)
    {
        if (open == value) return; // 同じ状態なら何もしない
        SoundManager.instance.SEPlay(SEType.GateOpen); //ゲートの開く音を鳴らす
        open = value;
        Debug.Log("ゲートが開いた！");

        if (anim) anim.SetBool("open", open);

        // コライダーON/OFF
        if (physicalCollider) physicalCollider.enabled = !open; // 開く時はOFF、閉じる時はON
    }
}