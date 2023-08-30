using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataSender<T> : MonoBehaviour {

    [Header("Cache")]

    protected T _dataPackCache;
    protected byte[] _byteArrayCache;

    protected virtual void FixedUpdate() {
        PreparePack();
        SendPack();
    }

    protected abstract void PreparePack();

    protected abstract void SendPack();
    // Implementation guide
        //MemoryStream = new MemoryStream(); // Always open a new memory stream
        //BinaryFormatter.Serialize(DataPacking.memoryStream, _dataPack); 
        //_byteArrayCache = MemoryStream.ToArray();
        //foreach (IPEndPoint ip in /* Server IP / Clients IP List */) UdpClient.Send(_byteArrayCache, _byteArrayCache.Length, ip);

}
