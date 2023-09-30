using System.Collections.Generic;
using System.Net;
using TMPro;
using UnityEngine;

public class Hud : Menu {

    [Header("Menus")]

    [SerializeField] private CanvasGroup _hud;
    [SerializeField] private CanvasGroup _menu;
    [SerializeField] private RectTransform _settings;
    [SerializeField] private Vector2 _settingsOutPos;
    [SerializeField] private Vector2 _settingsInPos;

    [Header("Health")]

    [SerializeField] private RectTransform[] _playerHealthImage;
    private Dictionary<IPEndPoint, RectTransform> _playerHealthImageFromIp = new Dictionary<IPEndPoint, RectTransform>();
    [SerializeField] private float _healthUnitWidth;

    [Header("End Screen")]

    [SerializeField] private RectTransform _endScreen;
    [SerializeField] private Vector2 _endScreenOutPos;
    [SerializeField] private Vector2 _endScreenInPos;
    [SerializeField] private TextMeshProUGUI _winText;

    private void Start() {
        foreach (IPEndPoint ip in ServerPlayerInfo.player.Keys) {
            if (ip == ConnectionHandler.serverIpEp) _playerHealthImageFromIp.Add(ip, _playerHealthImage[0]);
            else _playerHealthImageFromIp.Add(ip, _playerHealthImage[1]);
        }
    }

    public void UpdateHealth() {
        foreach (IPEndPoint ip in ServerPlayerInfo.player.Keys) 
            _playerHealthImageFromIp[ip].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthUnitWidth * ServerPlayerInfo.player[ip].health.GetCurrentHealth());
    }

    public void OpenHUD() {
        OpenUIFade(_hud);
    }

    public void OpenMenu() {
        OpenUIFade(_menu);
    }

    public void OpenSettings() {
        OpenUIMove(null, _settings, Vector2.zero, Vector2.zero, _settingsInPos, _settingsOutPos);
    }

    public void CloseSettings() {
        OpenUIMove(_settings, null, _settingsInPos, _settingsOutPos, Vector2.zero, Vector2.zero);
    }

    public void ShowEndScreen(bool didWin) {
        _winText.text = didWin ? "YOU WIN" : "LOSER";
        OpenUIMove(null, _endScreen, Vector2.zero, Vector2.zero, _endScreenInPos, _endScreenOutPos);
    }

    public void HideEndScreen() {
        OpenUIMove(_endScreen, null, _endScreenInPos, _endScreenOutPos, Vector2.zero, Vector2.zero);
    }

    public void QuitMatch() {
        ServerGameStateSender sender = FindObjectOfType<ServerGameStateSender>();
        if (sender != null) {
            sender.QuitMatch();

            return;
        }
        FindObjectOfType<ClientGameStateSender>().QuitGame();
    }

    public void Rematch() {
        // stuff
    }

}
