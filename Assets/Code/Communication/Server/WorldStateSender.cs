using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Net;

public class WorldStateSender : DataSender<WorldStateDataPack> {

    [Header("Cache")]

    private BinaryFormatter _bf;
    private MemoryStream _ms;
    private UdpClient _udpClient = new UdpClient(WorldStateDataPack.Port);

    protected override void PreparePack() {
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++) {
            _dataPackCache.playersPos[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position);
            _dataPackCache.playersVelocity[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].rigidBody.velocity);
            _dataPackCache.playersHasBullet[i] = ServerConnectionHandler.players[i].shoot.CheckBullet();
            _dataPackCache.bulletsPos[2*i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position); // Calc Inside [] because each player has 2 bullets
            _dataPackCache.bulletsPos[2*i + 1] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position);
            _dataPackCache.bulletsVelocity[i] = PackingUtility.Vector3ToFloatArray(ServerConnectionHandler.players[i].health.transform.position); // Change later into account of bullet per player
        }
    }

    protected override void SendPack() {
        _ms = new MemoryStream();
        _bf.Serialize(_ms, _dataPackCache);
        _byteArrayCache = AddIdentifierByte(_ms.ToArray(), (byte)DataPacksIdentification.WorldStateDataPack);
        for (int i = 0; i < ServerConnectionHandler.players.Count; i++) _udpClient.Send(_byteArrayCache, _byteArrayCache.Length, new IPEndPoint(ServerConnectionHandler.players[i].ip, WorldStateDataPack.Port));
    }

}
