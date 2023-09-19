using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BulletPickup : MonoBehaviour
{
    [SerializeField] private SoundManager.SfxAudioData _audioData;
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
        SetPosition(_spawnPointUsed);
    }

    public void SetPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }

    public void UpdateState(bool isActive)
    {
        gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerShoot>().GetBullet();
        CollectBullet();
    }

    public void CollectBullet()
    {
        SoundManager.Instance.PlaySoundEffect(_audioData, transform.position);
        UpdateState(false);
        OnCollect?.Invoke(_spawnPointUsed);
    }
}
