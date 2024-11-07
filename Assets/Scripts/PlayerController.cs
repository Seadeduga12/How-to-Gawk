using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 10f;
    public float kickForce = 10f; // Kecepatan tendangan bola
    public GameObject ballPrefab; // Prefab bola
    private GameObject ball; // Bola yang ada di scene

    private Rigidbody ballRb; // Rigidbody bola

    void Start()
    {
        if (ballPrefab != null)
        {
            // Cek jika bola belum ada di scene, maka instansiasi bola
            if (ball == null)
            {
                ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
                ballRb = ball.GetComponent<Rigidbody>();
                // Set NetworkObject agar bola bisa disinkronkan di semua klien
                ball.GetComponent<NetworkObject>().Spawn();
            }
        }
    }

    void Update()
    {
        if (!IsOwner) return; // Pastikan hanya pemain yang memiliki kontrol atas objeknya

        // Gerakan pemain
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        transform.Translate(move);

        // Cek input untuk tendangan (misalnya tombol "Space")
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KickBall();
        }
    }

    void KickBall()
    {
        if (ball != null)
        {
            // Hitung arah tendangan berdasarkan posisi bola dan pemain
            Vector3 kickDirection = (ball.transform.position - transform.position).normalized;

            // Terapkan gaya ke bola
            ballRb.AddForce(kickDirection * kickForce, ForceMode.Impulse);
        }
    }
}
