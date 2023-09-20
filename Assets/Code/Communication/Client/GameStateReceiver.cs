using System.IO;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateReceiver : DataReceiver<GameStateDataPack>
{
    [SerializeField] private NetworkReferenceContainer _container;
    private GameStateDataPack _dataPack;

    protected override void ReceivePack()
    {
        while (true)
        {
            IPEndPoint temp = ClientConnectionHandler.ServerEndPoint;
            _memoryStream = new MemoryStream(ClientConnectionHandler.UdpClient.Receive(ref temp));
            if (temp == ClientConnectionHandler.ServerEndPoint)
            {
                _dataPack = CheckDataPack<GameStateDataPack>(DataPacksIdentification.GamStateDataPack);
                if (_dataPack != null)
                {
                    _ipToData[temp] = _dataPack;
                }
            }
        }
    }

    protected override void ImplementPack()
    {
        if (_ipToData[ClientConnectionHandler.ServerEndPoint].updated)
        {
            switch (_ipToData[ClientConnectionHandler.ServerEndPoint].gameState)
            {
                case GameStateDataPack.GameState.Initiate:
                    //spawn host
                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(Instantiate(InstantiateHandler.GetPlayerPrefab(),
                        _container.SpawnPlayer.GetPointFurthestFromOponent(_container.LocalPlayer.transform.position), Quaternion.identity)));

                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(_container.LocalPlayer));

                    UpdateHealthUI();
                    break;
                case GameStateDataPack.GameState.Restart:
                    for (int i = 0; i < ClientConnectionHandler.PlayersList.Count; i++)
                    {
                        _container.SpawnPlayer.GetPointFurthestFromOponent(i+1 >= ClientConnectionHandler.PlayersList.Count ? 
                            ClientConnectionHandler.PlayersList[0].Object.transform.position : 
                            ClientConnectionHandler.PlayersList[i+1].Object.transform.position);
                    }
                    _container.Hud.HideEndScreen();
                    ClientConnectionHandler._hasGameEnded = false;
                    UpdateHealthUI();
                    break;
                case GameStateDataPack.GameState.Continue:
                    UpdateHealthUI();
                    break;
                case GameStateDataPack.GameState.Ended:                    
                    _container.Hud.ShowEndScreen(_dataPack.playersHealth[1] > 0);
                    ClientConnectionHandler._hasGameEnded = true;
                    break;
                case GameStateDataPack.GameState.Quit:
                    ClientConnectionHandler.PlayersList.Clear();
                    ClientConnectionHandler.BulletsList.Clear();
                    ClientConnectionHandler.ServerEndPoint = null;
                    ClientConnectionHandler._hasGameEnded = false;
                    SceneManager.LoadScene("MainMenu");
                    break;
            }
            _ipToData[ClientConnectionHandler.ServerEndPoint].updated = false;
        }
    }
    private void UpdateHealthUI()
    {
        for (int i = 0; i < _dataPack.playersHealth.Count; i++)
        {
            _container.Hud.UpdateHealth(i, _dataPack.playersHealth[i]);
        }
    }
}
