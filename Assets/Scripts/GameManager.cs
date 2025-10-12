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
    public static int playerHP = 3;  //プレイヤーの充電HP

    void Start()
    {
        //まずはゲーム開始状態にする
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

            //Invokeメソッドでも可能
        }
    }

    //ゲームオーバーの際に発動するコルーチン
    IEnumerator TitleBack()
    {
        yield return new WaitForSeconds(5);  //5秒まつ
        SceneManager.LoadScene("Title");   //タイトルに戻る
    }

}
