using System.Collections;
using TMPro;
using UnityEngine;
using static GameManager;

public class TalkController : MonoBehaviour
{
    public MessageData message;   //ScriptableObject情報
    bool isPlayerInRange;         //プレイヤーが領域に入ったかどうか
    bool isTalk;                  //トークが開始されたかどうか
    GameObject canvas;            //トークUIを含んだCanvasオブジェクト
    GameObject talkPanel;         //対象となるトークUIパネル
    TextMeshProUGUI nameText;     //対象となるトークUIパネルの名前
    TextMeshProUGUI messageText;  //対象となるトークUIパネルのメッセージ

    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
        nameText = talkPanel.transform.Find("NameText").GetComponent<TextMeshProUGUI>();
        messageText = talkPanel.transform.Find("MessageText").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isPlayerInRange && !isTalk && Input.GetKeyDown(KeyCode.E))
        {
            StartConversation();  //トーク開始
        }
    }

    void StartConversation()
    {
        isTalk = true;  //トーク中フラグを立てる
        GameManager.gameState = GameState.talk; //ステータスをTalk
        talkPanel.SetActive(true); //トークUIパネルを表示
        Time.timeScale = 0;  //ゲーム進行スピードを０

        //TalkProcessコルーチンの発動
        StartCoroutine(TalkProcess());
    }

    //TalkProcessコルーチンの作成
    IEnumerator TalkProcess()
    {
        //対象としたScriptableObject(変数message)が扱っている入れるmsgArrayの数だけ繰り返す
        for (int i = 0; i < message.msgArray.Length; i++)
        {
            nameText.text = message.msgArray[i].name;
            messageText.text = message.msgArray[i].message;

            yield return new WaitForSecondsRealtime(0.1f);  //0.1秒待つ

            while (!Input.GetKeyDown(KeyCode.E))//Eキーが押されて”いない”間(押されるまで)
            {
                yield return null;  //何もしない
            }
        }
        EndConversation(); //トーク終了の処理
    }

    void EndConversation()
    {
        talkPanel.SetActive(false);
        GameManager.gameState = GameState.playing; //ゲームステータスをPlaying
        isTalk = false;  //トーク中を解除
        Time.timeScale = 1.0f; //ゲームスピードを元に戻す
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //プレイヤーが領域に入ったら
        if (collision.gameObject.CompareTag("Player"))
        {
            //フラグがON
            isPlayerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //プレイヤーが領域からでたら
        if (collision.gameObject.CompareTag("Player"))
        {
            //フラグがOFF
            isPlayerInRange = false;
        }
    }
}