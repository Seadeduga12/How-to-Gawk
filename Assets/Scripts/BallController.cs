using Unity.Netcode;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on Ball prefab!");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (rb != null)
        {
            if (IsServer)
            {
                rb.bodyType = RigidbodyType2D.Dynamic; // Server menetapkan Rigidbody menjadi dynamic
            }
            else
            {
                rb.bodyType = RigidbodyType2D.Kinematic; // Klien tidak memanipulasi fisika bola
            }
        }
    }
}
