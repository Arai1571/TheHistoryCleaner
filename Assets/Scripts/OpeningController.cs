using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using static GameManager;

public class OpeningController : MonoBehaviour
{
    [Header("Scriptable Object")]
    public OpeningMessageData message; //オープニング用のメッセージデータ

    [Header("References")]
    public GameObject player;//表情を切り替える対象（プレイヤー）
    private SpriteRenderer playerRenderer;//プレイヤーのスプライト
    GameObject canvas;
    GameObject talkPanel;
    TextMeshProUGUI nameText;
    TextMeshProUGUI messageText;

    [Header("演出用")]
    public Camera mainCam;//カメラ（ズーム制御用）
    public AudioSource audioSource;//通常SE再生用
    private Coroutine faceAnimCoroutine; // 表情アニメコルーチンを管理
    int currentLineIndex = 0; // クラス内変数として追加
    bool bugNoiseTriggered = false;//Title Bgmを停止するトリガーとなる効果音

    void Start()
    {
        // カメラ自動取得
        if (mainCam == null)
            mainCam = Camera.main;

        // プレイヤーのSpriteRendererを取得
        if (player != null)
            playerRenderer = player.GetComponent<SpriteRenderer>();

        // UI参照取得
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();

        // ステータス設定
        GameManager.gameState = GameState.opening;

        // トーク開始
        StartCoroutine(StartConversation());

    }

    IEnumerator StartConversation()
    {
        talkPanel.SetActive(true);
        yield return StartCoroutine(PlayOpening());
    }

    IEnumerator PlayOpening()
    {
        for (currentLineIndex = 0; currentLineIndex < message.msgArray.Length; currentLineIndex++)
        {
            var line = message.msgArray[currentLineIndex];

            // 古いアニメを止める
            if (faceAnimCoroutine != null)
            {
                StopCoroutine(faceAnimCoroutine);
                faceAnimCoroutine = null;
            }

            // 表情アニメ（瞬き）を再生開始
            if (playerRenderer && line.faceSprites != null && line.faceSprites.Length > 0)
                faceAnimCoroutine = StartCoroutine(PlayFaceAnimation(line.faceSprites, 0.15f));

            // 名前と台詞の更新
            nameText.text = line.speaker;
            messageText.text = "";

            // カメラズーム演出
            StartCoroutine(CameraZoom(line.zoomLevel));

            // 効果音を再生
            if (line.sfx)
            {
                // AudioSourceが有効でなければ再生成
                if (audioSource == null || !audioSource.isActiveAndEnabled)
                {
                    Debug.LogWarning("AudioSourceが無効、再作成します。");
                    audioSource = gameObject.AddComponent<AudioSource>();
                }

                float vol = 1.0f; // デフォルト音量

                // 効果音名ごとに SoundManager の設定値を参照
                switch (line.sfx.name)
                {
                    case "SE_CatBot": vol = SoundManager.instance.seVolumeCatBot; break;
                    case "SE_CatBot1": vol = SoundManager.instance.seVolumeCatBot1; break;
                    case "SE_CatBot2": vol = SoundManager.instance.seVolumeCatBot2; break;
                    case "SE_GotBot1": vol = SoundManager.instance.seVolumeGotBot1; break;
                    case "SE_GotBot2": vol = SoundManager.instance.seVolumeGotBot2; break;
                    case "SE_GotBot3": vol = SoundManager.instance.seVolumeGotBot3; break;
                    case "SE_RoboTalkingBug": vol = SoundManager.instance.seVolumeRoboTalkingBug; break;
                    case "SE_BugNoise": vol = SoundManager.instance.seVolumeBugNoise; break;
                }

                // BugNoiseだけは特別に扱う->トリガーとなってBGMを止めるため
                if (line.sfx.name == "SE_BugNoise")
                {
                    // BGMを停止
                    SoundManager.instance.StopBgm();

                    // BugNoiseを確実に再生（AudioSourceが無効でもOK）
                    audioSource.PlayOneShot(line.sfx, vol);

                    bugNoiseTriggered = true;
                }
                else
                {
                    // 通常SEを再生
                    audioSource.PlayOneShot(line.sfx);
                }
            }

            // テキスト出力（1文字ずつ）
            foreach (char c in line.text)
            {
                messageText.text += c;
                yield return new WaitForSecondsRealtime(0.07f);
            }

            // 次のセリフまで待つ
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

            // BugNoise再生後、特定のテキストが来たらBGM再開
            if (bugNoiseTriggered && line.text.Contains("人間に造られし全ての清掃ロボたち"))
            {
                Debug.Log("Trigger Text Detected → Play InGame BGM");
                SoundManager.instance.PlayBgm(BGMType.InGame);
                bugNoiseTriggered = false;
            }
        }

        EndConversation();
    }

    //プレイヤーの瞬きアニメーション
    IEnumerator PlayFaceAnimation(Sprite[] sprites, float interval = 0.2f)
    {
        if (sprites == null || sprites.Length == 0) yield break;

        // 現在のセリフ番号を控える
        int currentLine = currentLineIndex;  // PlayOpening()側で今の行を入れる変数を用意しておく

        while (currentLineIndex == currentLine && GameManager.gameState == GameState.opening)
        {
            // 瞬き1回
            for (int i = 0; i < sprites.Length; i++)
            {
                playerRenderer.sprite = sprites[i];
                yield return new WaitForSecondsRealtime(interval);
            }

            // 閉じたまま少し静止
            yield return new WaitForSecondsRealtime(interval * 10);

            // 戻る
            for (int i = sprites.Length - 2; i >= 0; i--)
            {
                playerRenderer.sprite = sprites[i];
                yield return new WaitForSecondsRealtime(interval);
            }

            // 次の瞬きまでの待ち時間（自然な休憩）
            yield return new WaitForSecondsRealtime(Random.Range(1f, 3f));
        }
    }

    //トーク終了時の処理
    void EndConversation()
    {
        talkPanel.SetActive(false);
        GameManager.gameState = GameState.playing;
        GameManager.ChangeScene("Main", 1.0f);
    }

    //カメラズーム演出
    IEnumerator CameraZoom(float target)
    {
        float start = mainCam.orthographicSize;
        float t = 0;
        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 1.5f;
            mainCam.orthographicSize = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }
}
