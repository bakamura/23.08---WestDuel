using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class ClientConnectionHandler
{
    // Stores UDP Client + Server's IPEndPoint
    public static List<MovableObjectData> PlayersList = new List<MovableObjectData>();
    public static List<MovableObjectData> BulletsList = new List<MovableObjectData>();
    public static List<BulletPickup> AmmoBoxList = new List<BulletPickup>();

    [System.Serializable]
    public struct MovableObjectData
    {
        public GameObject Object;
        public Rigidbody Rigidbody;

        public MovableObjectData(GameObject obj)
        {
            Object = obj;
            Rigidbody = Object.GetComponent<Rigidbody>();
        }
    }

    public const int Port = 11000;
    public static IPEndPoint ServerEndPoint { get { return ServerEndPoint; } set { } }
    public static UdpClient UdpClient = new UdpClient(Port);

    public static void SetIpEndPoint(string IpAddress)
    {
        if (IPAddress.TryParse(IpAddress, out IPAddress address))
        {
            ServerEndPoint = new IPEndPoint(address, Port);
        }
        else
        {
            Debug.LogError($"the Address {IpAddress} is invalid");
        }
    }
}