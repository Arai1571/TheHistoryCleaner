using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI ExtinguisherText; //消化器の数を記すテキストオブジェクトのコンポーネント
    public TextMeshProUGUI MoneyCountText; //獲得損害総額

    public Slider playerHP;  //対象スライダー

    int currentExtinguisherCount; //差分
    int currentMoneyCount;
    int currentHPCount; 

    void Start()
    {
        currentExtinguisherCount = GameManager.Extinguisher; //初期設定
        currentMoneyCount = GameManager.MoneyCount;
        currentHPCount = GameManager.playerHP;

        ExtinguisherText.text = currentExtinguisherCount.ToString(); //UIに反映
        ExtinguisherText.text= currentMoneyCount.ToString();
        playerHP.value = currentHPCount; //UIに反映
    }

    void Update()
    {
        //消化器の保持数に変化があれば
        if (currentExtinguisherCount != GameManager.Extinguisher)
        {
            currentExtinguisherCount = GameManager.Extinguisher;
            ExtinguisherText.text = currentExtinguisherCount.ToString();
        }

        //PlayerのHPに変化があれば
        if (currentHPCount != GameManager.playerHP)
        {
            currentHPCount = GameManager.playerHP;
            playerHP.value = currentHPCount;
        }
    }
}