using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPickup : MonoBehaviour
{
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
    private BulletPickup[] _bullets;
    private BulletPickup _bulletAvailable;

    private void Awake()
    {
        _delay = new WaitForSeconds(_delayBetweenSpawns);
        _currentAvailable = new List<Vector3>(_spawnPoints);
        _spawnByDelay = _startWithAutomaticSpawns;
        _bullets = new BulletPickup[_maxAmmoPickupsInScene];

        for (int i = 0; i < _maxAmmoPickupsInScene; i++)
        {
            _bullets[i] = Instantiate(_bulletPickupPrefab, null).GetComponent<BulletPickup>();
            _bullets[i].UpdateState(false);
            _bullets[i].OnCollect += OnPickupCollect;
            _bullets[i].name = $"BulletPickup {i}";
        }

        _bulletAvailable = _bullets[0];
        for (int i = 0; i < _maxAmmoPickupsInScene - 1; i++) _bullets[i].SetNextInLine(_bullets[i + 1]);
        _bullets[_maxAmmoPickupsInScene - 1].SetNextInLine(null);

        UpdateAutoSpawn();
    }

    private void UpdateAutoSpawn()
    {
        if (_spawnByDelay && _autoSpawnCoroutine == null)
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
            BulletPickup temp = GetBullet();
            temp.SetSpawnPointUsed(GetValidSpawnPoint());
            temp.gameObject.SetActive(true);
            _currentBoxAmount++;
        }
    }

    private Vector3 GetValidSpawnPoint()
    {
        Vector3 temp = _currentAvailable[UnityEngine.Random.Range(0, _currentAvailable.Count)];
        _currentAvailable.Remove(temp);
        return temp;
    }

    private void OnPickupCollect(Vector3 spawnPoint)
    {
        _currentAvailable.Add(spawnPoint);
        _currentBoxAmount--;
        UpdateAutoSpawn();
        Debug.Log("collect");
    }

    private void OnValidate()
    {
        Color[] temp = _colorPoints;
        _colorPoints = new Color[_spawnPoints.Length];
        for (int i = 0; i < _colorPoints.Length; i++)
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

    private BulletPickup GetBullet()
    {
        BulletPickup temp;
        if (_bulletAvailable)
        {
            temp = _bulletAvailable;
            _bulletAvailable = _bulletAvailable.NextInPoolList;
        }
        else
        {
            //dessa forma sepre tera uma sequencia de balas possiveis de serem usadas
            for (int i = 0; i < _maxAmmoPickupsInScene; i++)
            {
                if (!_bullets[i].isActiveAndEnabled)
                {
                    _bullets[i].SetNextInLine(_bulletAvailable);
                    _bulletAvailable = _bullets[i];
                }
            }
            temp = _bulletAvailable;
            if (_bulletAvailable) _bulletAvailable = _bulletAvailable.NextInPoolList;
        }
        return temp;
        //modo padrão de busca por disponivel
        //for(int i = 0; i < _bullets.Length; i++) {
        //    if(!_bullets[i].isActiveAndEnabled) {
        //        temp = _bullets[i];
        //        break;
        //    }
        //}
    }
}
