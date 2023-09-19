using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class ServerConnectionHandler {

    [Header("Server Handling")]
    public static UdpClient udpClient = new UdpClient(11000);
    public static List<PlayerInfo> players = new List<PlayerInfo>();

}

public class PlayerInfo {
    public IPEndPoint ip;
    public Transform transform;
    public Rigidbody rigidBody;
    public PlayerHealth health;
    public PlayerMovementClient movement;
    public PlayerShoot shoot;
}