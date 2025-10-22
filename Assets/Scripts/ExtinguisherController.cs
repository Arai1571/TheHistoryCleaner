using UnityEngine;

public class ExtinguisherController : MonoBehaviour
{
    public float deleteTime = 2.0f;//自己消滅までの時間
    public GameObject smokePrefab; //自己消滅と引き換えに生成するプレハブ

    void Start()
    {
        Invoke("FieldExpansion", deleteTime);
    }

    void FieldExpansion()
    {
        Instantiate(smokePrefab, transform.position, Quaternion.identity); //消化器と同じ場所にバリア生成
        Destroy(gameObject);  //消化器は消滅
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Guardman"))
        {
            FieldExpansion();
        }
    }
}
