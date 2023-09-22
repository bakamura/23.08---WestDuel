using UnityEngine;
using System;

public class PlayerShootServer : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    private bool _hasBullet = false;

    [Header("Inputs")]

    [SerializeField] private LayerMask _mouseElevatedReceiver;

    [Header("Cache")]

    private bool _canInput = false;
    private Bullet[] _bulletPool = new Bullet[MaxBulletAmount];
    private Camera _cam;
    public static int MaxBulletAmount = 2;
    public Action OnShoot;

    private void Awake() {
        for(int i = 0; i < _bulletPool.Length; i++) _bulletPool[i] = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();
        _cam = Camera.main;

        _canInput = true; // test
        _hasBullet = true; // test
    }

    public void Shoot(Vector3 mousePos) {
        if (_canInput && _hasBullet) {
            Physics.Raycast(_cam.ScreenPointToRay(mousePos), out RaycastHit hit, Mathf.Infinity, _mouseElevatedReceiver, QueryTriggerInteraction.Collide);

            foreach (Bullet bullet in _bulletPool) {
                if (!bullet.gameObject.activeSelf) {
                    bullet.gameObject.SetActive(true);
                    bullet.Shoot(_firePoint.position, hit.point);
                    //_hasBullet = false;
                    break;
                }
            }
            OnShoot?.Invoke();
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
