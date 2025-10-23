using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    public void StartGame()
    {
        //ゲームの初期化
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Opening");
    }

}
