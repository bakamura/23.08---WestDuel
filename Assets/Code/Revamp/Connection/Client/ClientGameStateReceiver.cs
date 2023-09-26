using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientGameStateReceiver : MonoBehaviour
{
    [SerializeField] private Hud _hud;
    private void FixedUpdate()
    {
        ProcessData();
    }
    private void ProcessData()
    {
        if (DataReceiveHandler.queueGameStateData.Count > 0)
        {
            GameStateDataPack temp = DataReceiveHandler.queueGameStateData.Dequeue();
            switch (temp.gameState)
            {
                case GameStateDataPack.GameState.Initiate:
                    IPEndPoint[] playersIPs = new IPEndPoint[temp.playerHealth.Keys.Count];
                    temp.playerHealth.Keys.CopyTo(playersIPs, 0);
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

                    UpdateHealthUI(temp);
                    break;
                case GameStateDataPack.GameState.Restart:
                    //for (int i = 0; i < ClientConnectionHandler.PlayersList.Count; i++)
                    //{
                    //    _container.SpawnPlayer.GetPointFurthestFromOponent(i + 1 >= ClientConnectionHandler.PlayersList.Count ?
                    //        ClientConnectionHandler.PlayersList[0].Object.transform.position :
                    //        ClientConnectionHandler.PlayersList[i + 1].Object.transform.position);
                    //}
                    _hud.HideEndScreen();
                    ClientConnectionHandler._hasGameEnded = false;
                    UpdateHealthUI(temp);
                    break;
                case GameStateDataPack.GameState.Continue:
                    UpdateHealthUI(temp);
                    break;
                case GameStateDataPack.GameState.Ended:
                    _hud.ShowEndScreen(_dataPack.playersHealth[1] > 0);
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
    private void UpdateHealthUI(GameStateDataPack dataPack)
    {
        for (int i = 0; i < dataPack.playerHealth.Count; i++)
        {
            _hud.UpdateHealth(i, dataPack.playersHealth[i]);
        }
    }
}
