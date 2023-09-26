using UnityEngine;

public class ServerLocalInputReader : MonoBehaviour {

    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyShoot;

    [SerializeField] private LayerMask _mouseRaycastLayer;

    private Vector2 _movementInput;
    private Vector3 _shootInput;

    public Vector2 MovementInput { get { return _movementInput; } }
    public Vector3 ShootInput { get { return _shootInput; } }

    [Header("Cache")]

    private Camera _camera;

    private void Start() {
        _camera = Camera.main;
    }

    private void Update() {
        _movementInput.Set((Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0),
                           (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0));

        if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, Mathf.Infinity, _mouseRaycastLayer)) {
            _shootInput = raycastHit.point;
        }
        else _shootInput = Vector3.up * 256f;

        if(Input.GetKeyDown(_keyShoot)) {
            if (_shootInput != Vector3.up * 256f) ServerPlayerInfo.player[ConnectionHandler.serverIpEp].shoot.Shoot();
            else Debug.LogWarning("Mouse not hitting shit");
        }
    }

}
