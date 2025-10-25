using UnityEngine;
public enum ExhibitKind { Painting, Pottery }

// 絵のレア度（高いほど価値が高い想定）
public enum PaintingRarity { S, A, B, C, D, E, F, G, H }//ランク：Sはレア→Hは普通

//陶器のバリエーション
public enum PotterySize { Small, Large }
public enum PotteryColor { Earth, Pink, Silver, Gold }

public class ExhibitData : MonoBehaviour
{
    [Header("共通")]
    public ExhibitKind kind;
    [Min(1)] public int damageNeeded; //最終的に壊すまでに必要な攻撃回数
    [Min(0)] public long valueJPY = 0; //壊したら加算される被害額
    int currentHP; //展示品が持つHP

    [Header("絵 (Painting) 専用")]
    public PaintingRarity paintingRarity = PaintingRarity.C;

    [Header("陶器 (Pottery) 専用")]
    public PotterySize potterySize = PotterySize.Small;
    public PotteryColor potteryColor = PotteryColor.Earth;

    public int MaxHP => damageNeeded;
    public int CurrentHP => Mathf.Max(0, currentHP);
    public float HpRatio => (float)CurrentHP / MaxHP;


    void Awake()
    {
        // 初期値
        if (damageNeeded < 1) damageNeeded = CalcSuggestedDamageNeeded();
        currentHP = damageNeeded; //HP満タンで開始
        if (valueJPY < 0) valueJPY = 0;
    }

    //種類に応じた必要攻撃回数を返す（例：絵=1、陶器S=2、陶器L=3）
    public int CalcSuggestedDamageNeeded()
    {
        switch (kind)
        {
            case ExhibitKind.Painting:
                return 1; // 絵は一撃で漂白・破壊
            case ExhibitKind.Pottery:
                return (potterySize == PotterySize.Small) ? 2 : 3;
            default:
                return 1;
        }
    }

    //ダメージ適用
    public bool ApplyDamage(int amount = 1)
    {
        if (currentHP <= 0) return false; // 既に破壊済み
        currentHP -= Mathf.Max(1, amount);

        //もしすでに破壊済みなら
        if (currentHP <= 0)
        {
            //加算
            GameManager.AddDamage(valueJPY);
            Invoke(nameof(AddMoney), 0.5f); // 0.5秒後にお金の獲得音を鳴らす

            return true;
        }
        return false;
    }

    void AddMoney()
    {
        SoundManager.instance.SEPlay(SEType.Money); //お金の獲得音を鳴らす
    }

    void OnValidate()
    {
        // 0やマイナスになってたら最低値で保護
        if (damageNeeded < 1) damageNeeded = CalcSuggestedDamageNeeded();
        if (valueJPY < 0) valueJPY = 0;
    }
}
