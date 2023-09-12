using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class InputReceiver : DataReceiver<InputDataPack>
{

    [Header("Cache")]

    private IPEndPoint _ipEpCache;
    private InputDataPack _dataPack;

    protected override void ReceivePack()
    {
        while (true)
        {
            for (int i = 0; i < ServerConnectionHandler.players.Count; i++)
            {
                _ipEpCache = ServerConnectionHandler.players[i].ip;
                _memoryStream = new MemoryStream(ServerConnectionHandler.udpClient.Receive(ref _ipEpCache));
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
                ServerConnectionHandler.players[i].movement.SetInputDirection(PackingUtility.FloatArrayToVector2(_ipToData[ServerConnectionHandler.players[i].ip]._movementInput));
                if (_ipToData[ServerConnectionHandler.players[i].ip]._mouseClick) ServerConnectionHandler.players[i].shoot.Shoot(PackingUtility.FloatArrayToVector3(_ipToData[ServerConnectionHandler.players[i].ip]._mousePoint));
            }
        }
    }

}
