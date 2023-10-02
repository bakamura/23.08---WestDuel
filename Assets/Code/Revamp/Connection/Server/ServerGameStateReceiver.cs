using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerGameStateReceiver : MonoBehaviour {

    [Header("Cache")]

    private GameStateDataPack _dataPackCache;

    private void Update() {
        ImplementPack();
    }

    private void ImplementPack() {
        while (DataReceiveHandler.queueGameStateData.Count > 0) {
            _dataPackCache = DataReceiveHandler.queueGameStateData.Dequeue();

            if (ServerPlayerInfo.player.Keys.ToArray().Contains(PackingUtility.StringToIPEndPoint(_dataPackCache.ipEpString))) {
                switch (_dataPackCache.gameState) {
                    case GameStateDataPack.GameState.Quit:
                        // Reset Every Server Sided Class
                        SceneManager.LoadScene(0);
                        break;
                    default:
                        Debug.LogWarning("Receiving Unexpected GameState from Client");
                        break;
                }
            }
        }
    }

}
