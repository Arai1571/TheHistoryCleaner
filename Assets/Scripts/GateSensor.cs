using System.Collections;
using UnityEngine;

public class GateSensor : MonoBehaviour
{
    private GateController gate; //親の参照
    public bool isExitSensor = false; // 出口側センサーならtrue
    public Vector2 allowedDirection = Vector2.left; // この方向から来た時だけ反応
    public float dotThreshold = 0.3f; // 角度の許容範囲（1=完全同方向、0=90°） 
    GameObject canvas;
    GameObject gatePanel;


    void Start()
    {
        gate = GetComponentInParent<GateController>();
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        gatePanel = canvas.transform.Find("GatePanel").gameObject;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gate || !collision.CompareTag("Player")) return;//ゲートがない、プレイヤーないない時は何もしない

        // プレイヤーの向き(進行方向)を取得
        Rigidbody2D playerRb = collision.attachedRigidbody;
        if (!playerRb) return;
        Vector2 moveDir = playerRb.linearVelocity.normalized;

        // ゲートの許可方向との角度を比較
        float dot = Vector2.Dot(moveDir, allowedDirection.normalized);

        if (dot > dotThreshold)
        {
            gate.SetGateOpen(true);
            Debug.Log("正面から入ったのでゲート開く");
        }
        else
        {
            Debug.Log("逆方向なので反応しない");
            SoundManager.instance.SEPlay(SEType.GateClosed); //ゲートが閉鎖している音を鳴らす
            gatePanel.SetActive(true);//「開かない」と表示させる
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!gate || !collision.CompareTag("Player")) return;

        gate.SetGateOpen(false);
        gatePanel.SetActive(false);

        // ✅ 出口ゲートを通過したらゲームクリア
        if (isExitSensor && gate.isExitGate)
        {
            Debug.Log("出口を通過 → ゲームクリア！");
            StartCoroutine(GameClearSequence());
        }
    }

    IEnumerator GameClearSequence()
    {
        SoundManager.instance.SEPlay(SEType.GameClear);
        yield return new WaitForSeconds(2.0f);
        GameManager.gameState = GameManager.GameState.gameclear;
    }
}