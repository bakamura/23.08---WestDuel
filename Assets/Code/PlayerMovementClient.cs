using UnityEngine;

[RequireComponent(typeof(PlayerShootClient), typeof(Rigidbody))]
public class PlayerMovementClient : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _noBulletSpeed;

    [Header("Cache")]

    private bool _canInput = false;
    private Vector2 _input = Vector2.zero;
    private Rigidbody _rb;
    private PlayerShootClient _shootScript;
    private Transform _camTransform;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _shootScript = GetComponent<PlayerShootClient>();
        _camTransform = Camera.main.transform;

        _canInput = true; //testing
    }

    private void Update() {
        if (_canInput && _input != Vector2.zero) Move(_input.normalized); 
    }

    public void SetInputDirection(Vector2 direction) {
        _input = direction;
    }

    private void Move(Vector2 direction) {
        _rb.velocity = (Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _camTransform.eulerAngles.y, 0) * Vector3.forward).normalized 
                       * (_shootScript.CheckBullet() ? _baseSpeed : _noBulletSpeed); 
    }

    public void SetActive(bool isActive) {
        _canInput = isActive;
    }

}
