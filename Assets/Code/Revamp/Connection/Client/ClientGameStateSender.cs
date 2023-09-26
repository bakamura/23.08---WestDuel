using UnityEngine;

public class ClientGameStateSender : MonoBehaviour
{
    private GameStateDataPack _dataPackCache = new GameStateDataPack();    

    //needs to be called by the UI
    public void QuitGame()
    {
        _dataPackCache.gameState = GameStateDataPack.GameState.Quit;
        DataSendHandler.SendPack(ConnectionHandler.DataPacksIdentification.GameStateDataPack, _dataPackCache, ConnectionHandler.ipEpCache);
    }

    private void OnApplicationQuit()
    {
        QuitGame();
    }
}
