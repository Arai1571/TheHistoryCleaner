using UnityEngine;

public enum PaintingRarity { S, A, B, C, D, E, F, G, H } //ランク：Sはレア→Hは普通

public class PaintingData : MonoBehaviour
{
    [Header("識別")]
    public PaintingRarity rarity;

    [Header("基本設定")]
    public int damageNeeded=1; //必要な攻撃回数(絵なら1回)
    public int value;        //損害額(サイズ・レア度で変える)
}
