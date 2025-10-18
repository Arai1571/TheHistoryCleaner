using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public float deleteTime = 5.0f;
    void Start()
    {
        //DeleteTime秒後に消滅
        Destroy(gameObject, deleteTime);
    }
}

