using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateReceiverServer : DataReceiver<GameStateDataPack>
{
    [Header("Cache")]

    private IPEndPoint _ipEpCache;
    private GameStateDataPack _dataPack;
    protected override void ImplementPack()
    {
        while (true)
        {
            for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
            {
                _ipEpCache = ServerConnectionHandler.players[i].ip;
                _memoryStream = new MemoryStream(ServerConnectionHandler.udpClient.Receive(ref _ipEpCache));
                _dataPack = CheckDataPack<GameStateDataPack>(DataPacksIdentification.GamStateDataPack);
                if (_dataPack != null)
                {
                    _ipToData[ServerConnectionHandler.players[i].ip] = _dataPack;
                }
            }
        }
    }

    protected override void ReceivePack()
    {
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
        {
            if (_ipToData[ServerConnectionHandler.players[i].ip].updated)
            {
                switch (_ipToData[ClientConnectionHandler.ServerEndPoint].gameState)
                {
                    case GameStateDataPack.GameState.Ended:
                        ServerConnectionHandler.players.Clear();
                        SceneManager.LoadScene("MainMenu");
                        for (int a = 0; a < ServerConnectionHandler.players.Count; a++)
                        {
                            _ipToData[ServerConnectionHandler.players[a].ip].updated = false;
                        }
                        return;
                    default:
                        break;
                }
                _ipToData[ServerConnectionHandler.players[i].ip].updated = false;
            }
        }
    }
}
