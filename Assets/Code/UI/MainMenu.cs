using System.Linq;
using System.Net;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu {

    [Header("Join Game")]

    [SerializeField] private TextMeshProUGUI _ipInput;

    [Header("Host Game")]

    private TextMeshProUGUI _ipSelf;
    private IPEndPoint _joinerIp;

    private void Awake() {
        _ipSelf.text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
        Thread thread0 = new Thread(Hosting);
    }

    public void JoinGame() {
        // Send signal to host
    }

    public void HostGame() {
        // Start receiving joins
    }

    private void Hosting() {
        // If Hosting && someone sends a join signal and _joinerIp = null, confirm to the sender and _joinerIP = value
    }

    public void StartGame() {
        if (_joinerIp != null) {
            // Send start game signal
            // Start Game
            SceneManager.LoadScene(1);
        }
    }

}
