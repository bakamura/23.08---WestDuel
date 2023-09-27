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

    protected override void Awake() {
        _currentUi = _initialMenu;
    }

    public void OpenInitialMenu() {
        OpenUIFade(_initialMenu);
    }

    public void OpenJoinMenu() {
        bool comingFromInitial = _initialMenu.interactable;
        _currentUi = comingFromInitial ? _initialMenu : _lobbyMenu;
        OpenUIFade(comingFromInitial ? _joinMenu : _mainMenu, _joinMenu);
    }

    public void OpenMainMenu() {
        OpenUIFade(_mainMenu);
        _currentUi = _joinMenu;
    }

    public void OpenLobbyMenu() {
        _currentUi = _mainMenu;
        OpenUIFade(_lobbyMenu);
    }

    public void OpenSettingsMenu() {
        OpenUIMove(null, _settingsMenu, Vector2.zero, Vector2.zero, _settingsMenuActivePos, _settingsMenuDeactivePos);
    }

    public void CloseSetingsMenu() {
        OpenUIMove(_settingsMenu, null,_settingsMenuActivePos, _settingsMenuDeactivePos, Vector2.zero, Vector2.zero);
    }

    public void SetIpText(int userId, string ipText) {
        _playerIpText[userId].text = ipText;
    }

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
