using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class ServerConnectionHandler {

    [Header("Server Handling")]
    public static List<PlayerInfo> players = new List<PlayerInfo>();

    public static void InstantiatePlayer(bool isServer, IPAddress ip) {
        GameObject go = isServer ? InstantiateHandler.GetPlayer1HostPrefab() : InstantiateHandler.GetPlayer2HostPrefab();
        PlayerInfo info = new PlayerInfo();
        info.ip = ip;
        info.transform = go.transform;
        info.rigidBody = go.GetComponent<Rigidbody>();
        info.health = go.GetComponent<PlayerHealth>();
        info.movement = go.GetComponent<PlayerMovementServer>();
        info.shoot = go.GetComponent<PlayerShootServer>();
        info.animationsUpdate = go.GetComponent<AnimationsUpdate>();
        players.Add(info);
    }

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