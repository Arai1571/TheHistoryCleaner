using UnityEngine;

public class ExhibitBreakController : MonoBehaviour
{
    [Header("壊れるスプライトの段階")]
    [Tooltip("陶器ならSmall: 3枚（綺麗/ひび/粉砕） Large: 4枚（綺麗/ひび/ボロボロ/粉砕）")]
    public Sprite[] pics; //壊れる展示品のスプライト
    public int num; //現在の段階
    SpriteRenderer sr;

    public bool CanBreak() => (pics != null && pics.Length > 0 && num < pics.Length - 1);

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Apply();
    }

    void Apply()
    {
        if (sr == null || pics == null || pics.Length == 0) return;
        //段階＝現在ヒット数（S: 0,1,2  / L: 0,1,2,3）想定
        num = Mathf.Clamp(num, 0, pics.Length - 1);
        sr.sprite = pics[num];
    }

    //プレイヤーから呼ばれる破壊演出（壊れる絵に差し替えする）メソッド
    public void Break()
    {
        if (sr == null || pics == null || pics.Length == 0) return;

        switch (num)
        {
            case 0:
                sr.sprite = pics[1];
                num++;
                break;
            case 1:
                sr.sprite = pics[2];
                num++;
                break;
            case 2:
                sr.sprite = pics[3];
                num++;
                break;
        }
    }
}
