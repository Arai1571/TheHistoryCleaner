using UnityEngine;

public enum PotterySize { Small, Large }
public enum PotteryColor { Earth, Pink, Silver, Gold }

public class PotteryData : MonoBehaviour
{
    [Header("識別")]
    public PotterySize size = PotterySize.Small;
    public PotteryColor color = PotteryColor.Earth;

    [Header("基本設定")]
    public int damageNeeded; //必要な攻撃回数(絵なら1回、S陶器なら2回、L陶器なら3回)
    public int value;        //損害額(サイズ・色で変える)
}
