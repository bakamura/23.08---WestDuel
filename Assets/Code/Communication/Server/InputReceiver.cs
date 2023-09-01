using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class InputReceiver : DataReceiver<InputDataPack> {

    [Header("Cache")]

    private BinaryFormatter _bf = new BinaryFormatter();
    private MemoryStream _ms;
    private IPEndPoint _ipEpCache;

    protected override void ReceivePack() {
        while (true) {
            for(int i = 0; i < ServerConnectionHandler.players.Count; i++) {
                _ipEpCache = ServerConnectionHandler.players[i].ip;
                _ms = new MemoryStream(ServerConnectionHandler.udpClient.Receive(ref _ipEpCache));
                _ipToData[ServerConnectionHandler.players[i].ip] = (InputDataPack)_bf.Deserialize(_ms);
            }
        }
    }

    protected override void ImplementPack() {
        throw new System.NotImplementedException();
    }

}
