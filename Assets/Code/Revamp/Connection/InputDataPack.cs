using System.Net;

[System.Serializable]
public struct InputDataPack {

    public byte identifierByte;

    public float[] movementInput;

    public float[] shootPoint;
    public bool shootTrigger;

    public InputDataPack(byte a = 0) {
        identifierByte = 0;

        movementInput = new float[2];
        shootPoint = new float[3];
        shootTrigger = false;

        ipEpString = null;
    }

    //public IPEndPoint senderIp;
    public string ipEpString; // I hate this

}
