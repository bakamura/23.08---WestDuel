using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPickup : MonoBehaviour{

    [Header("Values")]
    [SerializeField] private Vector3[] _spawnPoints;
    [SerializeField] private float _delayBetweenSpawns;
    [SerializeField] private GameObject _bulletPickupPrefab;
    [SerializeField] private byte _maxAmmoPickupsInScene = 2;
    [SerializeField] private bool _startWithAutomaticSpawns = true;

    [Header("Debug")]
    [SerializeField] private bool _debugMode;
    [SerializeField] private Color[] _colorPoints;
    [SerializeField] private float _gizmoSize;

    private WaitForSeconds _delay;
    private bool _spawnByDelay = true;
    private byte _currentBoxAmount;
    private List<Vector3> _currentAvailable;
    private Coroutine _autoSpawnCoroutine;

    private void Awake()
    {
        _delay = new WaitForSeconds(_delayBetweenSpawns);
        _currentAvailable = new List<Vector3>(_spawnPoints);
        _spawnByDelay = _startWithAutomaticSpawns;
        UpdateAutoSpawn();
    }

    private void UpdateAutoSpawn()
    {
        if(_spawnByDelay && _autoSpawnCoroutine == null)
        {
            _autoSpawnCoroutine = StartCoroutine(SpawnDelay());
        }
    }

    private IEnumerator SpawnDelay()
    {
        while (_spawnByDelay)
        {
            Spawn();
            yield return _delay;
        }
        _autoSpawnCoroutine = null;
    }

    //public void SetSpawnAutomatic(bool activate)
    //{
    //    _spawnByDelay = activate;
    //    UpdateAutoSpawn();
    //}

    private void Spawn()
    {
        if (_currentBoxAmount < _maxAmmoPickupsInScene)
        {
            Instantiate(_bulletPickupPrefab, GetValidSpawnPoint(), Quaternion.identity);
            _currentBoxAmount++;
        }
    }

    private Vector3 GetValidSpawnPoint()
    {
        Vector3 temp = _currentAvailable[UnityEngine.Random.Range(0, _currentAvailable.Count)];
        _currentAvailable.Remove(temp);
        return temp;
    }

    private void OnValidate()
    {
        Color[] temp = _colorPoints;
        _colorPoints = new Color[_spawnPoints.Length];
        for (int i =  0; i < _colorPoints.Length; i++)
        {
            if (i >= temp.Length) break;
            _colorPoints[i] = temp[i];
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_debugMode)
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                Gizmos.color = _colorPoints[i];
                Gizmos.DrawSphere(transform.position + _spawnPoints[i], _gizmoSize);
            }
        }
    }
}
