using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void NewGame()
    {
        //ゲームの初期化
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Opening");
    }

    public void ReStart()
    {
        //ゲームの初期化
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Main");
    }
}
