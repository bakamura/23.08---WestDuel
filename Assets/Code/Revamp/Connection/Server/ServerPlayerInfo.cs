using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public static class ServerPlayerInfo {

    public static Dictionary<IPEndPoint, PlayerInfo> player = new Dictionary<IPEndPoint, PlayerInfo>();
    private const int playerMax = 2;

    public static void InstantiatePlayer(bool isServer, IPEndPoint ip) {
        if (isServer) ConnectionHandler.serverIpEp = ip;
        GameObject go = GameObject.Instantiate(isServer ? InstantiateHandler.GetPlayer1HostPrefab() : InstantiateHandler.GetPlayer2HostPrefab(), 
                                               GameObject.FindObjectOfType<SpawnPlayer>().GetPointFurthestFromOponent(isServer ? Vector3.zero : player[ConnectionHandler.serverIpEp].transform.position), 
                                               Quaternion.identity);
        player.Add(ip, new PlayerInfo(go.transform,
                                      go.GetComponentInChildren<Rigidbody>(),
                                      go.GetComponent<PlayerHealth>(),
                                      go.GetComponent<PlayerMovement>(),
                                      go.GetComponent<PlayerShoot>()));

        if (player.Count == playerMax) {
            IPEndPoint[] ipEpArr = player.Keys.ToArray();
            ServerWorldStateSender.Instance.AddPlayerIP(ipEpArr);
            ServerGameStateSender.Instance.AddPlayerIP(ipEpArr);
        }
    }
}

public struct PlayerInfo {
    public Transform transform;
    public Rigidbody rigidBody;
    public PlayerHealth health;
    public PlayerMovement movement;
    public PlayerShoot shoot;

    public PlayerInfo(Transform transform, Rigidbody rigidBody, PlayerHealth health, PlayerMovement movement, PlayerShoot shoot) {
        this.transform = transform;
        this.rigidBody = rigidBody;
        this.health = health;
        this.movement = movement;
        this.shoot = shoot;
    }
}