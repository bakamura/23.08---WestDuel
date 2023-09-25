using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateReceiverServer : DataReceiver<GameStateDataPack>
{
    [Header("Cache")]

    private IPEndPoint _ipEpCache;
    private GameStateDataPack _dataPack;
    private UdpClient _udpClient;

    protected override void Awake()
    {
        _udpClient = new UdpClient(GameStateDataPack.Port);
        base.Awake();
    }

    protected override void ImplementPack()
    {
        while (true)
        {
            //_ipEpCache = new IPEndPoint(ServerConnectionHandler.players[i].ip, GameStateDataPack.Port);
            _memoryStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
            _dataPack = CheckDataPack<GameStateDataPack>(DataPacksIdentification.GamStateDataPack);
            if (_dataPack != null)
            {
                for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
                {
                    if(ServerConnectionHandler.players[i].ip == _ipEpCache.Address)
                    {
                        _ipToData[ServerConnectionHandler.players[i].ip] = _dataPack;
                        break;
                    }
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
    private void OnDestroy()
    {
        _udpClient.Close();
    }
}