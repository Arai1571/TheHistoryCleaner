using System.Collections;
using UnityEngine;

public class SpotLightItem : MonoBehaviour
{
    Rigidbody2D rbody;
    public float lightDelay = 0.5f;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;

        if (GameManager.hasSpotLight) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.SEPlay(SEType.Pickup); //アイテムゲット音を鳴らす
            GameManager.hasSpotLight = true;//触れたPlayerの持っているコンポーネントhasSpotLightをオン

            CircleCollider2D col = GetComponent<CircleCollider2D>();
            col.enabled = false;

            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);

            Destroy(gameObject, 0.5f);

            // 即時ではなく、少し待ってからSpotLightCheckを呼ぶ
            var pc = collision.GetComponent<PlayerController>();
            if (pc) StartCoroutine(DelaySpotOn(pc));
        }

        IEnumerator DelaySpotOn(PlayerController pc)
        {
            yield return new WaitForSeconds(lightDelay);
            if (pc) pc.SpotLightCheck();
        }
    }
}
