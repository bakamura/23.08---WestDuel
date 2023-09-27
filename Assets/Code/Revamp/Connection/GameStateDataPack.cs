using System.Collections.Generic;
using System.Net;

[System.Serializable]
public struct GameStateDataPack {

    public byte identifierByte;

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
        identifierByte = 2;

        gameState = GameState.Initiate;

        playerHealth = new Dictionary<IPEndPoint, int>();
        foreach (IPEndPoint ip in playerIp) playerHealth.Add(ip, new int());

        ipEpString = null;
    }

    //public IPEndPoint senderIp;
    public string ipEpString; // I hate this
}
