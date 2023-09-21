using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : Menu {

    [Header("Menus")]

    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private CanvasGroup _initialMenu;
    [SerializeField] private CanvasGroup _joinMenu;
    [SerializeField] private CanvasGroup _lobbyMenu;
    [SerializeField] private RectTransform _settingsMenu;
    [SerializeField] private Vector2 _settingsMenuActivePos;
    [SerializeField] private Vector2 _settingsMenuDeactivePos;

    [Header("Join Game")]

    [SerializeField] private TextMeshProUGUI _ipInput;

    [Header("IP")]

    [SerializeField] private TextMeshProUGUI _ipSelfText;
    [SerializeField] private TextMeshProUGUI _ipOtherText;
    private IPEndPoint _ipSelf;
    private IPEndPoint _ipOther;

    [Header("Cache")]

    private UdpClient _udpClient;
    private IPEndPoint _ipEpCache;
    private MemoryStream _mStream;
    private BinaryFormatter _bFormatter;
    private Thread _threadC;

    private const string START = "START";
    private const string START_SUCCESS = "START SUCCESS";
    private const string JOIN = "JOIN";
    private const string JOIN_SUCCESS = "JOIN SUCCESS";
    private const string LEAVE_LOBBY = "LEAVE";

    private string threadToMain;

    private void Awake() {
        _currentUi = _initialMenu;

        _udpClient = new UdpClient(10000);
        _bFormatter = new BinaryFormatter();

        _ipSelf = new IPEndPoint(Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork), 10000);
        foreach (IPAddress adress in Dns.GetHostEntry(Dns.GetHostName()).AddressList) if (adress.ToString().Split('.')[0] == "172" && adress.ToString().Split('.')[1] == "17") _ipSelf = new IPEndPoint(adress, 10000);

        string[] ipText = _ipSelf.ToString().Split(':');
        _ipSelfText.text = ipText[0];

    }

    private void Update() {
        if (threadToMain != null) {
            string[] ipText = _ipOther.ToString().Split(':');
            switch (threadToMain) {
                case "OPEN LOBBY":
                    _ipOtherText.text = ipText[0];
                    OpenUIFade(_lobbyMenu);
                    break;
                case "LEAVE LOBBY":
                    OpenJoinMenu();
                    break;
                case "OTHER JOINED":
                    _ipOtherText.text = ipText[0];
                    break;
            }
            threadToMain = null;
        }
    }

    public void OpenInitialMenu() {
        OpenUIFade(_initialMenu);
    }

    public void OpenJoinMenu() {
        _currentUi = _initialMenu.interactable ? _initialMenu : _mainMenu;
        if (_ipOther != null) {
            _mStream = new MemoryStream();
            _bFormatter.Serialize(_mStream, LEAVE_LOBBY);
            _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _ipOther);
        }
        if (_threadC != null) _threadC.Abort();
        _threadC = new Thread(Joining);
        _threadC.Start();
        OpenUIFade(_joinMenu);
    }

    public void OpenMainMenu() {
        OpenUIFade(_mainMenu);
        _currentUi = _joinMenu;
    }

    public void OpenHostMenu() {
        if (_threadC != null) _threadC.Abort();
        _threadC = new Thread(Hosting);
        _threadC.Start();
        _currentUi = _mainMenu;
        OpenUIFade(_lobbyMenu);
    }

    public void OpenSettingsMenu() {
        OpenUIMove(_settingsMenu, _settingsMenuActivePos, _settingsMenuDeactivePos);
    }

    public void JoinGame() {
        _mStream = new MemoryStream();
        _bFormatter.Serialize(_mStream, JOIN);
        _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, new IPEndPoint(IPAddress.Parse(_ipInput.text.Substring(0, _ipInput.text.Length - 1)), 10000)); // Unsafely removes last CHAR (text mesh pro invisible char
    }

    public void StartGame() {
        _mStream = new MemoryStream();
        _bFormatter.Serialize(_mStream, START);
        _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _ipOther);
    }

    public void CopySelfIp() {
        string[] ipText = _ipSelf.ToString().Split(':');
        GUIUtility.systemCopyBuffer = ipText[0];
    }

    private void Hosting() {
        while (true) {
            Debug.Log("Hosting Thread");
            _mStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
            string str = (string)_bFormatter.Deserialize(_mStream);
            if (_ipOther == null) {
                if (str == JOIN) {
                    _ipOther = _ipEpCache;
                    _mStream = new MemoryStream();
                    _bFormatter.Serialize(_mStream, JOIN_SUCCESS);
                    _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _ipOther);
                    threadToMain = "OTHER JOINED";
                }
            }
            else {
                if (str == START_SUCCESS) {
                    ServerConnectionHandler.players[0].ip = _ipOther; // Instantiate before
                    //ServerConnectionHandler.players[0].ip = _ipOther.Address;
                    SceneManager.LoadScene(1);
                }
                else if (str == LEAVE_LOBBY) {
                    _ipOther = null;
                }
            }
        }
    }

    private void Joining() {
        while (true) {
            Debug.Log("Joining Thread");
            _mStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
            string str = (string)_bFormatter.Deserialize(_mStream);
            if (_ipOther == null) {
                if (str == JOIN_SUCCESS) {
                    _currentUi = _mainMenu;

                    _ipOther = _ipEpCache; ;
                    threadToMain = "OPEN LOBBY";
                }
            }
            else {
                if (str == START) {
                    _mStream = new MemoryStream();
                    _bFormatter.Serialize(_mStream, START_SUCCESS);
                    _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, new IPEndPoint(IPAddress.Parse(_ipInput.text), 10000));
                    ClientConnectionHandler.ServerEndPoint = _ipOther;
                    //ClientConnectionHandler.ServerEndPoint = _ipOther.Address;
                    SceneManager.LoadScene(1);
                }
                else if (str == LEAVE_LOBBY) {
                    _ipOther = null;
                    threadToMain = "LEAVE LOBBY";
                }
            }
        }
    }

    public void QuitGame() {
        Application.Quit();
        print("left");
    }

    ~MainMenu() {
        _udpClient.Close();
    }
}
