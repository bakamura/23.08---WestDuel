using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameStateReceiver : DataReceiver<GameStateDataPack>
{
    [SerializeField] private NetworkReferenceContainer _container;
    private MemoryStream _memoryStream;
    private BinaryFormatter _binaryFormatter = new BinaryFormatter();

    protected override void ReceivePack()
    {
        while (true)
        {
            IPEndPoint temp = ClientConnectionHandler.ServerEndPoint;
            _memoryStream = new MemoryStream(ClientConnectionHandler.UdpClient.Receive(ref temp));
            _ipToData[temp] = (GameStateDataPack)_binaryFormatter.Deserialize(_memoryStream);
        }
    }

    protected override void ImplementPack()
    {
        if (_ipToData[ClientConnectionHandler.ServerEndPoint].updated)
        {
            switch (_ipToData[ClientConnectionHandler.ServerEndPoint].gameState)
            {
                case GameStateDataPack.GameState.Continue:
                    break;
                case GameStateDataPack.GameState.Ended:
                    break;
                case GameStateDataPack.GameState.Initiate:
                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(Instantiate(InstantiateHandler.GetPlayerPrefab(),
                        _container.SpawnPlayer.GetPointFurthestFromOponent(_container.LocalPlayer.transform.position), Quaternion.identity)));

                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(_container.LocalPlayer));
                    break;
                case GameStateDataPack.GameState.Restart:
                    break;
            }
            _ipToData[ClientConnectionHandler.ServerEndPoint].updated = false;
        }
    }
}
