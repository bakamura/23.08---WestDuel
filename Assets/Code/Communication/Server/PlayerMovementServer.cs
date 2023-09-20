using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementServer : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _noBulletSpeed;

    [Header("Cache")]

    private bool _canInput = false;
    private Vector2 _input = Vector2.zero;
    private Rigidbody _rb;
    private PlayerShootServer _shootScript;
    private Transform _camTransform;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _shootScript = GetComponent<PlayerShootServer>();
        _camTransform = Camera.main.transform;

        _canInput = true; //testing
    }

    private void Update() {
        Move(_input.normalized);
    }

    public void SetInputDirection(Vector2 direction) {
        _input = direction;
    }

    private void Move(Vector2 direction) {
        _rb.velocity = _canInput ?
            (Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _camTransform.eulerAngles.y, 0) * Vector3.forward).normalized * (_shootScript.CheckBullet() ? _baseSpeed : _noBulletSpeed) :
            Vector3.zero;
    }

    public void SetActive(bool isActive) {
        _canInput = isActive;
    }

}
