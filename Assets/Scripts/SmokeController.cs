using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public float deleteTime = 5.0f;
    void Start()
    {
        //Smokeの音
        SoundManager.instance.SEPlay(SEType.Smoke); 
        //DeleteTime秒後に消滅
        Destroy(gameObject, deleteTime);
    }
}

