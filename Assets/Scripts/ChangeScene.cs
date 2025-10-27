using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NewGame()
    {
        //ゲームの初期化->Opening
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Opening");
    }

    public void ReStart()
    {
        //ゲームの初期化->Main
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main");
    }

    //タイトルシーンへ戻る
    public void toTitle()
    {
        //ゲームステートを初期化
        GameManager.gameState = GameManager.GameState.title;

        //損害金額を０に戻す
        GameManager.TotalValueJPY = 0;

        //引数に指定した名前のシーンに切り替えしてくれるメソッド呼び出し
        SceneManager.LoadScene("Title");
    }
}
