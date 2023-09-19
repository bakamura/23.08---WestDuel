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
    [SerializeField] private CanvasGroup _joinMenu;
    [SerializeField] private CanvasGroup _hostMenu;
    [SerializeField] private RectTransform _settingsMenu;
    [SerializeField] private Vector2 _settingsMenuActivePos;
    [SerializeField] private Vector2 _settingsMenuDeactivePos;

    [Header("Join Game")]

    [SerializeField] private TextMeshProUGUI _ipInput;

    [Header("Host Game")]

    private TextMeshProUGUI _ipSelf;
    private IPEndPoint _hostIp;
    private IPEndPoint _joinerIp;

    [Header("Cache")]

    private UdpClient _udpClient = new UdpClient(11000);
    private IPEndPoint _ipEpCache;
    private MemoryStream _mStream;
    private BinaryFormatter _bFormatter = new BinaryFormatter();
    private Thread _threadC;

    private const string START = "START";
    private const string START_SUCCESS = "START SUCCESS";
    private const string JOIN = "JOIN";
    private const string JOIN_SUCCESS = "JOIN SUCCESS";
    private const string LEAVE_LOBBY = "LEAVE";

    private void Awake() {
        _ipSelf.text = Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == AddressFamily.InterNetwork).ToString();
    }

    public void OpenMainMenu() {
        OpenUIFade(_mainMenu);
    }

    public void OpenJoinMenu() {
        if (_hostIp != null) {
            _bFormatter.Serialize(_mStream, LEAVE_LOBBY);
            _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _hostIp);
        }
        _threadC.Abort();
        _threadC = new Thread(Joining);
        OpenUIFade(_joinMenu);
    }

    public void OpenHostMenu() {
        _threadC.Abort();
        _threadC = new Thread(Hosting);
        OpenUIFade(_joinMenu);
    }

    public void OpenSettingsMenu() {
        OpenUIMove(_settingsMenu, _settingsMenuActivePos, _settingsMenuDeactivePos);
    }

    public void JoinGame() {
        _bFormatter.Serialize(_mStream, JOIN);
        _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, new IPEndPoint(IPAddress.Parse(_ipInput.text), 11000));
    }

    public void HostGame() {
        if (_joinerIp != null) {
            _bFormatter.Serialize(_mStream, JOIN);
            _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, new IPEndPoint(IPAddress.Parse(_ipInput.text), 11000));
        }
    }

    public void StartGame() {
        _bFormatter.Serialize(_mStream, START);
        _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _joinerIp);
    }

    public void CopySelfIp() {
        GUIUtility.systemCopyBuffer = _ipSelf.text;
    }

    private void Hosting() {
        if (_joinerIp == null) {
            _mStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
            string str = (string)_bFormatter.Deserialize(_mStream);
            if (str == JOIN) {
                _joinerIp = _ipEpCache;
                _bFormatter.Serialize(_mStream, JOIN_SUCCESS);
                _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, _joinerIp);
            } else if (str == START_SUCCESS) {
                ServerConnectionHandler.players[0].ip = _joinerIp; // Instantiate before
                SceneManager.LoadScene(1);
            } else if (str == LEAVE_LOBBY) {
                _joinerIp = null;
            }
        }
    }

    private void Joining() {
        if (_hostIp == null) {
            _mStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
            string str = (string)_bFormatter.Deserialize(_mStream);
            if (str == JOIN_SUCCESS) _hostIp = _ipEpCache;
            else if (str == START) {
                _bFormatter.Serialize(_mStream, START_SUCCESS);
                _udpClient.Send(_mStream.ToArray(), _mStream.ToArray().Length, new IPEndPoint(IPAddress.Parse(_ipInput.text), 11000));
                ClientConnectionHandler.ServerEndPoint = _hostIp;
                SceneManager.LoadScene(1);
            }
        }
    }

    ~MainMenu() {
        _udpClient.Close();
    }
}
