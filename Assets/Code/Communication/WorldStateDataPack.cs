using System.Collections.Generic;

[System.Serializable]
public class WorldStateDataPack {

    public bool updated = true;

    public List<float[]> playersPos = new List<float[]>();
    public List<float[]> playersVelocity = new List<float[]>();
    public List<float[]> playersMousePosition = new List<float[]>();
    public List<bool> playersHasBullet = new List<bool>();

    public List<float[]> bulletsPos = new List<float[]>();
    public List<float[]> bulletsVelocity = new List<float[]>();

    public List<float[]> boxesPos = new List<float[]>();

    public readonly float[] deactivatePos =  { 0, -100, 0 };

    public static int Port = 11002;
}
