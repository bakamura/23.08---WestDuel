using UnityEngine;

public class ServerInputReader : MonoBehaviour {
    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyShoot;

    private Vector2 _input;
    private Camera _camera;

    private void Awake() {
        _camera = Camera.main;
    }

    void Update() {
        _input.Set((Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0),
                   (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0));
        ServerConnectionHandler.players[0].movement.SetInputDirection(_input);

        if (Input.GetKeyDown(_keyShoot)) ServerConnectionHandler.players[0].shoot.Shoot(_camera.ScreenToWorldPoint(Input.mousePosition));
    }
}
