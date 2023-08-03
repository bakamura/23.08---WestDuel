using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletPickup : MonoBehaviour
{
    private BulletPickup _nextInPoolList;
    private Vector3 _spawnPointUsed;

    public BulletPickup NextInPoolList => _nextInPoolList;
    public Vector3 SpawnPointUsed => _spawnPointUsed;
    public Action<Vector3> OnCollect;

    public void SetNextInLine(BulletPickup next)
    {
        _nextInPoolList = next;
    }

    public void SetSpawnPointUsed(Vector3 spawnPoint)
    {
        _spawnPointUsed = spawnPoint;
        transform.position = _spawnPointUsed;
    }

    public void UpdateState(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerShoot>().GetBullet();
        UpdateState(false);
        OnCollect?.Invoke(_spawnPointUsed);
    }
}
