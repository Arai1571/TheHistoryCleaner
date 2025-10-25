using UnityEngine;

public class ExtinguisherPickup : MonoBehaviour
{
    Rigidbody2D rbody;

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
            GameManager.Extinguisher++;

            //取得演出
            this.gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);
        }
    }
}
