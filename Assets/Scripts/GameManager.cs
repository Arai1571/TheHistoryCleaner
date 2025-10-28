using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //ゲーム状態を管理する列挙型
    public enum GameState
    {
        title,      //タイトル画面
        opening,//オープニング中
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

    public static GameManager instance;
    float clearTimer = 0f;//クリアしてから推移するまでの時間猶予

    void Awake()
    {
        // シングルトン化(既存のインスタンスを残す)
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (SoundManager.instance == null) return;

        // Mainシーンを単体で再生した場合、強制的に初期化
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Main" && gameState == GameState.title)
        {
            Debug.LogWarning("Mainシーン単体起動を検出 → ゲームステートを初期化");
            gameState = GameState.playing;
        }

        // すでにBGMが流れているならスキップ
        if (SoundManager.playingBGM != BGMType.None) return;
        // シーンの名前を取得
        string sceneName = currentScene.name;

        // シーン名から自動的に状態を設定
        switch (sceneName)
        {
            case "Title":
                gameState = GameState.title;
                SoundManager.instance.PlayBgm(BGMType.Title);
                break;
            case "Opening":
                gameState = GameState.opening;
                SoundManager.instance.PlayBgm(BGMType.Opening);
                break;
            case "Main":
                gameState = GameState.playing;
                SoundManager.instance.PlayBgm(BGMType.InGame);
                break;
            case "Ending":
                gameState = GameState.ending;
                SoundManager.instance.PlayBgm(BGMType.Ending);
                break;
            default:
                gameState = GameState.playing;
                SoundManager.instance.PlayBgm(BGMType.InGame);
                break;
        }
    }

    // 任意のシーン遷移を呼び出す汎用メソッド
    public static void ChangeScene(string sceneName, float delay = 0f)
    {
        if (instance != null)
            instance.StartCoroutine(instance.ChangeSceneAfterDelay(sceneName, delay));
    }

    // 遅延付きシーン遷移
    IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }

    public void Update()
    {
        // 現在の状態をデバッグ表示
        Debug.Log($"[GameManager] 現在のステート: {gameState}");
        
        // ゲームクリア時は少し待ってからエンディングへ
        if (gameState == GameState.gameclear)
        {
            clearTimer += Time.deltaTime;
            if (clearTimer >= 5.0f) // 5秒後に遷移
            {
                clearTimer = 0f;
                gameState = GameState.ending;
                StartCoroutine(Ending());
            }
            return;
        }

        // ゲームオーバーのときはすぐ遷移
        if (gameState == GameState.gameover)
        {
            gameState = GameState.ending;
            StartCoroutine(Ending());
        }
    }

    //ゲームオーバーもしくはクリアの際に発動するコルーチン
    IEnumerator Ending()
    {
        yield return new WaitForSeconds(5);  //5秒まつ
        SoundManager.instance.SEPlay(SEType.News); //速報音を鳴らす
        SceneManager.LoadScene("Ending");   //タイトルに戻る
    }

    //獲得被害総額加算メソッド
    public static void AddDamage(long amount)
    {
        if (amount <= 0) return;
        TotalValueJPY += amount;
        OnTotalValueChanged?.Invoke(TotalValueJPY);
        // Debug.Log($"TotalValueJPY = {TotalValueJPY:N0}");
    }

    public static void ResetAll()
    {
        hasSpotLight = false;
        playerHP = 100;
        Extinguisher = 0;
        TotalValueJPY = 0;
        OnTotalValueChanged?.Invoke(TotalValueJPY);
        // Endingの時はタイトルに戻さない
        if (gameState != GameState.ending)
            gameState = GameState.title;
    }
}
