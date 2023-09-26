using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;

public static class ConnectionHandler {

    public static UdpClient udpClient;

    public static BinaryFormatter binaryFormatter = new BinaryFormatter();
    public static MemoryStream memoryStreamCache;
    public static byte byteCache;
    public static byte[] byteArrayCache;

    public static IPEndPoint ipEpCache;
    public static InputDataPack inputDataCache;
    public static WorldStateDataPack worldDataCache;
    public static GameStateDataPack gameStateDataCache;

    static ConnectionHandler() {
        udpClient = new UdpClient(11000);
    }

    public enum DataPacksIdentification {
        InputDataPack,
        GamStateDataPack,
        WorldStateDataPack
    }

}
