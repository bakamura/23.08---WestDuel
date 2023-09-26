using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class InputReceiver : DataReceiver<InputDataPack>
{

    [Header("Cache")]

    private IPEndPoint _ipEpCache;
    private InputDataPack _dataPack;
    private UdpClient _udpClient;
    protected override void Awake()
    {
        _udpClient = new UdpClient(InputDataPack.Port);
        base.Awake();
    }

    protected override void ReceivePack()
    {
        while (true)
        {
            //_ipEpCache = new IPEndPoint(ServerConnectionHandler.players[i].ip, InputDataPack.Port);
            _memoryStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
            _dataPack = CheckDataPack<InputDataPack>(DataPacksIdentification.GamStateDataPack);
            if (_dataPack != null)
            {
                for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
                {
                    if (ServerConnectionHandler.players[i].ip == _ipEpCache.Address)
                    {
                        _ipToData[ServerConnectionHandler.players[i].ip] = _dataPack;
                        break;
                    }
                }
            }

        }
    }

    protected override void ImplementPack()
    {
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
        {
            if (_ipToData.ContainsKey(ServerConnectionHandler.players[i].ip) && _ipToData[ServerConnectionHandler.players[i].ip].updated)
            {
                Vector2 movmentDirection = PackingUtility.FloatArrayToVector2(_ipToData[ServerConnectionHandler.players[i].ip].movementInput);
                ServerConnectionHandler.players[i].movement.SetInputDirection(movmentDirection);
                ServerConnectionHandler.players[i].animationsUpdate.SetDirection(movmentDirection);
                ServerConnectionHandler.players[i].animationsUpdate.SetMousePosition(PackingUtility.FloatArrayToVector3(_ipToData[ServerConnectionHandler.players[i].ip].mousePoint));
                if (_ipToData[ServerConnectionHandler.players[i].ip].mouseClick)
                {
                    ServerConnectionHandler.players[i].shoot.Shoot(PackingUtility.FloatArrayToVector3(_ipToData[ServerConnectionHandler.players[i].ip].mousePoint));
                    ServerConnectionHandler.players[i].animationsUpdate.TriggerShootAnim();
                }
            }
        }
    }

    private void OnDestroy()
    {
        _udpClient.Close();
    }
}
