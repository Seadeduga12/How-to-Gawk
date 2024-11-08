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

    private void OnHostButtonClicked()
    {
        StartCoroutine(LoadSceneAndStartHost());
    }

    private void OnJoinButtonClicked()
    {
        StartCoroutine(LoadSceneAndStartClient());
    }

    private IEnumerator LoadSceneAndStartHost()
    {
        // Menampilkan status teks
        statusText.gameObject.SetActive(true);
        statusText.text = "Hosting game...";

        // Memulai pemuatan scene secara asinkron
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

        // Menunggu sampai scene selesai dimuat
        while (!asyncLoad.isDone)
        {
            yield return null; // Menunggu frame berikutnya
        }

        // Memastikan scene yang dimuat adalah "GameScene"
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            NetworkManager.Singleton.StartHost();
        }
    }

    private IEnumerator LoadSceneAndStartClient()
    {
        // Menampilkan status teks
        statusText.gameObject.SetActive(true);
        statusText.text = "Joining game...";

        // Memulai pemuatan scene secara asinkron
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameScene", LoadSceneMode.Single);

        // Menunggu sampai scene selesai dimuat
        while (!asyncLoad.isDone)
        {
            yield return null; // Menunggu frame berikutnya
        }

        // Memastikan scene yang dimuat adalah "GameScene"
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            NetworkManager.Singleton.StartClient();
        }
    }
}
