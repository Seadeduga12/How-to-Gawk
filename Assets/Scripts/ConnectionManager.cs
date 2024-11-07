using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ConnectionManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public TextMeshProUGUI statusText;

    private void Start()
    {
        // Nonaktifkan status text saat pertama kali
        statusText.gameObject.SetActive(false);

        // Tambahkan listener untuk tombol
        hostButton.onClick.AddListener(StartHost);
        joinButton.onClick.AddListener(StartClient);
    }

    public void StartHost()
    {
        // Aktifkan status text dan set pesan menjadi "Hosting game..."
        statusText.gameObject.SetActive(true);
        statusText.text = "Hosting game...";

        // Mulai Host
        NetworkManager.Singleton.StartHost();

        // Pindahkan ke GameScene setelah berhasil
        LoadGameScene();
    }

    public void StartClient()
    {
        // Aktifkan status text dan set pesan menjadi "Joining game..."
        statusText.gameObject.SetActive(true);
        statusText.text = "Joining game...";

        // Mulai Client
        NetworkManager.Singleton.StartClient();

        // Pindahkan ke GameScene setelah berhasil
        LoadGameScene();
    }

    private void LoadGameScene()
    {
        // Pindah ke scene utama untuk gameplay setelah koneksi berhasil
        SceneManager.LoadScene("GameScene"); // Pastikan nama scene benar
    }
}
