using UnityEngine;
using System;

public class ServerInputReader : MonoBehaviour {

    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyShoot;

    private Vector2 _input;
    private Vector3 _mousePosition;
    private Camera _camera;

    public Vector3 MousePosition { get {return _mousePosition;} }
    public Vector3 Direction { get { return _input; } }

    private void Awake() {
        _camera = Camera.main;
    }

    void Update() {
        // Movement
        _input.Set((Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0),
                   (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0));

        ServerConnectionHandler.players[0].movement.SetInputDirection(_input);

        // Aiming
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit)) {
            _mousePosition = raycastHit.point;
        }
        else {
            _mousePosition = Vector3.up * 256f;
            Debug.Log("Mouse not hitting shit");
        }

        // Shooting
        if (Input.GetKeyDown(_keyShoot) && _mousePosition.y < 128f) ServerConnectionHandler.players[0].shoot.Shoot(_mousePosition);

    }
}
