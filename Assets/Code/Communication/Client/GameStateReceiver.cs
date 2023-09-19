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
                case GameStateDataPack.GameState.Continue:
                    //atualiza a UI de Vida
                    break;
                case GameStateDataPack.GameState.Ended:
                    ClientConnectionHandler.PlayersList.Clear();
                    ClientConnectionHandler.BulletsList.Clear();
                    ClientConnectionHandler.ServerEndPoint = null;
                    SceneManager.LoadScene("MainMenu");
                    break;
                case GameStateDataPack.GameState.Initiate:
                    //spawn host
                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(Instantiate(InstantiateHandler.GetPlayerPrefab(),
                        _container.SpawnPlayer.GetPointFurthestFromOponent(_container.LocalPlayer.transform.position), Quaternion.identity)));

                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(_container.LocalPlayer));

                    //atualiza a UI de Vida
                    break;
                case GameStateDataPack.GameState.Restart:
                    for (int i = 0; i < ClientConnectionHandler.PlayersList.Count; i++)
                    {
                        _container.SpawnPlayer.GetPointFurthestFromOponent(i+1 >= ClientConnectionHandler.PlayersList.Count ? 
                            ClientConnectionHandler.PlayersList[0].Object.transform.position : 
                            ClientConnectionHandler.PlayersList[i+1].Object.transform.position);
                    }
                    //atualiza a UI de Vida
                    break;
            }
            _ipToData[ClientConnectionHandler.ServerEndPoint].updated = false;
        }
    }
}
