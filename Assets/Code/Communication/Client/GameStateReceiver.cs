using System.IO;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateReceiver : DataReceiver<GameStateDataPack>
{
    [SerializeField] private NetworkReferenceContainer _container;
    private GameStateDataPack _dataPack;
    private UdpClient _udpClient;
    private IPEndPoint _endPoint;

    protected override void Awake()
    {
        _udpClient = new UdpClient(GameStateDataPack.PortClientReceive);
        base.Awake();
    }

    protected override void ReceivePack()
    {        
        while (true)
        {
            print("running");
            _memoryStream = new MemoryStream(_udpClient.Receive(ref _endPoint));
            if (_endPoint.Address == ClientConnectionHandler.ServerEndPoint)
            {
                _dataPack = CheckDataPack<GameStateDataPack>(DataPacksIdentification.GamStateDataPack);
                if (_dataPack != null)
                {
                    if (!_ipToData.ContainsKey(ClientConnectionHandler.ServerEndPoint)) _ipToData.Add(ClientConnectionHandler.ServerEndPoint, _dataPack);
                    else _ipToData[_endPoint.Address] = _dataPack;
                }
            }
        }
    }

    protected override void ImplementPack()
    {
        if (_ipToData.ContainsKey(ClientConnectionHandler.ServerEndPoint) && _ipToData[ClientConnectionHandler.ServerEndPoint].updated)
        {
            switch (_ipToData[ClientConnectionHandler.ServerEndPoint].gameState)
            {
                case GameStateDataPack.GameState.Initiate:
                    //spawn host
                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(Instantiate(InstantiateHandler.GetPlayer1ClientPrefab(),
                        _container.SpawnPlayer.GetPointFurthestFromOponent(_container.LocalPlayer.transform.position), Quaternion.identity)));

                    ClientConnectionHandler.PlayersList.Add(new ClientConnectionHandler.MovableObjectData(_container.LocalPlayer));

                    UpdateHealthUI();
                    break;
                case GameStateDataPack.GameState.Restart:
                    for (int i = 0; i < ClientConnectionHandler.PlayersList.Count; i++)
                    {
                        _container.SpawnPlayer.GetPointFurthestFromOponent(i + 1 >= ClientConnectionHandler.PlayersList.Count ?
                            ClientConnectionHandler.PlayersList[0].Object.transform.position :
                            ClientConnectionHandler.PlayersList[i + 1].Object.transform.position);
                    }
                    _container.Hud.HideEndScreen();
                    ClientConnectionHandler._hasGameEnded = false;
                    UpdateHealthUI();
                    break;
                case GameStateDataPack.GameState.Continue:
                    UpdateHealthUI();
                    break;
                case GameStateDataPack.GameState.Ended:
                    _container.Hud.ShowEndScreen(_dataPack.playerHealth[1] > 0);
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
        for (int i = 0; i < _dataPack.playerHealth.Count; i++)
        {
            _container.Hud.UpdateHealth(i, _dataPack.playerHealth[i]);
        }
    }

    private void OnDestroy()
    {
        _udpClient.Close();
    }
}
