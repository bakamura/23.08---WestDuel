using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class LobbyConnectionHandler : MonoBehaviour {

    [Header("IP")]

    private IPEndPoint _ipSelf;
    [HideInInspector] public IPEndPoint ipOther; // Doesn't consider 3+ players
    private bool _isHost = false;

    [Header("Cache")]

    private MainMenu _menu;
    private StringDataPack _dataPackCache;

    private const string START = "START"; // Server Send
    private const string START_SUCCESS = "START SUCCESS"; // Client Send
    private const string JOIN = "JOIN"; // Client Send
    private const string JOIN_SUCCESS = "JOIN SUCCESS"; // Server Send
    private const string LEAVE_LOBBY = "LEAVE"; // Both Send

    private void Start() {
        IPAddress adressSelf = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork);
        foreach (IPAddress adress in Dns.GetHostEntry(Dns.GetHostName()).AddressList) // Works only for PUC PC, shouldn't be needed elsewhere
            if (adress.ToString().Split('.')[0] == "172" && adress.ToString().Split('.')[1] == "17") adressSelf = adress;

        _menu = FindObjectOfType<MainMenu>();
        _menu.SetIpText(0, adressSelf.ToString());
        _ipSelf = new IPEndPoint(adressSelf, 11000);

        DataReceiveHandler.Start();

        DontDestroyOnLoad(gameObject); // USE AN EMPTY GO WITH ONLY THIS SCRIPT ATTACHED
    }

    private void Update() {
        ImplementPack();
    }

    private void ImplementPack() {
        while (DataReceiveHandler.queueString.Count > 0) {
            _dataPackCache = DataReceiveHandler.queueString.Dequeue();
            if (_isHost) {
                if (ipOther == null) {
                    if (_dataPackCache.stringSent == JOIN) {
                        ipOther = _dataPackCache.senderIp;
                        _menu.SetIpText(1, ipOther.ToString().Split(':')[0]);
                    }
                }
                else {
                    if (_dataPackCache.stringSent == START_SUCCESS) GoToGameScene();
                    else if (_dataPackCache.stringSent == LEAVE_LOBBY) {
                        ipOther = null;
                        _menu.SetIpText(1, "");
                    }
                }
            }
            else {
                if (ipOther == null) {
                    if (_dataPackCache.stringSent == JOIN_SUCCESS) {
                        ipOther = _dataPackCache.senderIp;
                        _menu.SetIpText(1, ipOther.ToString().Split(':')[0]);
                        _menu.OpenLobbyMenu();
                    }
                }
                else {
                    if (_dataPackCache.stringSent == START) {
                        DataSendHandler.SendPack(START_SUCCESS, ipOther);
                        GoToGameScene();
                    }
                    else if (_dataPackCache.stringSent == LEAVE_LOBBY) {
                        ipOther = null;
                        _menu.SetIpText(1, "");
                        _menu.OpenJoinMenu();
                    }
                }
            }

        }
    }

    // Called by MainMenu
    public void StartJoinMenu() {
        _isHost = false;
        if (ipOther != null) DataSendHandler.SendPack(LEAVE_LOBBY, ipOther);
    }

    // Called by Button
    public void JoinGame() {
        if (IPAddress.TryParse(_menu.ipInput.text.Substring(0, _menu.ipInput.text.Length - 1), out IPAddress ip)) {
            DataSendHandler.SendPack(JOIN, new IPEndPoint(ip, 11000)); // Unsafely removes last CHAR (text mesh pro invisible char)
        }
        else Debug.Log("Invalid IP Adress!");
    }

    // Called by MainMenu
    public void StartHostMenu() {
        _isHost = true;
    }

    // Called by Button
    public void CopySelfIp() {
        string[] ipText = _ipSelf.ToString().Split(':');
        GUIUtility.systemCopyBuffer = ipText[0];
    }

    // Called by Button
    public void StartGame() {
        if (ipOther != null) {
            DataSendHandler.SendPack(START, ipOther);
        }
    }

    private void GoToGameScene() {
        ConnectionHandler.serverIpEp = _isHost ? _ipSelf : ipOther;

        SceneManager.LoadScene(_isHost ? 1 : 2);

        if (_isHost) {
            ServerPlayerInfo.InstantiatePlayer(true, _ipSelf);
            ServerPlayerInfo.InstantiatePlayer(false, ipOther);
        }

        Destroy(gameObject);
    }

}
