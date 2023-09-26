using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClientInputReader))]
public class ClientInputSender : MonoBehaviour
{
    private ClientInputReader _inputReader;
    private InputDataPack _dataPackCache = new InputDataPack();

    private void Awake()
    {
        _inputReader = GetComponent<ClientInputReader>();
    }

    private void Update()
    {
        PreparePack();
        DataSendHandler.SendPack(ConnectionHandler.DataPacksIdentification.InputDataPack, _dataPackCache, ConnectionHandler.ipEpCache);
    }

    private void PreparePack()
    {
        _dataPackCache.shootTrigger = _inputReader.MouseClick;
        _dataPackCache.shootPoint = PackingUtility.Vector3ToFloatArray(_inputReader.MousePosition);
        _dataPackCache.movementInput = PackingUtility.Vector3ToFloatArray(_inputReader.CurrenMovment);
    }
}
