using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public static class ServerConnectionHandler {

    [Header("Server Handling")]
    public static List<PlayerInfo> players = new List<PlayerInfo>();

}

public class PlayerInfo {
    public IPAddress ip;
    public Transform transform;
    public Rigidbody rigidBody;
    public PlayerHealth health;
    public PlayerMovementServer movement;
    public PlayerShootServer shoot;
    public AnimationsUpdate animationsUpdate;
}