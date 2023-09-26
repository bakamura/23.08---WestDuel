using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientGameStateSender : MonoBehaviour
{
    private GameStateDataPack _dataPackCache = new GameStateDataPack();    

    //needs to be called by the UI
    public void QuitGame()
    {
        PreparePack();
        DataSendHandler.SendPack(ConnectionHandler.DataPacksIdentification.GamStateDataPack, _dataPackCache, ConnectionHandler.ipEpCache);
    }

    private void PreparePack()
    {
        _dataPackCache.gameState = GameStateDataPack.GameState.Quit;
    }

    private void OnApplicationQuit()
    {
        QuitGame();
    }
}
