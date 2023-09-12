using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DataSender<T> : MonoBehaviour
{

    [Header("Cache")]

    protected T _dataPackCache;
    protected byte[] _byteArrayCache;

    protected virtual void FixedUpdate()
    {
        PreparePack();
        SendPack();
    }

    protected abstract void PreparePack();

    protected abstract void SendPack();

    protected byte[] AddIdentifierByte(byte[] bArray, byte addedB)
    {
        byte[] newBArray = new byte[bArray.Length + 1];
        newBArray[0] = addedB;
        Array.Copy(bArray, 0, newBArray, 1, bArray.Length);
        return newBArray;
    }
}
