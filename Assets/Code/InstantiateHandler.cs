using UnityEngine;

public static class InstantiateHandler {

    private static GameObject _player1ClientPrefab;
    private static GameObject _player2ClientPrefab;
    private static GameObject _player1HostPrefab;
    private static GameObject _player2HostPrefab;
    private static GameObject _ammoBoxPrefab;
    private static GameObject _bulletPrefab;

    static InstantiateHandler() {
        _player1ClientPrefab = Resources.Load<GameObject>("Prefab/Player1 Instance Client");
        _player2ClientPrefab = Resources.Load<GameObject>("Prefab/Player2 Instance Client");
        _player1HostPrefab = Resources.Load<GameObject>("Prefab/Player1 Instance Host");
        _player2HostPrefab = Resources.Load<GameObject>("Prefab/Player2 Instance Host");
        _ammoBoxPrefab = Resources.Load<GameObject>("Prefab/AmmoBox");
        _bulletPrefab = Resources.Load<GameObject>("Prefab/Bullet");
    }

    public static GameObject GetPlayer1ClientPrefab() {
        return _player1ClientPrefab;
    }

    public static GameObject GetPlayer2ClientPrefab() {
        return _player2ClientPrefab;
    }

    public static GameObject GetPlayer1HostPrefab() {
        return _player1HostPrefab;   
    }                                  
                                       
    public static GameObject GetPlayer2HostPrefab() {
        return _player2HostPrefab;
    }

    public static GameObject GetAmmoBoxPrefab() {
        return _ammoBoxPrefab;
    }

    public static GameObject GetBulletPrefab() {
        return _bulletPrefab;
    }
}
