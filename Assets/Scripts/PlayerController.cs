using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float kickForce = 10f;
    public float kickUpwardForce = 5f; // Gaya ke atas tambahan untuk melambungkan bola
    public GameObject ballPrefab;
    private GameObject ball;
    private Rigidbody2D ballRb;
    private Rigidbody2D rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (IsServer && ball == null) // Pastikan bola di-spawn hanya di server
        {
            ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
            ballRb = ball.GetComponent<Rigidbody2D>();
            ball.GetComponent<NetworkObject>().Spawn();
        }
        else if (ball != null)
        {
            ballRb = ball.GetComponent<Rigidbody2D>(); // Ambil Rigidbody2D bola
        }
    }

    void Update()
    {
        if (!IsOwner) return;

        // Gerakan pemain
        float horizontal = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        Vector3 move = new Vector3(horizontal, 0, 0) * moveSpeed * Time.deltaTime;
        transform.Translate(move);

        // Membalikkan sprite berdasarkan arah gerakan
        if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Lompat
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            Jump();
        }

        // Kick bola
        if (Input.GetKeyDown(KeyCode.Space))
        {
            KickBall();
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    void KickBall()
    {
        if (ballRb != null && ball != null)
        {
            Vector3 kickDirection = (ball.transform.position - transform.position).normalized + Vector3.up * 0.5f;

            ballRb.AddForce(new Vector2(kickDirection.x * kickForce, kickUpwardForce), ForceMode2D.Impulse);

            Debug.Log("Ball kicked with upward force: " + (kickDirection * kickForce));
        }
        else
        {
            Debug.LogWarning("Ball or ball Rigidbody not found!");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
