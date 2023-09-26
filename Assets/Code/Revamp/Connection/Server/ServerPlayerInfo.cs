using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class ServerPlayerInfo {

    public static Dictionary<IPEndPoint, PlayerInfo> player = new Dictionary<IPEndPoint, PlayerInfo>();

    public static void InstantiatePlayer(bool isServer, IPEndPoint ip) {
        GameObject go = GameObject.Instantiate(isServer ? InstantiateHandler.GetPlayer1HostPrefab() : InstantiateHandler.GetPlayer2HostPrefab()); //
        player.Add(ip, new PlayerInfo(go.transform,
                                      go.GetComponentInChildren<Rigidbody>(),
                                      go.GetComponentInChildren<PlayerHealth>(),
                                      go.GetComponentInChildren<PlayerMovement>(),
                                      go.GetComponentInChildren<PlayerShoot>()));
    }
}

public struct PlayerInfo {
    public Transform transform;
    public Rigidbody rigidBody;
    public PlayerHealth health;
    public PlayerMovement movement;
    public PlayerShoot shoot;

    public PlayerInfo(IPAddress ip, Transform transform, Rigidbody rigidBody, PlayerHealth health, PlayerMovement movement, PlayerShoot shoot) {
        this.transform = transform;
        this.rigidBody = rigidBody;
        this.health = health;
        this.movement = movement;
        this.shoot = shoot;
    }
}