using UnityEngine;

public class GateSensor : MonoBehaviour
{
    private GateController gate; //親の参照

    void Start()
    {
        gate = GetComponentInParent<GateController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gate?.SetGateOpen(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gate?.SetGateOpen(false);
        }
    }
}
