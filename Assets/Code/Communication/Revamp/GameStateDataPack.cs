using System.Collections.Generic;
using System.Net;

[System.Serializable]
public struct GameStateDataPack {
    public enum GameState {
        Initiate,
        Restart,
        Continue,
        Ended,
        Quit
    }
    public GameState gameState;

    public List<int> playersHealth;

    public GameStateDataPack(byte a = 0) {
        gameState = GameState.Initiate;

        playersHealth = new List<int>() { new int(), new int() };

        senderIp = null;
        updated = true;
    }

    public IPEndPoint senderIp;
    public bool updated;
}
