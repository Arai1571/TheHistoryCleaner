using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class OpeningController : MonoBehaviour
{
    public OpeningMessageData message; // ScriptableObject情報
    public Image faceImage;             // プレイヤーの表情
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI messageText;
    public Camera mainCam;
    public AudioSource audioSource;

    void Start()
    {
        StartCoroutine(PlayOpening());
    }

    IEnumerator PlayOpening()
    {
        for (int i = 0; i < message.msgArray.Length; i++)
        {
            var line = message.msgArray[i]; // 構造体全体

            // 表情
            if (faceImage && line.face) faceImage.sprite = line.face;

            // 名前・台詞リセット
            nameText.text = line.speaker;
            messageText.text = "";

            // カメラズーム
            StartCoroutine(CameraZoom(line.zoomLevel));

            // 効果音
            if (line.sfx) audioSource.PlayOneShot(line.sfx);

            // テキスト出力（1文字ずつ）
            foreach (char c in line.text)
            {
                messageText.text += c;
                yield return new WaitForSeconds(0.03f);
            }

            // セリフ間ウェイト
            yield return new WaitForSeconds(line.waitAfter);
        }

        Debug.Log("Opening finished!");

        // オープニング終了 → メインへ
        GameManager.ChangeScene("Main", 1.0f);
    }

    IEnumerator CameraZoom(float target)
    {
        float start = mainCam.orthographicSize;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            mainCam.orthographicSize = Mathf.Lerp(start, target, t);
            yield return null;
        }
    }
}
