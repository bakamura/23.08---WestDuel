using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : Menu {

    [Header("Menus")]

    [SerializeField] private CanvasGroup _hud;
    [SerializeField] private CanvasGroup _menu;
    [SerializeField] private RectTransform _settings;
    [SerializeField] private Vector2 _settingsOutPos;
    [SerializeField] private Vector2 _settingsInPos;

    [Header("Health")]

    [SerializeField] private RectTransform[] _playerHealthImage;

    [Header("End Screen")]

    [SerializeField] private RectTransform _endScreen;
    [SerializeField] private Vector2 _endScreenOutPos;
    [SerializeField] private Vector2 _endScreenInPos;
    [SerializeField] private TextMeshProUGUI _winText;

    public void OpenHUD() {
        OpenUIFade(_hud);
    }

    public void OpenMenu() {
        OpenUIFade(_menu);
    }

    public void OpenSettings() {
        OpenUIMove(_settings, _settingsInPos, _settingsOutPos);
    }

    public void QuitMatch() {
        GameStateSenderClient gameStateSenderClient = FindObjectOfType<GameStateSenderClient>();
        if (gameStateSenderClient != null) gameStateSenderClient.QuitMatch();
        else FindObjectOfType<GameStateSender>().QuitMatch();
        SceneManager.LoadScene(0); // 
    }

    public void Rematch() {
        // stuff
    }

    public void UpdateHealth(int playerId, int health) {
        _playerHealthImage[playerId].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _playerHealthImage[playerId].rect.height * health);
    }

    public void ShowEndScreen(bool didWin) {
        _winText.text = didWin ? "YOU WIN" : "LOSER";
        OpenUIMove(_endScreen, _endScreenInPos, _endScreenOutPos);
    }

    public void HideEndScreen() {
        CloseUIMove(_endScreen, _endScreenInPos, _endScreenOutPos);
    }

}
