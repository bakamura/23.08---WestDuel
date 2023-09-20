using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkReferenceContainer : MonoBehaviour
{
    [SerializeField] private SpawnPlayer _spawnPlayer;
    [SerializeField] private GameObject _localPlayer;
    [SerializeField] private SpawnPickup _spawnPickup;
    [SerializeField] private HUD _hud;

    public SpawnPlayer SpawnPlayer => _spawnPlayer;
    public GameObject LocalPlayer => _localPlayer;
    public SpawnPickup SpawnPickup => _spawnPickup;
    public HUD Hud => _hud;
}
