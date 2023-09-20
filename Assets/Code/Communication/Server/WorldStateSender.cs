using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class WorldStateSender : DataSender<WorldStateDataPack> {

    [Header("Cache")]

    private BinaryFormatter _bf;
    private MemoryStream _ms;

    protected override void PreparePack() {
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++) {
            _dataPackCache.playersPos[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position);
            _dataPackCache.playersVelocity[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].rigidBody.velocity);
            _dataPackCache.playersHasBullet[i] = ServerConnectionHandler.players[i].shoot.CheckBullet();
            _dataPackCache.bulletsPos[2*i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position); // Calc Inside [] because each player has 2 bullets
            _dataPackCache.bulletsPos[2*i + 1] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position);
            _dataPackCache.bulletsVelocity[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position); // Change later into account of bullet per player
            //_dataPackCache.playersMousePosition[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i]);
        }
    }

    protected override void SendPack() {
        _ms = new MemoryStream();
        _bf.Serialize(_ms, _dataPackCache);
        _byteArrayCache = AddIdentifierByte(_ms.ToArray(), (byte)DataPacksIdentification.WorldStateDataPack);
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++) ServerConnectionHandler.udpClient.Send(_byteArrayCache, _byteArrayCache.Length, ServerConnectionHandler.players[i].ip);
    }

}
