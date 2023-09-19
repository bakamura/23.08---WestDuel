using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class ClientConnectionHandler
{
    // Stores UDP Client + Server's IPEndPoint
    public static List<MovableObjectData> PlayersList = new List<MovableObjectData>();
    public static List<Bullet> BulletsList = new List<Bullet>();
    //public static List<BulletPickup> AmmoBoxList = new List<BulletPickup>();

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
    public static IPEndPoint ServerEndPoint { get { return ServerEndPoint; } set { SetIpEndPoint(string.Empty); } }
    public static UdpClient UdpClient = new UdpClient(Port);

    public static IPEndPoint SetIpEndPoint(string IpAddress)
    {
        IPEndPoint temp;
        if (IPAddress.TryParse(IpAddress, out IPAddress address))
        {
            temp = new IPEndPoint(address, Port);
            return temp;
        }
        else
        {
            Debug.LogError($"the Address {IpAddress} is invalid");
            return null;
        }
    }
}