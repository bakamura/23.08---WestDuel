using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private float _baseSpeed;
    [SerializeField] private float _noBulletSpeed;

    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;

    [Header("Cache")]

    private bool _canInput = false;
    private Vector2 _input = Vector2.zero;
    private Rigidbody _rb;
    private PlayerShoot _shootScript;
    private Transform _camTransform;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        _shootScript = GetComponent<PlayerShoot>();
        _camTransform = Camera.main.transform;

        _canInput = true; //testing
    }

    private void Update() {
        if (_canInput) {
            _input[0] = (Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0);
            _input[1] = (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0);
            if(_input.sqrMagnitude > 0) Move(_input.normalized); // does it need to be normalized?
        }
    }

    private void Move(Vector2 direction) {
        _rb.velocity = (Quaternion.Euler(0, Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + _camTransform.eulerAngles.y, 0) * Vector3.forward).normalized 
                       * (_shootScript.CheckBullet() ? _baseSpeed : _noBulletSpeed); 
    }

    public void SetActive(bool isActive) {
        _canInput = isActive;
    }

}
