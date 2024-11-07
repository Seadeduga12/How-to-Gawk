using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public NetworkVariable<int> player1Score = new NetworkVariable<int>(0);
    public NetworkVariable<int> player2Score = new NetworkVariable<int>(0);

    public void GoalScored(int player)
    {
        if (!IsServer) return;

        if (player == 1)
            player1Score.Value++;
        else
            player2Score.Value++;
    }
}
