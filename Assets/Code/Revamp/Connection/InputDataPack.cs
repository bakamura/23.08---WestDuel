using System.Net;

[System.Serializable]
public struct InputDataPack {

    public byte identifierByte;

    public float[] movementInput;

    public float[] mousePoint;
    public bool mouseClick;

    public InputDataPack(byte a = 0) {
        identifierByte = 0;

        movementInput = new float[2];
        mousePoint = new float[3];
        mouseClick = false;

        senderIp = null;
        updated = true;
    }

    public IPEndPoint senderIp;
    public bool updated;

}
