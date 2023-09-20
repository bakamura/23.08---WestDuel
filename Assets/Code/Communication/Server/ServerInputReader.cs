using UnityEngine;
using System;

public class ServerInputReader : MonoBehaviour
{
    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyShoot;

    private Vector2 _input;
    private Vector3 _mousePosition;
    private Camera _camera;
    public Action OnShoot;

    public Vector3 MousePosition => _mousePosition;
    public Vector2 Direction => _input;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        _input.Set((Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0),
                   (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0));
        //ServerConnectionHandler.players[0].movement.SetInputDirection(_input);
        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
        {
            _mousePosition = raycastHit.point;
        }
        if (Input.GetKeyDown(_keyShoot))
        {
            ServerConnectionHandler.players[0].shoot.Shoot(_mousePosition);
            OnShoot?.Invoke();
        }
    }
}
