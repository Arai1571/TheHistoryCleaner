using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //ゲーム状態を管理する列挙型
    public enum GameState
    {
        playing,    // プレイ中
        talk,       // トーク中
        gameover,   // ゲームオーバー
        gameclear,  // ゲームクリア
        ending      // エンディング
    }

    public static GameState gameState; //ゲームのステータス
    public static bool hasSpotLight; // スポットライトを所持しているかどうか。
    public static int playerHP = 100;  //プレイヤーの充電HP
    public static int Extinguisher = 0; //消化器の残数

    public static long TotalValueJPY;//獲得被害総額
    public static event Action<long> OnTotalValueChanged;

    void Start()
    {
        //ゲーム開始状態にする
        gameState = GameState.playing;

        //シーン名の取得
        Scene currentScene = SceneManager.GetActiveScene();
        // シーンの名前を取得
        string sceneName = currentScene.name;
    }

    public void Update()
    {
        //ゲームオーバーになったらタイトルに戻る
        if (gameState == GameState.gameover || gameState == GameState.gameclear)
        {
            //時間差でシーン切り替え
            StartCoroutine(TitleBack());
        }
    }

    //ゲームオーバーの際に発動するコルーチン
    IEnumerator TitleBack()
    {
        yield return new WaitForSeconds(5);  //5秒まつ
        SceneManager.LoadScene("Title");   //タイトルに戻る
    }

    //獲得被害総額加算メソッド
    public static void AddDamage(long amount)
    {
        if (amount <= 0) return;
        TotalValueJPY += amount;
        OnTotalValueChanged?.Invoke(TotalValueJPY);
        // Debug.Log($"TotalValueJPY = {TotalValueJPY:N0}");
    }

    public static void ResetDamage()
    {
        TotalValueJPY = 0;
        OnTotalValueChanged?.Invoke(TotalValueJPY);
    }
}
