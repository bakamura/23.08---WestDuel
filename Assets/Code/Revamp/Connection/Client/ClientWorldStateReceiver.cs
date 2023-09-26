using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using System.Linq;

public class ClientWorldStateReceiver : MonoBehaviour
{
    private Dictionary<IPEndPoint, List<Bullet>> _bullets = new Dictionary<IPEndPoint, List<Bullet>>();
    private List<BulletPickup> _bulletPickups = new List<BulletPickup>();

    private void FixedUpdate()
    {
        ProcessData();
    }
    private void ProcessData()
    {
        if (DataReceiveHandler.queueWorldData.Count > 0)
        {
            WorldStateDataPack temp = DataReceiveHandler.queueWorldData.Dequeue();
            #region UpdatePlayersAndAnimations
            IPEndPoint[] playersIPs = new IPEndPoint[temp.playersPos.Keys.Count];
            temp.playersPos.Keys.CopyTo(playersIPs, 0);
            for (int i = 0; i < playersIPs.Length; i++)
            {
                //movment
                ClientConnectionHandler.PlayersList[playersIPs[i]].Object.transform.position = PackingUtility.FloatArrayToVector3(temp.playersPos[playersIPs[i]]);
                ClientConnectionHandler.PlayersList[playersIPs[i]].Rigidbody.velocity = PackingUtility.FloatArrayToVector3(temp.playersVelocity[playersIPs[i]]);
                //animations
                ClientConnectionHandler.PlayersList[playersIPs[i]].AnimationsUpdate.SetDirection(PackingUtility.FloatArrayToVector3(temp.playersVelocity[playersIPs[i]]));
                ClientConnectionHandler.PlayersList[playersIPs[i]].AnimationsUpdate.SetMousePosition(PackingUtility.FloatArrayToVector3(temp.playersShootPoint[playersIPs[i]]));
                for (int a = 0; a < temp.bulletsPos[playersIPs[i]].Count; i++)
                {
                    if (PackingUtility.FloatArrayToVector3(temp.bulletsPos[playersIPs[i]][a]) != PackingUtility.FloatArrayToVector3(temp.deactivatePos))
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
                for (int a = 0; a < temp.bulletsPos[playersIPs[i]].Count; a++)
                {
                    if (_bullets[playersIPs[i]][a].transform.position == PackingUtility.FloatArrayToVector3(temp.deactivatePos))
                    {
                        _bullets[playersIPs[i]][a].UpdateState(false);
                    }
                    else
                    {
                        _bullets[playersIPs[i]][a].UpdateState(true);
                        _bullets[playersIPs[i]][a].Shoot(PackingUtility.FloatArrayToVector3(temp.bulletsPos[playersIPs[i]][a]), PackingUtility.FloatArrayToVector3(temp.bulletsVelocity[playersIPs[i]][a]));
                    }
                }
            }
            #endregion
            #region UpdateAmmoBoxes
            if (_bulletPickups.Count == 0) InstantiateAmmoBoxes(temp.boxesPos.Count);
            for (int i = 0; i < _bulletPickups.Count; i++)
            {
                if (_bulletPickups[i].transform.position == PackingUtility.FloatArrayToVector3(temp.deactivatePos))
                {
                    _bulletPickups[i].UpdateState(false);
                }
                else
                {
                    _bulletPickups[i].SetPosition(PackingUtility.FloatArrayToVector3(temp.boxesPos[i]));
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
