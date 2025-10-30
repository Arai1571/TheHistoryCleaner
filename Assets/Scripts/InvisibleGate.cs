using UnityEngine;

public class InvisibleGate : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.hasEnteredMuseum = true; //館内に入ったことを保証するー＞Gate Sensorがtrueを確認してゲームクリアにする
            Debug.Log("プレイヤーがゴールGateに近づいた（ExitGate有効化）");
        }
    }
}
