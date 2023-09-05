using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameStateSender : DataSender<GameStateDataPack> {

    [Header("Check")]

    private bool _sendSignal;

    [Header("Cache")]

    private BinaryFormatter _bf;
    private MemoryStream _ms;

    protected override void FixedUpdate() {
        PreparePack();
        if (_sendSignal) {
            _sendSignal = false;
            SendPack();
        }
    }

    protected override void PreparePack() {
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++) {
            if (_dataPackCache.playersHealth[i] != ServerConnectionHandler.players[i].health.GetCurrentHealth()) {
                _sendSignal = true;
                _dataPackCache.playersHealth[i] = ServerConnectionHandler.players[i].health.GetCurrentHealth();
                _dataPackCache.gameState = _dataPackCache.playersHealth[i] > 0 ? GameStateDataPack.GameState.Continue : GameStateDataPack.GameState.Ended;
                if (_dataPackCache.gameState == GameStateDataPack.GameState.Ended) {
                    EndGame();
                    break; // Doesn't allow ties
                }
            }
        }
    }

    private void EndGame() {
        // Maybe should be in other script
        // Pause all world behaviours
    }

    public void RestartGame() {
        // Maybe should be in other script
        // Set playerHealth to Max
        // Unpause Game
    }

    protected override void SendPack() {
        _ms = new MemoryStream();
        _bf.Serialize(_ms, _dataPackCache);
        _byteArrayCache = AddIdentifierByte(_ms.ToArray(), (byte)DataPacksIdentification.GamStateDataPack);
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++) ServerConnectionHandler.udpClient.Send(_byteArrayCache, _byteArrayCache.Length, ServerConnectionHandler.players[i].ip);
    }

}
