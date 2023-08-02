using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    private bool _hasBullet = false;

    [Header("Inputs")]

    [SerializeField] private KeyCode _keyShoot;
    [SerializeField] private LayerMask _mouseElevatedReceiver;

    [Header("Cache")]

    private bool _canInput = false;
    private Bullet[] _bulletPool = new Bullet[2];
    private Camera _cam;

    private void Awake() {
        for(int i = 0; i < _bulletPool.Length; i++) _bulletPool[i] = Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity).GetComponent<Bullet>();
        _cam = Camera.main;

        _canInput = true; // test
        _hasBullet = true; // test
    }

    private void Update() {
        if (_canInput && _hasBullet && Input.GetKeyDown(_keyShoot)) Shoot();
    }

    private void Shoot() {
        Physics.Raycast(_cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, _mouseElevatedReceiver, QueryTriggerInteraction.Collide);

        foreach (Bullet bullet in _bulletPool) {
            if (!bullet.gameObject.activeSelf) {
                bullet.gameObject.SetActive(true);
                bullet.Shoot(_firePoint.position, hit.point);
                //_hasBullet = false;
                break;
            }
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
