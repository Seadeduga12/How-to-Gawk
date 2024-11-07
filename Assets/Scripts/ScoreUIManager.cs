using TMPro;
using Unity.Netcode;
using UnityEngine;

public class ScoreUIManager : MonoBehaviour
{
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    private void Start()
    {
        // Check for ScoreManager instance (attach this script to NetworkManager or another singleton)
        if (NetworkManager.Singleton.IsServer)
        {
            // Only the server should update the score (then clients will receive updates)
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        }
    }

    private void OnServerStarted()
    {
        var scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            // Register for score updates
            scoreManager.player1Score.OnValueChanged += UpdatePlayer1Score;
            scoreManager.player2Score.OnValueChanged += UpdatePlayer2Score;
        }
    }

    private void UpdatePlayer1Score(int previous, int current)
    {
        player1ScoreText.text = "Player 1: " + current.ToString();
    }

    private void UpdatePlayer2Score(int previous, int current)
    {
        player2ScoreText.text = "Player 2: " + current.ToString();
    }
}
