using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : Menu {

    private RectTransform[] _playerHealthImage;

    public void QuitMatch() {
        // Quit the match and return to main menu (gives oponent the victory)
        SceneManager.LoadScene(0); // 
    }

    public void UpdateHealth(int playerId, int health) {
        _playerHealthImage[playerId].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _playerHealthImage[playerId].rect.height * health);
    }

}
