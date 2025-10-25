using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using static GameManager;

public class OpeningController : MonoBehaviour
{
    [Header("Scriptable Object")]
    public OpeningMessageData message;

    [Header("References")]
    public GameObject player;             
    private SpriteRenderer playerRenderer; 
    GameObject canvas;
    GameObject talkPanel;
    TextMeshProUGUI nameText;
    TextMeshProUGUI messageText;

    [Header("Camera & Audio")]
    public Camera mainCam;
    public AudioSource audioSource;

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
        for (int i = 0; i < message.msgArray.Length; i++)
        {
            var line = message.msgArray[i];

            // プレイヤーの表情を切り替える
            if (playerRenderer && line.face)
                playerRenderer.sprite = line.face;

            // 名前と台詞
            nameText.text = line.speaker;
            messageText.text = "";

            // カメラズーム
            StartCoroutine(CameraZoom(line.zoomLevel));

            // 効果音
            if (line.sfx)
                audioSource.PlayOneShot(line.sfx);

            // テキスト出力
            foreach (char c in line.text)
            {
                messageText.text += c;
                yield return new WaitForSecondsRealtime(0.07f);
            }

            // 次のセリフまでの待機
            yield return new WaitForSecondsRealtime(line.waitAfter);

            // Eキー押下で次へ
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }

        EndConversation();
    }

    void EndConversation()
    {
        talkPanel.SetActive(false);
        GameManager.gameState = GameState.playing;
        GameManager.ChangeScene("Main", 1.0f);
    }

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
