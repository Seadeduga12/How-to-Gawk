using Unity.Netcode;
using UnityEngine;

public class BallController : NetworkBehaviour
{
    private Rigidbody2D rb;

    // Variabel skor untuk masing-masing pemain
    public NetworkVariable<int> player1Score = new NetworkVariable<int>();
    public NetworkVariable<int> player2Score = new NetworkVariable<int>();

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return; // Pastikan ini hanya diproses di server

        if (collision.CompareTag("LeftGoal"))
        {
            player2Score.Value++; // Tambahkan skor untuk Player 2
            ResetBallPosition();
        }
        else if (collision.CompareTag("RightGoal"))
        {
            player1Score.Value++; // Tambahkan skor untuk Player 1
            ResetBallPosition();
        }
    }

    private void ResetBallPosition()
    {
        // Atur ulang posisi bola ke titik awal setelah mencetak gol
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero; // Menghentikan pergerakan bola
    }
}
