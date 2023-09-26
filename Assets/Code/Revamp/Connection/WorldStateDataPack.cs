using System.Collections.Generic;
using System.Net;

[System.Serializable]
public struct WorldStateDataPack {


    public Dictionary<IPEndPoint, float[]> playersPos;
    public Dictionary<IPEndPoint, float[]> playersVelocity;
    public Dictionary<IPEndPoint, float[]> playersShootPoint;
    public Dictionary<IPEndPoint, bool> playersHasBullet;

    public List<Dictionary<IPEndPoint, float[]>> bulletsPos;
    public List<Dictionary<IPEndPoint, float[]>> bulletsVelocity;

    public List<float[]> boxesPos;

    public readonly float[] deactivatePos;

    public WorldStateDataPack(IPEndPoint[] playerIp) {

        playersPos = new Dictionary<IPEndPoint, float[]>();
        playersVelocity = new Dictionary<IPEndPoint, float[]>();
        playersShootPoint = new Dictionary<IPEndPoint, float[]>();
        playersHasBullet = new Dictionary<IPEndPoint, bool>();
        foreach(IPEndPoint ip in playerIp) {
            playersPos.Add(ip, new float[3]);
            playersVelocity.Add(ip, new float[3]);
            playersShootPoint.Add(ip, new float[3]);
            playersHasBullet.Add(ip, false);
        }

        bulletsPos = new List<Dictionary<IPEndPoint, float[]>>();
        bulletsVelocity = new List<Dictionary<IPEndPoint, float[]>>();

        Dictionary<IPEndPoint, float[]> dictPos;
        Dictionary<IPEndPoint, float[]> dictVelocity;
        for (int i =0; i < 2; i++) { // Provisory should read Max Bullet Instead
            dictPos = new Dictionary<IPEndPoint, float[]>();
            dictVelocity = new Dictionary<IPEndPoint, float[]>();
            foreach (IPEndPoint ip in playerIp) {
                dictPos.Add(ip, new float[3]);
                dictVelocity.Add(ip, new float[3]);
            }
            bulletsPos.Add(dictPos);
            bulletsVelocity.Add(dictPos);
        }

        boxesPos = new List<float[]>();

        deactivatePos = new float[3] { 0, 256, 0 };

        senderIp = null;
    }

    public IPEndPoint senderIp;
}
