using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Linq;

public class ClientWorldStateReceiver : MonoBehaviour
{
    private Dictionary<IPEndPoint, List<Bullet>> _bullets = new Dictionary<IPEndPoint, List<Bullet>>();
    private List<BulletPickup> _bulletPickups = new List<BulletPickup>();

    [Header("Cache")]

    private WorldStateDataPack _dataPackCache;

    private void FixedUpdate()
    {
        ProcessData();
    }
    private void ProcessData()
    {
        while (DataReceiveHandler.queueWorldData.Count > 0)
        {
            _dataPackCache = DataReceiveHandler.queueWorldData.Dequeue();
            #region UpdatePlayersAndAnimations
            IPEndPoint[] playersIPs = new IPEndPoint[_dataPackCache.playersPos.Keys.Count];
            _dataPackCache.playersPos.Keys.CopyTo(playersIPs, 0);
            for (int i = 0; i < playersIPs.Length; i++)
            {
                //movment
                ClientConnectionHandler.PlayersList[playersIPs[i]].Object.transform.position = PackingUtility.FloatArrayToVector3(_dataPackCache.playersPos[playersIPs[i]]);
                ClientConnectionHandler.PlayersList[playersIPs[i]].Rigidbody.velocity = PackingUtility.FloatArrayToVector3(_dataPackCache.playersVelocity[playersIPs[i]]);
                //animations
                ClientConnectionHandler.PlayersList[playersIPs[i]].AnimationsUpdate.SetDirection(PackingUtility.FloatArrayToVector3(_dataPackCache.playersVelocity[playersIPs[i]]));
                ClientConnectionHandler.PlayersList[playersIPs[i]].AnimationsUpdate.SetMousePosition(PackingUtility.FloatArrayToVector3(_dataPackCache.playersShootPoint[playersIPs[i]]));
                for (int a = 0; a < _dataPackCache.bulletsPos[playersIPs[i]].Count; i++)
                {
                    if (PackingUtility.FloatArrayToVector3(_dataPackCache.bulletsPos[playersIPs[i]][a]) != PackingUtility.FloatArrayToVector3(_dataPackCache.deactivatePos))
                    {
                        ClientConnectionHandler.PlayersList[playersIPs[i]].AnimationsUpdate.TriggerShootAnim();
                        break;
                    }
                }
            }
            #endregion
            #region UpdateBullets
            if (_bullets.Count == 0) InstantiateBullets(playersIPs, 2/*maxBulletInstances*/);
            for (int i = 0; i < playersIPs.Length; i++)
            {
                for (int a = 0; a < _dataPackCache.bulletsPos[playersIPs[i]].Count; a++)
                {
                    if (_bullets[playersIPs[i]][a].transform.position == PackingUtility.FloatArrayToVector3(_dataPackCache.deactivatePos))
                    {
                        _bullets[playersIPs[i]][a].UpdateState(false);
                    }
                    else
                    {
                        _bullets[playersIPs[i]][a].UpdateState(true);
                        _bullets[playersIPs[i]][a].Shoot(PackingUtility.FloatArrayToVector3(_dataPackCache.bulletsPos[playersIPs[i]][a]), PackingUtility.FloatArrayToVector3(_dataPackCache.bulletsVelocity[playersIPs[i]][a]));
                    }
                }
            }
            #endregion
            #region UpdateAmmoBoxes
            if (_bulletPickups.Count == 0) InstantiateAmmoBoxes(_dataPackCache.boxesPos.Count);
            for (int i = 0; i < _bulletPickups.Count; i++)
            {
                if (_bulletPickups[i].transform.position == PackingUtility.FloatArrayToVector3(_dataPackCache.deactivatePos))
                {
                    _bulletPickups[i].UpdateState(false);
                }
                else
                {
                    _bulletPickups[i].SetPosition(PackingUtility.FloatArrayToVector3(_dataPackCache.boxesPos[i]));
                    _bulletPickups[i].UpdateState(true);
                }
            }
            #endregion
        }
    }

    private void InstantiateBullets(IPEndPoint[] keys, int amount)
    {
        Bullet[] temp = new Bullet[amount];
        for (int i = 0; i < keys.Length; i++)
        {
            for (int a = 0; a < amount; a++)
            {
                temp[a] = Instantiate(InstantiateHandler.GetBulletPrefab(), null).GetComponent<Bullet>();
            }
            _bullets.Add(keys[i], temp.ToList());
        }
    }

    private void InstantiateAmmoBoxes(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _bulletPickups.Add(Instantiate(InstantiateHandler.GetAmmoBoxPrefab(), null).GetComponent<BulletPickup>());
        }
    }
}
