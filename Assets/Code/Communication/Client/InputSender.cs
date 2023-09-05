using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(ClientInputReader))]
public class InputSender : DataSender<InputDataPack>
{
    private ClientInputReader _clientInputReader;
    private MemoryStream _memoryStream;
    private BinaryFormatter _binaryFormatter =  new BinaryFormatter();

    private void Awake()
    {
        _dataPackCache = new InputDataPack();
        _clientInputReader = GetComponent<ClientInputReader>();
    }

    protected override void PreparePack()
    {
        _dataPackCache.updated = true;
        _dataPackCache._mouseClick = _clientInputReader.MouseClick;
        _dataPackCache._mousePoint = PackingUtility.Vector3ToFloatArray(_clientInputReader.MousePosition);
        _dataPackCache._movementInput = PackingUtility.Vector3ToFloatArray(_clientInputReader.CurrenMovment);
    }

    protected override void SendPack()
    {
        _memoryStream = new MemoryStream(); // Always open a new memory stream
        _binaryFormatter.Serialize(_memoryStream, _dataPackCache);
        _byteArrayCache = AddIdentifierByte(_memoryStream.ToArray(), (byte)DataPacksIdentification.InputDataPack);
        ClientConnectionHandler.UdpClient.Send(_byteArrayCache, _byteArrayCache.Length, ClientConnectionHandler.ServerEndPoint);
    }
}