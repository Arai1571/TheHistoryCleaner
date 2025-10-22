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


    // public Sprite[] stageSprites;
    // public SpriteRenderer sr;

    // [Header("演出（任意）")]
    // public GameObject shatterVFX;
    // public AudioClip shatterSE;
    // public bool showFinalSpriteOnBreak = true;
    // public float shatterShowTime = 0.06f;

    //int hits;

    // void Reset() { sr = GetComponent<SpriteRenderer>(); }
    // void Start() { ApplyStage(); }

    // void ApplyStage()
    // {
    //     if (sr == null || stageSprites == null || stageSprites.Length == 0) return;

    //     //段階＝現在ヒット数（S: 0,1,2  / L: 0,1,2,3）想定
    //     int idx = Mathf.Clamp(hits, 0, stageSprites.Length - 1);
    //     sr.sprite = stageSprites[idx];
    // }

    // public void Hit()
    // {
    //     if (hits >= damageNeeded) return;

    //     hits++;

    //     if (hits >= damageNeeded)
    //     {
    //         if (showFinalSpriteOnBreak && stageSprites != null && stageSprites.Length > 0)
    //         {
    //             sr.sprite = stageSprites[Mathf.Clamp(stageSprites.Length - 1, 0, stageSprites.Length - 1)];
    //         }
    //         Shatter();
    //     }
    //     else
    //     {
    //         ApplyStage();
    //     }
    // }
    
    //     async void Shatter()
    // {
    //     // GameManager.Instance.AddMoney(value);
    //     if (shatterVFX) Instantiate(shatterVFX, transform.position, Quaternion.identity);
    //     if (shatterSE)  AudioSource.PlayClipAtPoint(shatterSE, transform.position);

    //     if (showFinalSpriteOnBreak) await System.Threading.Tasks.Task.Delay((int)(shatterShowTime * 1000));
    //     Destroy(gameObject);
    // }

    // private void OnValidate()
    // {
    //     // 仕様に合わせて叩き回数を自動で揃える
    //     if (size == PotterySize.Small && damageNeeded != 2) damageNeeded = 2;
    //     if (size == PotterySize.Large && damageNeeded != 3) damageNeeded = 3;

    //     // 配列枚数の軽いチェック
    //     if (size == PotterySize.Small && stageSprites != null && stageSprites.Length > 0 && stageSprites.Length != 3)
    //     {
    //         Debug.LogWarning($"{name}: Small は stageSprites を3枚（綺麗/ひび/粉砕）");
    //     }
    //     if (size == PotterySize.Large && stageSprites != null && stageSprites.Length > 0 && stageSprites.Length != 4)
    //     {
    //         Debug.LogWarning($"{name}: Large は stageSprites を4枚（綺麗/ひび/ボロボロ/粉砕）");
    //     }
    // }
}
