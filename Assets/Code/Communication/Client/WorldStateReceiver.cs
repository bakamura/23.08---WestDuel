using System.IO;
using UnityEngine;
using System.Net;

public class WorldStateReceiver : DataReceiver<WorldStateDataPack>
{
    [SerializeField] private NetworkReferenceContainer _container;
    private WorldStateDataPack _dataPack;
    private bool[] _bulletsShoot = new bool[4];//size is playerCount * MaxBulletPerPlayer

    private void Start()
    {
        for (int a = 0; a < _bulletsShoot.Length; a++)
        {
            Bullet temp = Instantiate(InstantiateHandler.GetBulletPrefab(), null).GetComponent<Bullet>();
            ClientConnectionHandler.BulletsList.Add(temp);
        }
    }

    protected override void ReceivePack()
    {
        while (true)
        {
            IPEndPoint temp = ClientConnectionHandler.ServerEndPoint;
            _memoryStream = new MemoryStream(ClientConnectionHandler.UdpClient.Receive(ref temp));
            if (temp == ClientConnectionHandler.ServerEndPoint)
            {
                _dataPack = CheckDataPack<WorldStateDataPack>(DataPacksIdentification.GamStateDataPack);
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
            #region UpdatePlayersPositions
            for (int i = 0; i < _ipToData[ClientConnectionHandler.ServerEndPoint].playersPos.Count; i++)
            {
                ClientConnectionHandler.PlayersList[i].Object.transform.position = PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].playersPos[i]);
                ClientConnectionHandler.PlayersList[i].Rigidbody.velocity = PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].playersVelocity[i]);
            }
            #endregion
            #region UpdateBullets               
            //if (_ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos.Count > ClientConnectionHandler.BulletsList.Count)
            //{
            //    int newBullets = _ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos.Count - ClientConnectionHandler.BulletsList.Count;
            //    for (int a = _ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos.Count - newBullets; a < _ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos.Count; a++)
            //    {
            //        Bullet temp = Instantiate(InstantiateHandler.GetBulletPrefab(), null).GetComponent<Bullet>();
            //        temp.Shoot(PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos[a]), PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].bulletsVelocity[a]));
            //        ClientConnectionHandler.BulletsList.Add(temp);
            //        _bulletsShoot[a] = true;
            //    }
            //}
            for (int i = 0; i < _ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos.Count; i++)
            {
                if (_ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos[i] == _ipToData[ClientConnectionHandler.ServerEndPoint].deactivatePos)
                {
                    ClientConnectionHandler.BulletsList[i].UpdateState(false);
                }
                else
                {
                    ClientConnectionHandler.BulletsList[i].UpdateState(true);
                    ClientConnectionHandler.BulletsList[i].Shoot(PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].bulletsPos[i]), PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].bulletsVelocity[i]));
                    _bulletsShoot[i] = true;
                }
            }
            #endregion
            #region UpdateAmmoBoxes
            //if (_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos.Count > ClientConnectionHandler.AmmoBoxList.Count)
            //{
            //    int newBoxes = _ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos.Count - ClientConnectionHandler.AmmoBoxList.Count;
            //    for (int i = 0; i < newBoxes; i++)
            //    {
            //        ClientConnectionHandler.AmmoBoxList.Add(_container.SpawnPickup.Spawn());
            //        //ClientConnectionHandler.AmmoBoxList.Add(Instantiate(InstantiateHandler.GetAmmoBoxPrefab(),
            //        //    PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos[i]),
            //        //    Quaternion.identity).GetComponent<BulletPickup>());
            //    }
            //}
            //else if (_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos.Count < ClientConnectionHandler.AmmoBoxList.Count)
            //{
            //    BulletPickup[] temp = ClientConnectionHandler.AmmoBoxList.Select(x => !_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos.Contains(PackingUtility.Vector3ToFloatArray(x.GetComponent<Transform>().position))).ToArray();
            //    for (int i = 0; i < temp.Length; i++)
            //    {
            //        ClientConnectionHandler.AmmoBoxList[i].CollectBullet();
            //    }
            //}
            //else
            //{
            // TALVEZ TENHA PROBLEMA DE VER CAIXAS TELEPORTANDO J� Q ELE SEMPRE PEGA O 1 DISPONIVEL
            for (int i = 0; i < _ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos.Count; i++)
            {
                if (_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos[i] == _ipToData[ClientConnectionHandler.ServerEndPoint].deactivatePos)
                {
                    _container.SpawnPickup.Spawn().CollectBullet();
                }
                else
                {
                    _container.SpawnPickup.Spawn().SetPosition(PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos[i]));
                    //BulletPickup temp = _container.SpawnPickup.Spawn();
                    //    temp.transform.position = PackingUtility.FloatArrayToVector3(_ipToData[ClientConnectionHandler.ServerEndPoint].boxesPos[i]);
                    //    ClientConnectionHandler.AmmoBoxList[i] = temp;
                }
            }
            //}
            #endregion
            #region UpdatePlayersAnimations
            for (int i = 0; i < _ipToData[ClientConnectionHandler.ServerEndPoint].playersPos.Count; i++)
            {
                ClientConnectionHandler.PlayersList[i].AnimationsUpdate.SetDirection(PackingUtility.FloatArrayToVector3(_dataPack.playersVelocity[i]));
                ClientConnectionHandler.PlayersList[i].AnimationsUpdate.SetMousePosition(PackingUtility.FloatArrayToVector3(_dataPack.playersMousePosition[i]));
                for(int a  = _ipToData[ClientConnectionHandler.ServerEndPoint].playersPos.Count * i; a < _bulletsShoot.Length; a++)
                {
                    ClientConnectionHandler.PlayersList[i].AnimationsUpdate.TriggerShootAnim();
                    _bulletsShoot[a] = false;
                }
            }
            #endregion
        }
        _ipToData[ClientConnectionHandler.ServerEndPoint].updated = false;
    }
}