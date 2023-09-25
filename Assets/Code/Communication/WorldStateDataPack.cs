using System.Collections.Generic;

[System.Serializable]
public class WorldStateDataPack {

    public bool updated = true;

    public List<float[]> playersPos = new List<float[]> { new float[3], new float[3] };
    public List<float[]> playersVelocity = new List<float[]> { new float[3], new float[3] };
    public List<float[]> playersMousePosition = new List<float[]> { new float[3], new float[3] };
    public List<bool> playersHasBullet = new List<bool> { new bool(), new bool() };

    public List<float[]> bulletsPos = new List<float[]> { new float[3], new float[3], new float[3], new float[3] };
    public List<float[]> bulletsVelocity = new List<float[]> { new float[3], new float[3], new float[3], new float[3] };

    public List<float[]> boxesPos = new List<float[]>(2) { new float[3], new float[3] };

    public readonly float[] deactivatePos =  { 0, -100, 0 };

    public static int Port = 11002;
}
