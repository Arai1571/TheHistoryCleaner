using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public TextMeshProUGUI ExtinguisherText; //消化器の数を記すテキストオブジェクトのコンポーネント
    public TextMeshProUGUI TotalValueJPYText; //獲得損害総額

    public Slider playerHP;  //対象スライダー

    int currentExtinguisherCount;
    int currentHPCount;

    void OnEnable()
    {
        // 総額の変化を購読（シーン入替対策で先に購読→初期反映）
        GameManager.OnTotalValueChanged += RefreshTotalValue;
        RefreshTotalValue(GameManager.TotalValueJPY);
    }

    void OnDisable()
    {
        GameManager.OnTotalValueChanged -= RefreshTotalValue;
    }

    void Start()
    {
        currentExtinguisherCount = GameManager.Extinguisher; //初期設定
        currentHPCount = GameManager.playerHP;

        if (ExtinguisherText) ExtinguisherText.text = currentExtinguisherCount.ToString();

        if (playerHP)
        {
            playerHP.minValue = 0;
            playerHP.maxValue = GameManager.playerHP; // 最大値を初期HPに
            playerHP.value = currentHPCount;
        }
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


    // 総額のUI更新（イベントで呼ばれる）
    void RefreshTotalValue(long value)
    {
        if (TotalValueJPYText)TotalValueJPYText.text = $"{value:N0}円";
    }
}