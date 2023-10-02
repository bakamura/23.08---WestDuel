using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public static class DataReceiveHandler {

    public static Queue<InputDataPack> queueInputData = new Queue<InputDataPack>();
    public static Queue<WorldStateDataPack> queueWorldData = new Queue<WorldStateDataPack>();
    public static Queue<GameStateDataPack> queueGameStateData = new Queue<GameStateDataPack>();
    public static Queue<StringDataPack> queueString = new Queue<StringDataPack>();

    private static Thread threadC;
    private static byte _identByteC;
    private static byte[] _byteArrayC;

    public static void Start() {
        if (threadC == null) {
            threadC = new Thread(ReceivePack);
            threadC.Start();
        }
    }

    private static void ReceivePack() {
        while (true) {
            ConnectionHandler.memoryStreamCache = new MemoryStream(ConnectionHandler.udpClient.Receive(ref ConnectionHandler.ipEpCache));
            ConnectionHandler.byteArrayCache = ConnectionHandler.memoryStreamCache.ToArray();
            _identByteC = ConnectionHandler.byteArrayCache[0];
            ConnectionHandler.memoryStreamCache = new MemoryStream(RemoveIdentByte(ConnectionHandler.byteArrayCache));

            switch ((ConnectionHandler.DataPacksIdentification)_identByteC) {
                case ConnectionHandler.DataPacksIdentification.InputDataPack:
                    ConnectionHandler.inputDataCache = (InputDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.inputDataCache.ipEpString = ConnectionHandler.ipEpCache.ToString();
                    queueInputData.Enqueue(ConnectionHandler.inputDataCache);
                    break;
                case ConnectionHandler.DataPacksIdentification.WorldStateDataPack:
                    ConnectionHandler.worldDataCache = (WorldStateDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.worldDataCache.ipEpString = ConnectionHandler.ipEpCache.ToString();
                    queueWorldData.Enqueue(ConnectionHandler.worldDataCache);
                    break;
                case ConnectionHandler.DataPacksIdentification.GameStateDataPack:
                    ConnectionHandler.gameStateDataCache = (GameStateDataPack)ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache);
                    ConnectionHandler.gameStateDataCache.ipEpString = ConnectionHandler.ipEpCache.ToString();
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

    private static byte[] RemoveIdentByte(byte[] byteArr) {
        _byteArrayC = new byte[byteArr.Length - 1];
        Array.Copy(byteArr, 1, _byteArrayC, 0, _byteArrayC.Length);
        return _byteArrayC;
    }

}
