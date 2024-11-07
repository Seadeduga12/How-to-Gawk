using UnityEngine;

public class NetworkManagerSetup : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
