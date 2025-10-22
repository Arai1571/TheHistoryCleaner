using UnityEngine;

public class ExhibitBreakController : MonoBehaviour
{
    public Sprite[] pics; //壊れる展示品のスプライト
    public int num; //現在の段階
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        Apply();
    }

    void Apply()
    {
        if (pics == null || pics.Length == 0) return;
        num = Mathf.Clamp(num, 0, pics.Length - 1);
        sr.sprite = pics[num];
    }

    //プレイヤーから呼ばれる破壊演出（壊れる絵に差し替えする）メソッド
    public void Break()
    {
        if (pics == null || pics.Length == 0) return;

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
