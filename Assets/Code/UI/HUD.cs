using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : Menu {

    public void QuitMatch() {
        // Quit the match and return to main menu (gives oponent the victory)
        SceneManager.LoadScene(0); // 
    }

}
