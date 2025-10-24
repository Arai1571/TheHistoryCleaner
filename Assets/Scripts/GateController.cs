using UnityEngine;

public class GateController : MonoBehaviour
{
    [Header("開閉状態")]
    public bool open = false;

    [Header("参照")]
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Sensorから呼ばれる
    public void SetGateOpen(bool value)
    {
        if (open == value) return; // 同じ状態なら何もしない

        open = value;
        Debug.Log(open ? "ゲート開いた！" : "ゲート閉じた！");

        if (anim) anim.SetBool("open", open);
    }
}