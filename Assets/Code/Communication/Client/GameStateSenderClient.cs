using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateSenderClient : DataSender<GameStateDataPack>
{
    [Header("Cache")]

    private BinaryFormatter _bf;
    private MemoryStream _ms;

    //protected override void FixedUpdate()
    //{
    //    PreparePack();
    //    if (_sendSignal)
    //    {
    //        _sendSignal = false;
    //        SendPack();
    //    }
    //}

    protected override void PreparePack()
    {
        _dataPackCache.gameState = GameStateDataPack.GameState.Ended;
        EndGame();
    }
    private void OnApplicationQuit()
    {
        PreparePack();
        SendPack();
    }

    private void EndGame()
    {
        ClientConnectionHandler.ServerEndPoint = null;
        ClientConnectionHandler.PlayersList.Clear();
        ClientConnectionHandler.BulletsList.Clear();
        SceneManager.LoadScene("MainMenu");

    }

    protected override void SendPack()
    {
        _ms = new MemoryStream();
        _bf.Serialize(_ms, _dataPackCache);
        _byteArrayCache = AddIdentifierByte(_ms.ToArray(), (byte)DataPacksIdentification.GamStateDataPack);
        ClientConnectionHandler.UdpClient.Send(_byteArrayCache, _byteArrayCache.Length, ClientConnectionHandler.ServerEndPoint);
    }
}
