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
            for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
            {
                _ipEpCache = new IPEndPoint(ServerConnectionHandler.players[i].ip, InputDataPack.Port);
                _memoryStream = new MemoryStream(_udpClient.Receive(ref _ipEpCache));
                _dataPack = CheckDataPack<InputDataPack>(DataPacksIdentification.GamStateDataPack);
                if (_dataPack != null)
                {
                    _ipToData[ServerConnectionHandler.players[i].ip] = _dataPack;
                }
            }
        }
    }

    protected override void ImplementPack()
    {
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
        {
            if (_ipToData[ServerConnectionHandler.players[i].ip].updated)
            {
                Vector2 movmentDirection = PackingUtility.FloatArrayToVector2(_ipToData[ServerConnectionHandler.players[i].ip]._movementInput);
                ServerConnectionHandler.players[i].movement.SetInputDirection(movmentDirection);
                ServerConnectionHandler.players[i].animationsUpdate.SetDirection(movmentDirection);
                ServerConnectionHandler.players[i].animationsUpdate.SetMousePosition(PackingUtility.FloatArrayToVector3(_ipToData[ServerConnectionHandler.players[i].ip]._mousePoint));
                if (_ipToData[ServerConnectionHandler.players[i].ip]._mouseClick)
                {
                    ServerConnectionHandler.players[i].shoot.Shoot(PackingUtility.FloatArrayToVector3(_ipToData[ServerConnectionHandler.players[i].ip]._mousePoint));
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
