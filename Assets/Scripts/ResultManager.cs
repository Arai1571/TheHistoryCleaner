using TMPro;
using UnityEngine;

public class ResultManager : MonoBehaviour
{
    public GameObject scoreText;
    void Start()
    {
        //String型に型変換
        scoreText.GetComponent<TextMeshProUGUI>().text = GameManager.TotalValueJPY.ToString("N0") + " 円";
    }

}
