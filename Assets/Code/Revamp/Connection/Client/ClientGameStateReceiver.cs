using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameStateReceiver : MonoBehaviour
{
    [Header("Cache")]
    [SerializeField] private Hud _hud;
    private GameStateDataPack _dataPackCache;

    private void FixedUpdate()
    {
        ProcessData();
    }
    private void ProcessData()
    {
        if (DataReceiveHandler.queueGameStateData.Count > 0)
        {
            _dataPackCache = DataReceiveHandler.queueGameStateData.Dequeue();            
            switch (_dataPackCache.gameState)
            {
                case GameStateDataPack.GameState.Initiate:
                    IPEndPoint[] playersIPs = new IPEndPoint[_dataPackCache.playerHealth.Keys.Count];
                    _dataPackCache.playerHealth.Keys.CopyTo(playersIPs, 0);
                    for (int i = 0; i < playersIPs.Length; i++)
                    {
                        if(i == 0)
                        {
                            ClientConnectionHandler.PlayersList.Add(playersIPs[i], new ClientConnectionHandler.MovableObjectData(Instantiate(InstantiateHandler.GetPlayer1ClientPrefab(),
                                transform.position, Quaternion.identity)));
                        }
                        else
                        {
                            ClientConnectionHandler.PlayersList.Add(playersIPs[i], new ClientConnectionHandler.MovableObjectData(Instantiate(InstantiateHandler.GetPlayer2ClientPrefab(),
                                transform.position, Quaternion.identity)));
                        }
                    }

                    _hud.UpdateHealth();
                    break;
                case GameStateDataPack.GameState.Restart:
                    //for (int i = 0; i < ClientConnectionHandler.PlayersList.Count; i++)
                    //{
                    //    _container.SpawnPlayer.GetPointFurthestFromOponent(i + 1 >= ClientConnectionHandler.PlayersList.Count ?
                    //        ClientConnectionHandler.PlayersList[0].Object.transform.position :
                    //        ClientConnectionHandler.PlayersList[i + 1].Object.transform.position);
                    //}
                    _hud.HideEndScreen(); // Implement When HUD Revamp done
                    ClientConnectionHandler._hasGameEnded = false;
                    _hud.UpdateHealth();
                    break;
                case GameStateDataPack.GameState.Continue:
                    _hud.UpdateHealth();
                    break;
                case GameStateDataPack.GameState.Ended:
                    _hud.ShowEndScreen(_dataPackCache.playerHealth[ConnectionHandler.ipEpCache] > 0); // Implement When HUD Revamp done USE IPEP TO GET
                    ClientConnectionHandler._hasGameEnded = true;
                    break;
                case GameStateDataPack.GameState.Quit:
                    ClientConnectionHandler.PlayersList.Clear();
                    ClientConnectionHandler._hasGameEnded = false;
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
        }
    }
}
