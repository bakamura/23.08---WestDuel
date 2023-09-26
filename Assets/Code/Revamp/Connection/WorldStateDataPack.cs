using System.Collections.Generic;
using System.Net;

[System.Serializable]
public struct WorldStateDataPack {


    public List<float[]> playersPos;
    public List<float[]> playersVelocity;
    public List<float[]> playersMousePosition;
    public List<bool> playersHasBullet;

    public List<float[]> bulletsPos;
    public List<float[]> bulletsVelocity;

    public List<float[]> boxesPos;

    public readonly float[] deactivatePos;

    public WorldStateDataPack(byte a = 0) {

        playersPos = new List<float[]> { new float[3], new float[3] };
        playersVelocity = new List<float[]> { new float[3], new float[3] };
        playersMousePosition = new List<float[]> { new float[3], new float[3] };
        playersHasBullet = new List<bool> { new bool(), new bool() };

        bulletsPos = new List<float[]> { new float[3], new float[3], new float[3], new float[3] };
        bulletsVelocity = new List<float[]> { new float[3], new float[3], new float[3], new float[3] };

        boxesPos = new List<float[]>(2) { new float[3], new float[3] };

        deactivatePos = new float[3] { 0, -100, 0 };

        senderIp = null;
        updated = true;
    }

    public IPEndPoint senderIp;
    public bool updated;
}
