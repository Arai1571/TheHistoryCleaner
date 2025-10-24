using UnityEngine;

public class DetectionSensor : MonoBehaviour
{
    private GuardmanController guard;

    void Start()
    {
        guard = GetComponentInParent<GuardmanController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            guard?.SetRandomPatrolDirection(); // ぶつかったら方向転換
        }
    }
}