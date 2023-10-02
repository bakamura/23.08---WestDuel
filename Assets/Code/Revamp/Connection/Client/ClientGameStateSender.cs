using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameStateSender : MonoBehaviour
{
    private GameStateDataPack _dataPackCache = new GameStateDataPack();    

    public void QuitGame()
    {
        _dataPackCache.gameState = GameStateDataPack.GameState.Quit;
        DataSendHandler.SendPack(_dataPackCache, (byte) ConnectionHandler.DataPacksIdentification.GameStateDataPack,ConnectionHandler.serverIpEp);

        SceneManager.LoadScene(0);
    }

    private void OnApplicationQuit()
    {
        QuitGame();
    }
}
