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
    private static Thread threadC;

    public static void Start() {
        if (threadC == null) {
            threadC = new Thread(ReceivePack);
            threadC.Start();
        }
    }

    private static void ReceivePack() {
        while (true) {
            ConnectionHandler.memoryStreamCache = new MemoryStream(ConnectionHandler.udpClient.Receive(ref ConnectionHandler.ipEpCache));
            Debug.Log(ConnectionHandler.ipEpCache);
            ConnectionHandler.byteArrayCache = GetBytes(ConnectionHandler.binaryFormatter.Deserialize(ConnectionHandler.memoryStreamCache));

            switch ((ConnectionHandler.DataPacksIdentification)ConnectionHandler.byteArrayCache[0]) {
                case ConnectionHandler.DataPacksIdentification.InputDataPack:
                    ConnectionHandler.inputDataCache = FromBytes<InputDataPack>(ConnectionHandler.byteArrayCache);
                    ConnectionHandler.inputDataCache.ipEpString = ConnectionHandler.ipEpCache.ToString();
                    queueInputData.Enqueue(FromBytes<InputDataPack>(ConnectionHandler.byteArrayCache));
                    break;
                case ConnectionHandler.DataPacksIdentification.WorldStateDataPack:
                    ConnectionHandler.worldDataCache = FromBytes<WorldStateDataPack>(ConnectionHandler.byteArrayCache);
                    ConnectionHandler.worldDataCache.ipEpString = ConnectionHandler.ipEpCache.ToString();
                    queueWorldData.Enqueue(FromBytes<WorldStateDataPack>(ConnectionHandler.byteArrayCache));
                    break;
                case ConnectionHandler.DataPacksIdentification.GameStateDataPack:
                    ConnectionHandler.gameStateDataCache = FromBytes<GameStateDataPack>(ConnectionHandler.byteArrayCache);
                    ConnectionHandler.gameStateDataCache.ipEpString = ConnectionHandler.ipEpCache.ToString();
                    queueGameStateData.Enqueue(FromBytes<GameStateDataPack>(ConnectionHandler.byteArrayCache));
                    break;
                case ConnectionHandler.DataPacksIdentification.String:
                    queueString.Enqueue(FromBytes<StringDataPack>(ConnectionHandler.byteArrayCache));
                    break;
                default:
                    Debug.LogError("Unindentified DataPack Type Received");
                    break;
            }
        }
    }

    private static byte[] GetBytes(object obj) {
        int size = Marshal.SizeOf(obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);

        return arr;
    }

    private static T FromBytes<T>(byte[] arr) {
        T structT = default(T);

        int size = Marshal.SizeOf(structT);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(arr, 0, ptr, size);

        structT = (T)Marshal.PtrToStructure(ptr, structT.GetType());
        Marshal.FreeHGlobal(ptr);

        return structT;
    }

}
