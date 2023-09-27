using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using UnityEngine;

public static class DataReceiveHandler {

    public static Queue<InputDataPack> queueInputData = new Queue<InputDataPack>();
    public static Queue<WorldStateDataPack> queueWorldData = new Queue<WorldStateDataPack>();
    public static Queue<GameStateDataPack> queueGameStateData = new Queue<GameStateDataPack>();
    public static Queue<StringDataPack> queueString = new Queue<StringDataPack>();

    public static void Start() {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
    }

    private static void ReceivePack() {
        while (true) {
            ConnectionHandler.memoryStreamCache = new MemoryStream(ConnectionHandler.udpClient.Receive(ref ConnectionHandler.ipEpCache));
            ConnectionHandler.byteArrayCache = getBytes(ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache));

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

    public static byte[] getBytes(object str) {
        int size = Marshal.SizeOf(str);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(str, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);

        return arr;
    }

    public static T fromBytes<T>(byte[] arr) {
        T str = default(T);

        int size = Marshal.SizeOf(str);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        str = (T)Marshal.PtrToStructure(ptr, str.GetType());
        Marshal.FreeHGlobal(ptr);

        return str;
    }
}
