using TMPro;
using UnityEngine;

public class MainMenu : Menu {

    [Header("Menus")]

    [SerializeField] private CanvasGroup _mainMenu;
    [SerializeField] private CanvasGroup _initialMenu;
    [SerializeField] private CanvasGroup _joinMenu;
    [SerializeField] private CanvasGroup _lobbyMenu;
    [SerializeField] private RectTransform _settingsMenu;
    [SerializeField] private Vector2 _settingsMenuActivePos;
    [SerializeField] private Vector2 _settingsMenuDeactivePos;

    [Header("Join Menu")]

    public TextMeshProUGUI ipInput;

    [Header("Lobby")]

    [SerializeField] private TextMeshProUGUI[] _playerIpText;

    [Header("Cache")]

    private LobbyConnectionHandler _connectionHandler;

    protected override void Awake() {
        _currentUi = _initialMenu;
    }

    private void Start() {
        _connectionHandler = FindObjectOfType<LobbyConnectionHandler>();
    }

    // Called by Button
    public void OpenInitialMenu() {
        OpenUIFade(_initialMenu);
    }

    // Called by Button and LobbyConnectionHandler
    public void OpenJoinMenu() {
        bool comingFromInitial = _initialMenu.interactable;
        _currentUi = comingFromInitial ? _initialMenu : _lobbyMenu;
        OpenUIFade(comingFromInitial ? _joinMenu : _mainMenu, _joinMenu);

        _connectionHandler.StartJoinMenu();
    }

    // Called by Button and LobbyConnectionHandler
    public void OpenLobbyMenu() {
        _currentUi = _mainMenu;
        OpenUIFade(_lobbyMenu);

        if (_connectionHandler.ipOther == null) _connectionHandler.StartHostMenu();
    }

    // Called by Button
    public void OpenSettingsMenu() {
        OpenUIMove(null, _settingsMenu, Vector2.zero, Vector2.zero, _settingsMenuActivePos, _settingsMenuDeactivePos);
    }

    // Called by Button
    public void CloseSetingsMenu() {
        OpenUIMove(_settingsMenu, null,_settingsMenuActivePos, _settingsMenuDeactivePos, Vector2.zero, Vector2.zero);
    }

    // Called by LobbyConnectionHandler
    public void SetIpText(int userId, string ipText) {
        _playerIpText[userId].text = ipText;
    }

    // Called by Button
    public void QuitGame() {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Needs Testing
#endif
    }

    private void OnApplicationQuit() {
        ConnectionHandler.udpClient.Close();
    }
}
