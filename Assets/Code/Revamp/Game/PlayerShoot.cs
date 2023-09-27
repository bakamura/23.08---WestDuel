using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerShoot : MonoBehaviour {

    public UnityEvent<PlayerShoot> onShoot = new UnityEvent<PlayerShoot>();

    [Header("Parameters")]

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    private bool _hasBullet = false;
    private bool _canInput = false;

    [Header("Cache")]

    public const int MaxBulletAmount = 2;
    private Bullet[] _bulletPool = new Bullet[MaxBulletAmount];
    private Camera _cam;

    private void Awake() {
        for (int i = 0; i < _bulletPool.Length; i++) {
            _bulletPool[i] = Instantiate(_bulletPrefab, Vector3.up * 256f, Quaternion.identity).GetComponent<Bullet>();
            _bulletPool[i].owner = transform;
        }
        _cam = Camera.main;

        _canInput = true; // Should be called by game initiator
        _hasBullet = true; // Should be called by game initiator
    }

    public void Shoot(Vector3 mouseWorldPos) {
        if (_canInput && _hasBullet) {
            foreach (Bullet bullet in _bulletPool) {
                if (!bullet.gameObject.activeSelf) {
                    bullet.gameObject.SetActive(true); // Not needed?
                    bullet.Shoot(_firePoint.position, mouseWorldPos);
                    _hasBullet = false;
                    break;
                }
            }
            onShoot?.Invoke(this);
        }
    }

    public void SetActive(bool isActive) {
        _canInput = isActive;
    }

    public bool CheckBullet() {
        return _hasBullet;
    }

    public void GetBullet() {
        _hasBullet = true;
    }

}
