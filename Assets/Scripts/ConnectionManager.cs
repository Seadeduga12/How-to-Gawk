using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class ConnectionManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public TextMeshProUGUI statusText;

    private void Start()
    {
        statusText.gameObject.SetActive(false);
        hostButton.onClick.AddListener(OnHostButtonClicked);
        joinButton.onClick.AddListener(OnJoinButtonClicked);
    }

    public void OnHostButtonClicked()
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "Hosting game...";
        StartCoroutine(LoadSceneAndStartNetwork("GameScene", true));
    }

    public void OnJoinButtonClicked()
    {
        statusText.gameObject.SetActive(true);
        statusText.text = "Joining game...";
        StartCoroutine(LoadSceneAndStartNetwork("GameScene", false));
    }

    private IEnumerator LoadSceneAndStartNetwork(string sceneName, bool isHost)
    {
        // Memulai pemuatan scene secara asinkron
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // Menunggu sampai scene selesai dimuat
        while (!asyncLoad.isDone)
        {
            yield return null; // Menunggu frame berikutnya
        }

        // Memastikan scene yang dimuat adalah GameScene
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            if (isHost)
            {
                NetworkManager.Singleton.StartHost();
                SpawnPlayer();
            }
            else
            {
                NetworkManager.Singleton.StartClient();
                SpawnPlayer();
            }
        }
    }

    private void SpawnPlayer()
    {
        // Pastikan bahwa player prefab telah didaftarkan dalam NetworkManager dan memiliki NetworkObject
        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Spawning player...");
            NetworkManager.Singleton.ConnectedClients[NetworkManager.Singleton.LocalClientId]
                .PlayerObject.GetComponent<NetworkObject>().Spawn();
        }
    }
}
