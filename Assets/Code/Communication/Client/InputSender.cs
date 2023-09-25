using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[RequireComponent(typeof(ClientInputReader))]
public class InputSender : DataSender<InputDataPack>
{
    private ClientInputReader _clientInputReader;
    private MemoryStream _memoryStream;
    private BinaryFormatter _binaryFormatter =  new BinaryFormatter();
    private UdpClient _udpClient;
    private IPEndPoint _endPoint;

    private void Awake()
    {
        _udpClient = new UdpClient(InputDataPack.Port);
        _endPoint = new IPEndPoint(ClientConnectionHandler.ServerEndPoint, InputDataPack.Port);
        _dataPackCache = new InputDataPack();
        _clientInputReader = GetComponent<ClientInputReader>();
    }

    protected override void FixedUpdate()
    {
        if (ClientConnectionHandler._hasGameEnded)
        {
            base.FixedUpdate();
        }
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
        _udpClient.Send(_byteArrayCache, _byteArrayCache.Length, _endPoint);
    }

    private void OnDestroy()
    {
        _udpClient.Close();
    }
}