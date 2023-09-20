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
                _dataPack = CheckDataPack<InputDataPack>(DataPacksIdentification.InputDataPack);
                if (_dataPack != null)
                {
                    _ipToData[ServerConnectionHandler.players[i].ip] = _dataPack;
                }
            }
            // sera q quando recebe um pacote do memory stream ele é descartado?
            //_memoryStream = new MemoryStream(ServerConnectionHandler.udpClient.Receive(ref _ipEpCache));
            //bool isInPlayerList = false;
            //foreach (PlayerInfo info in ServerConnectionHandler.players) if (info.ip == _ipEpCache)
            //    {
            //        isInPlayerList = true;
            //        break;
            //    }
            //if (isInPlayerList)
            //{
            //    switch(CheckDataPack(ref byte[] _byteArr))
            //    {
            //        case DataPacksIdentification.GamStateDataPack:
            //            // chama script especifico e altera o datapack correspondente

            //    }
            //}
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

}
