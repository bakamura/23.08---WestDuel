using System.IO;
using System.Net;
using UnityEngine;

public class DataSendHandler : MonoBehaviour {

    public void SendPack(DataPacksIdentification dataType, object data, IPEndPoint ipReceiver) {
        ConnectionHandler.memoryStreamCache = new MemoryStream();
        ConnectionHandler.binaryFormatter.Serialize(ConnectionHandler.memoryStreamCache, data);
        ConnectionHandler.udpClient.Send(ConnectionHandler.byteArrayCache, ConnectionHandler.byteArrayCache.Length, ipReceiver);
    }

}
