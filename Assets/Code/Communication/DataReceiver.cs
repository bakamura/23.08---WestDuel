using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
public abstract class DataReceiver<T> : MonoBehaviour
{

    protected Dictionary<IPEndPoint, T> _ipToData = new Dictionary<IPEndPoint, T>();
    protected MemoryStream _memoryStream;
    protected BinaryFormatter _binaryFormatter = new BinaryFormatter();

    protected void Awake()
    {
        Thread receiverThread = new Thread(ReceivePack);
        receiverThread.Start();
    }

    protected void FixedUpdate()
    {
        ImplementPack();
    }

    protected abstract void ReceivePack();

    protected abstract void ImplementPack();

    protected T CheckDataPack<T>(DataPacksIdentification dataPackTypeWanted)
    {
        byte[] bArray = (byte[])_binaryFormatter.Deserialize(_memoryStream);        
        if (bArray[0] == (byte)dataPackTypeWanted)
        {
            byte[] temp = new byte[bArray.Length - 1];
            Array.Copy(bArray, 1, temp, 0, temp.Length);
            _memoryStream = new MemoryStream(temp);
            return (T)_binaryFormatter.Deserialize(_memoryStream);
        }
        return default(T);
    }
}
