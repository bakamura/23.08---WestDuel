using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientInputReader : MonoBehaviour
{
    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyShoot;

    private Vector2 _input;
    private Vector3 _mousePosition;
    private bool _mouseClick;
    private Camera _camera;
    public Vector2 CurrenMovment => _input;
    public bool MouseClick => _mouseClick;
    public Vector3 MousePosition => _mousePosition;

    private void Awake()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        _input[0] = (Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0);
        _input[1] = (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0);

        _mouseClick = Input.GetKeyDown(_keyShoot);

        if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit))
        {
            _mousePosition = raycastHit.point;
        }
    }
}
