using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class ClientConnectionHandler
{
    // Stores UDP Client + Server's IPEndPoint
    public static Dictionary<IPEndPoint, MovableObjectData> PlayersList = new Dictionary<IPEndPoint, MovableObjectData>();
    //public static List<BulletPickup> AmmoBoxList = new List<BulletPickup>();

    [System.Serializable]
    public struct MovableObjectData
    {
        public GameObject Object;
        public Rigidbody Rigidbody;
        public AnimationsUpdate AnimationsUpdate;

        public MovableObjectData(GameObject obj)
        {
            Object = obj;
            Rigidbody = Object.GetComponent<Rigidbody>();
            AnimationsUpdate = Object.GetComponent<AnimationsUpdate>();
        }
    }

    //public static IPAddress ServerEndPoint;
    public static bool _hasGameEnded;

    //public static IPEndPoint SetIpEndPoint(string IpAddress, int port)
    //{
    //    IPEndPoint temp;
    //    if (IPAddress.TryParse(IpAddress, out IPAddress address))
    //    {
    //        temp = new IPEndPoint(address, port);
    //        return temp;
    //    }
    //    else
    //    {
    //        Debug.LogError($"the Address {IpAddress} is invalid");
    //        return null;
    //    }
    //}
}