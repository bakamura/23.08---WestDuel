using System.Collections;
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

        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (threadToMain != null) {
            string[] ipText;
            switch (threadToMain) {
                case "OPEN LOBBY":
                    ipText = _ipOther.ToString().Split(':');
                    _ipOtherText.text = ipText[0];
                    OpenUIFade(_lobbyMenu);
                    break;
                case "LEAVE LOBBY":
                    OpenJoinMenu();
                    break;
                case "OTHER JOINED":
                    ipText = _ipOther.ToString().Split(':');
                    _ipOtherText.text = ipText[0];
                    break;
                case "START GAME HOST":
                    StartCoroutine(GoToGameScene(true));
                    break;
                case "START GAME JOINER":
                    StartCoroutine(GoToGameScene(false));
                    break;
            }
            threadToMain = null;
        }
    }

    public void OpenInitialMenu() {
        OpenUIFade(_initialMenu);
    }

    public void OpenJoinMenu() {
        bool comingFromInitial = _initialMenu.interactable;
        _currentUi = comingFromInitial ? _initialMenu : _lobbyMenu;
        if (_ipOther != null) {
            _mStream = new MemoryStream();
            _bFormatter.Serialize(_mStream, LEAVE_LOBBY);
            _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _ipOther);
        }
        if (_threadC != null) _threadC.Abort();
        _threadC = new Thread(Joining);
        _threadC.Start();
        OpenUIFade(comingFromInitial ? _joinMenu : _mainMenu, _joinMenu);
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

    public void CloseSetingsMenu() {
        CloseUIMove(_settingsMenu, _settingsMenuActivePos, _settingsMenuDeactivePos);
    }

    public void JoinGame() {
        _mStream = new MemoryStream();
        _bFormatter.Serialize(_mStream, JOIN);
        if (IPAddress.TryParse(_ipInput.text.Substring(0, _ipInput.text.Length - 1), out IPAddress ip)) {
            _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, new IPEndPoint(ip, 10000)); // Unsafely removes last CHAR (text mesh pro invisible char
        }
        else Debug.LogWarning("Invalid IP Adress!");
    }

    public void StartGame() {
        if (_ipOther == null) return;
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
                if (str == START_SUCCESS) threadToMain = "START GAME HOST";
                else if (str == LEAVE_LOBBY) _ipOther = null;
            }
        }
    }

    private void Joining() {
        while (true) {
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
                    _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _ipOther);

                    threadToMain = "START GAME JOINER";
                }
                else if (str == LEAVE_LOBBY) {
                    _ipOther = null;
                    threadToMain = "LEAVE LOBBY";
                }
            }
        }
    }

    private IEnumerator GoToGameScene(bool isHost) {
        if(!isHost) {
            ClientConnectionHandler.ServerEndPoint = _ipOther.Address;
        }
        SceneManager.LoadScene(isHost ? 1 : 2);

        yield return null;

        _threadC.Abort();
        _udpClient.Close();

        if (isHost) {
            ServerConnectionHandler.InstantiatePlayer(true, _ipSelf.Address);
            ServerConnectionHandler.InstantiatePlayer(false, _ipOther.Address);
        }

        Destroy(gameObject);
    }

    public void QuitGame() {
        Application.Quit();
        print("left");
    }

    ~MainMenu() {
        _udpClient.Close();
    }
}
