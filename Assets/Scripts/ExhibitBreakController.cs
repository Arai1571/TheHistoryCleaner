using UnityEngine;

public class ExhibitBreakController : MonoBehaviour
{
    [Header("壊れるスプライトの段階")]
    public Sprite[] pics; //壊れる展示品のスプライト
    public int num; //現在の段階
    SpriteRenderer sr;
    ExhibitData data;

    public bool CanBreak() => (pics != null && pics.Length > 0 && num < pics.Length - 1);

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        data = GetComponent<ExhibitData>();
        Apply();
    }

    void Apply()
    {
        if (sr == null || pics == null || pics.Length == 0) return;
        //段階＝現在ヒット数（陶器ならS: 0,1,2  / L: 0,1,2,3）
        num = Mathf.Clamp(num, 0, pics.Length - 1);
        sr.sprite = pics[num];
        ApplyStage();
    }

    //壊れる絵を１段階づつ進める
    void ApplyStage()
    {
        if (pics != null && pics.Length > 0)
        {
            int idx = Mathf.Clamp(num, 0, pics.Length - 1);
            sr.sprite = pics[idx];
        }
    }

    // 1ダメージ（モップ/スプレー）が入った時にPlayerから呼ぶ
    public void HitOnce()
    {
        bool broken = data ? data.ApplyDamage(1) : false;

        // 段階表示を進める
        if (pics != null && pics.Length > 0 && num < pics.Length - 1)
        {
            num++;
            ApplyStage();
        }

        if (broken)
        {
            // 完全破壊の演出があればここで（SE/VFX/Destroy 等）
            // Destroy(gameObject, 0.05f);
        }
    }
}
