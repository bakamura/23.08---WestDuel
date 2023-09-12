using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameStateDataPack {
    public bool updated = true;
    public enum GameState {
        Initiate,
        Restart,
        Continue,
        Ended
    }
    public GameState gameState = GameState.Initiate;

    public List<int> playersHealth = new List<int>();

}
