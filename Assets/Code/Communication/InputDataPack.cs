
[System.Serializable]
public class InputDataPack {

    public bool updated = true;

    public float[] _movementInput = new float[2];

    public float[] _mousePoint = new float[3];
    public bool _mouseClick;

    public static int Port = 11003;
}
