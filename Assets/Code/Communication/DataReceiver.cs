using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;

public abstract class DataReceiver<T> : MonoBehaviour {

    protected Dictionary<IPEndPoint, T> _ipToData = new Dictionary<IPEndPoint, T>();

    protected void Awake() {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
    }

    protected void FixedUpdate() {
        ImplementPack();
    }

    protected abstract void ReceivePack();
    // Implementation Guide
        //while (true) {
        //    foreach (IPEndPoint ip in /* Server IP / Clients IP List */) {
        //        _ipC = ip;
        //        MemoryStream = new MemoryStream(DataPacking.udpClient.Receive(ref _ipC));
        //        DataPacking.ipToData[ip] = (DataPack)DataPacking.binaryF.Deserialize(DataPacking.memoryStream);
        //    }
        //}

    protected abstract void ImplementPack();
}
