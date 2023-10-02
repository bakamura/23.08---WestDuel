using System;
using System.IO;
using System.Net;

public static class DataSendHandler {

    private static byte[] _byteArrayC;

    public static void SendPack(object data, byte identByte, IPEndPoint ipReceiver) {
        ConnectionHandler.memoryStreamCache = new MemoryStream();
        ConnectionHandler.binaryFormatter.Serialize(ConnectionHandler.memoryStreamCache, data);
        ConnectionHandler.byteArrayCache = AddIdentByte(ConnectionHandler.memoryStreamCache.ToArray(), identByte);
        ConnectionHandler.udpClient.Send(ConnectionHandler.byteArrayCache, ConnectionHandler.byteArrayCache.Length, ipReceiver);
    }
    private static byte[] AddIdentByte(byte[] bArray, byte addedB) {
        _byteArrayC = new byte[bArray.Length + 1];
        _byteArrayC[0] = addedB;
        Array.Copy(bArray, 0, _byteArrayC, 1, bArray.Length);
        return _byteArrayC;
    }

}
