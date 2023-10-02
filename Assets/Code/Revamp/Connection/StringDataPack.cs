using System.Net;

[System.Serializable]
public struct StringDataPack {

    public string stringSent;

    public StringDataPack(IPEndPoint senderIp) {
        stringSent = string.Empty;

        this.ipEpString = senderIp.ToString();
    }

    //public IPEndPoint senderIp;
    public string ipEpString; // I hate this

}
