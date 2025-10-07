using UnityEngine;

public class SpotLightItem : MonoBehaviour
{
     Rigidbody2D rbody;

    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.bodyType = RigidbodyType2D.Static;

        if(GameManager.hasSpotLight) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            GameManager.hasSpotLight = true;//触れたPlayerの持っているコンポーネントhasSpotLightをオン

            CircleCollider2D col = GetComponent<CircleCollider2D>();
            col.enabled = false;

            rbody.bodyType = RigidbodyType2D.Dynamic;
            rbody.AddForce(Vector2.up * 5.0f, ForceMode2D.Impulse);

            Destroy(gameObject, 0.5f);

            collision.GetComponent<PlayerController>().SpotLightCheck(); //Playerにライトをつけさせる
        }
    }
}
