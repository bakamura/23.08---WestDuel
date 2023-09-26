using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ServerWorldStateSender : MonoBehaviour {

    private WorldStateDataPack _dataPackCache;
    private Dictionary<IPEndPoint, List<Rigidbody>> playerBulletRigidBody = new Dictionary<IPEndPoint, List<Rigidbody>>();

    private void FixedUpdate() {
        PreparePack();
    }

    private void PreparePack() {
        if (ServerPlayerInfo.player.Count > 0) {
            foreach (IPEndPoint ip in ServerPlayerInfo.player.Keys) {
                _dataPackCache.playersPos[ip] = PackingUtility.Vector3ToFloatArray(ServerPlayerInfo.player[ip].transform.position);
                _dataPackCache.playersVelocity[ip] = PackingUtility.Vector3ToFloatArray(ServerPlayerInfo.player[ip].rigidBody.velocity);
                _dataPackCache.playersShootPoint[ip] = PackingUtility.Vector3ToFloatArray(ServerPlayerInfo.player[ip].shoot.CurrentAim()); // Create vector3 CurrentAim() in PlayerShoot
                _dataPackCache.playersHasBullet[ip] = ServerPlayerInfo.player[ip].shoot.CheckBullet();

                for (int i = 0; i < _dataPackCache.bulletsPos.Count; i++) {
                    _dataPackCache.bulletsPos[ip][i] = PackingUtility.Vector3ToFloatArray(playerBulletRigidBody[ip][i].transform.position);
                    _dataPackCache.bulletsVelocity[ip][i] = PackingUtility.Vector3ToFloatArray(playerBulletRigidBody[ip][i].velocity);
                }

                for(int i = 0; i < 2; i++) {
                    // Get max bulletPickup Amount
                    // Update Each
                }
            }
            DataSendHandler.SendPack(ConnectionHandler.DataPacksIdentification.WorldStateDataPack, _dataPackCache, null); // null => Joiner IP
        }
    }

    public void AddBulletPool(IPEndPoint ipOwner, Bullet[] bulletPool) {
        List<Rigidbody> rbList = new List<Rigidbody>();
        foreach (Bullet bullet in bulletPool) rbList.Add(bullet.GetComponent<Rigidbody>());
        playerBulletRigidBody.Add(ipOwner, rbList);
    }

    public void AddPlayerIP(IPEndPoint[] playerIp) { // Make ServerPlayerInfo call this
        _dataPackCache = new WorldStateDataPack(playerIp);
    }

}
