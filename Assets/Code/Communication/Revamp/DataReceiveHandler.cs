using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class DataReceiveHandler : MonoBehaviour {

    public Queue<InputDataPack> queueInputData;
    public Queue<WorldStateDataPack> queueWorldData;
    public Queue<GameStateDataPack> queueGameStateData;

    protected virtual void Awake() {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
    }

    private void ReceivePack() {
        while (true) {
            ConnectionHandler.memoryStreamCache = new MemoryStream(ConnectionHandler.udpClient.Receive(ref ConnectionHandler.ipEpCache));
            ConnectionHandler.byteArrayCache = (byte[])ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);

            switch ((DataPacksIdentification)ConnectionHandler.byteArrayCache[0]) {
                case DataPacksIdentification.InputDataPack:
                    ConnectionHandler.inputDataCache = (InputDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.inputDataCache.senderIp = ConnectionHandler.ipEpCache;
                    queueInputData.Enqueue(ConnectionHandler.inputDataCache);
                    break;
                case DataPacksIdentification.WorldStateDataPack:
                    ConnectionHandler.worldDataCache = (WorldStateDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.worldDataCache.senderIp = ConnectionHandler.ipEpCache;
                    queueWorldData.Enqueue(ConnectionHandler.worldDataCache);
                    break;
                case DataPacksIdentification.GamStateDataPack:
                    ConnectionHandler.gameStateDataCache = (GameStateDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.gameStateDataCache.senderIp = ConnectionHandler.ipEpCache;
                    queueGameStateData.Enqueue(ConnectionHandler.gameStateDataCache);
                    break;
                default:
                    Debug.LogError("Unindentified DataPack Type Received");
                    break;
            }
        }
    }

}
