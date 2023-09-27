using System.Net;

[System.Serializable]
public struct StringDataPack {

    public byte identifierByte;

    public string stringSent;

    public StringDataPack(IPEndPoint senderIp) {
        identifierByte = 3;

        stringSent = string.Empty;

        this.ipEpString = senderIp.ToString();
    }

    //public IPEndPoint senderIp;
    public string ipEpString; // I hate this

}
