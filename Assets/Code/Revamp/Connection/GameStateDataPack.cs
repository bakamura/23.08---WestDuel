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

    public Dictionary<IPEndPoint, int> playerHealth;

    public GameStateDataPack(IPEndPoint[] playerIp) {
        gameState = GameState.Initiate;

        playerHealth = new Dictionary<IPEndPoint, int>();
        foreach (IPEndPoint ip in playerIp) playerHealth.Add(ip, new int());

        senderIp = null;
    }

    public IPEndPoint senderIp;
}
