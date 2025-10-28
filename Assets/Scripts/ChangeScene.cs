using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void NewGame()
    {
        GameManager.ResetAll();
        GameManager.gameState = GameManager.GameState.opening;
        SceneManager.LoadScene("Opening");
    }

    public void ReStart()
    {  
        GameManager.ResetAll();
        GameManager.gameState = GameManager.GameState.playing;
        SceneManager.LoadScene("Main");
    }

    //タイトルシーンへ戻る
    public void toTitle()
    {
        GameManager.ResetAll();
        GameManager.gameState = GameManager.GameState.title;
        //引数に指定した名前のシーンに切り替えしてくれるメソッド呼び出し
        SceneManager.LoadScene("Title");
    }
}
