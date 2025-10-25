using UnityEngine;

public class OilData : MonoBehaviour
{
    Rigidbody2D rbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.SEPlay(SEType.Pickup); //アイテムゲット音を鳴らす

            //PlayerのHPが満タンより少なくなっていれば満タンに充電する
            if (GameManager.playerHP < 100)
            {
                Invoke(nameof(PlayCharge), 0.5f); // 0.5秒後に回復音を鳴らす
                GameManager.playerHP = 100;
            }

            //アイテム取得の演出
            GetComponent<CircleCollider2D>().enabled = false;
            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);
        }
    }
    void PlayCharge()
    {
        SoundManager.instance.SEPlay(SEType.Oil); //音を鳴らす
    }
}
