using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPickup : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private Vector3[] _spawnPoints;
    [SerializeField] private float _delayBetweenSpawns;
    //[SerializeField] private GameObject _bulletPickupPrefab;
    [SerializeField] private byte _maxAmmoPickupsInScene = 2;

    [Header("Debug")]
    [SerializeField] private bool _startWithAutomaticSpawns = true;
    [SerializeField] private bool _debugMode;
    [SerializeField] private Color[] _colorPoints;
    [SerializeField] private float _gizmoSize;

    private WaitForSeconds _delay;
    private bool _spawnByDelay = true;
    private byte _currentBoxAmount;
    private List<Vector3> _currentAvailable;
    private Coroutine _autoSpawnCoroutine;
    private BulletPickup[] _bulletPickups;
    private BulletPickup _bulletAvailable;

    private void Awake()
    {
        _delay = new WaitForSeconds(_delayBetweenSpawns);
        _currentAvailable = new List<Vector3>(_spawnPoints);
        _spawnByDelay = _startWithAutomaticSpawns;
        _bulletPickups = new BulletPickup[_maxAmmoPickupsInScene];

        for (int i = 0; i < _maxAmmoPickupsInScene; i++)
        {
            _bulletPickups[i] = Instantiate(InstantiateHandler.GetBulletPrefab(), null).GetComponent<BulletPickup>();
            _bulletPickups[i].UpdateState(false);
            _bulletPickups[i].OnCollect += OnPickupCollect;
            _bulletPickups[i].name = $"BulletPickup {i}";
        }

        _bulletAvailable = _bulletPickups[0];
        for (int i = 0; i < _maxAmmoPickupsInScene - 1; i++) _bulletPickups[i].SetNextInLine(_bulletPickups[i + 1]);
        _bulletPickups[_maxAmmoPickupsInScene - 1].SetNextInLine(null);

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

    public void SetSpawnAutomatic(bool activate)
    {
        _spawnByDelay = activate;
        UpdateAutoSpawn();
    }

    public BulletPickup Spawn()
    {
        if (_currentBoxAmount < _maxAmmoPickupsInScene)
        {
            BulletPickup temp = GetBullet();
            temp.SetSpawnPointUsed(GetValidSpawnPoint());
            temp.gameObject.SetActive(true);
            _currentBoxAmount++;
            return temp;
        }
        return null;
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

#if UNITY_EDITOR
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
#endif

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
                if (!_bulletPickups[i].isActiveAndEnabled)
                {
                    _bulletPickups[i].SetNextInLine(_bulletAvailable);
                    _bulletAvailable = _bulletPickups[i];
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
