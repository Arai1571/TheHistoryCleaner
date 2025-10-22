using UnityEngine;
using UnityEngine.UI;

public class ExhibitHPBar : MonoBehaviour
{
    public ExhibitData target;       // 親のExhibitDataをInspectorでセット（または自動取得）
    public Slider slider;            // 自分のSlider参照
    public bool hideWhenFull = true; // 初期は非表示にしたい場合

    void Awake()
    {
        if (!target) target = GetComponentInParent<ExhibitData>();
        if (!slider) slider = GetComponent<Slider>();

        slider.minValue = 0;
        slider.wholeNumbers = true;
        slider.maxValue = target.MaxHP;
        slider.value = target.CurrentHP;

        // 初期状態は非表示
        gameObject.SetActive(false);
    }

    void LateUpdate()
    {
        if (!target || !slider) return;

        slider.maxValue = target.MaxHP;
        slider.value = target.CurrentHP;

        // HPが完全に0なら確実に非表示
        if (target.CurrentHP <= 0)
            slider.value = 0f;

        // 破壊後、Fill部分だけ隠す
        if (slider.fillRect)
        {
            bool shouldShowFill = target.CurrentHP > 0;
            slider.fillRect.gameObject.SetActive(shouldShowFill);
        }
    }

    public void RefreshNow()
    {
        if (!target || !slider) return;

        slider.maxValue = target.MaxHP;
        slider.value = target.CurrentHP;
    }
}
