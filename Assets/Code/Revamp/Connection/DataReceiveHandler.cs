using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class DataReceiveHandler {

    public static Queue<InputDataPack> queueInputData;
    public static Queue<WorldStateDataPack> queueWorldData;
    public static Queue<GameStateDataPack> queueGameStateData;
    public static Queue<StringDataPack> queueString;

    public static void Start() {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
    }

    private static void ReceivePack() {
        while (true) {
            ConnectionHandler.memoryStreamCache = new MemoryStream(ConnectionHandler.udpClient.Receive(ref ConnectionHandler.ipEpCache));
            ConnectionHandler.byteArrayCache = (byte[])ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);

            switch ((ConnectionHandler.DataPacksIdentification)ConnectionHandler.byteArrayCache[0]) {
                case ConnectionHandler.DataPacksIdentification.InputDataPack:
                    ConnectionHandler.inputDataCache = (InputDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.inputDataCache.senderIp = ConnectionHandler.ipEpCache;
                    queueInputData.Enqueue(ConnectionHandler.inputDataCache);
                    break;
                case ConnectionHandler.DataPacksIdentification.WorldStateDataPack:
                    ConnectionHandler.worldDataCache = (WorldStateDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.worldDataCache.senderIp = ConnectionHandler.ipEpCache;
                    queueWorldData.Enqueue(ConnectionHandler.worldDataCache);
                    break;
                case ConnectionHandler.DataPacksIdentification.GameStateDataPack:
                    ConnectionHandler.gameStateDataCache = (GameStateDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.gameStateDataCache.senderIp = ConnectionHandler.ipEpCache;
                    queueGameStateData.Enqueue(ConnectionHandler.gameStateDataCache);
                    break;
                case ConnectionHandler.DataPacksIdentification.String:
                    queueString.Enqueue((StringDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache));
                    break;
                default:
                    Debug.LogError("Unindentified DataPack Type Received");
                    break;
            }
        }
    }

}
