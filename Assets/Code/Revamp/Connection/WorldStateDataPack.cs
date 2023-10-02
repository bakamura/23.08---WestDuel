using System.Collections.Generic;
using System.Net;

[System.Serializable]
public struct WorldStateDataPack {

    public Dictionary<IPEndPoint, float[]> playersPos;
    public Dictionary<IPEndPoint, float[]> playersVelocity;
    public Dictionary<IPEndPoint, float[]> playersShootPoint;
    public Dictionary<IPEndPoint, bool> playersHasBullet;

    public Dictionary<IPEndPoint, List<float[]>> bulletsPos;
    public Dictionary<IPEndPoint, List<float[]>> bulletsVelocity;

    public List<float[]> boxesPos;

    public readonly float[] deactivatePos;

    public WorldStateDataPack(IPEndPoint[] playerIp) {
        playersPos = new Dictionary<IPEndPoint, float[]>();
        playersVelocity = new Dictionary<IPEndPoint, float[]>();
        playersShootPoint = new Dictionary<IPEndPoint, float[]>();
        playersHasBullet = new Dictionary<IPEndPoint, bool>();
        foreach (IPEndPoint ip in playerIp) {
            playersPos.Add(ip, new float[3]);
            playersVelocity.Add(ip, new float[3]);
            playersShootPoint.Add(ip, new float[3]);
            playersHasBullet.Add(ip, false);
        }

        bulletsPos = new Dictionary<IPEndPoint, List<float[]>>();
        bulletsVelocity = new Dictionary<IPEndPoint, List<float[]>>();

        List<float[]> listCache;
        foreach (IPEndPoint ip in playerIp) {
            listCache = new List<float[]>();
            for (int i = 0; i < 2; i++) listCache.Add(new float[PlayerShoot.MaxBulletAmount]);
            bulletsPos.Add(ip, listCache);
            bulletsVelocity.Add(ip, listCache);
        }

        boxesPos = new List<float[]>();
        for (int i = 0; i < 2; i++) { // Provisory should read max BulletPickups
            boxesPos.Add(new float[3]);
        }

        deactivatePos = new float[3] { 0, 256, 0 };

        ipEpString = null;
    }

    //public IPEndPoint senderIp;
    public string ipEpString; // I hate this

}
