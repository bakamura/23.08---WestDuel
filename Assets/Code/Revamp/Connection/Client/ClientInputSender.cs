using UnityEngine;

public class ClientInputSender : MonoBehaviour
{
    private InputDataPack _dataPackCache = new InputDataPack();
    
    [Header("Inputs")]

    [SerializeField] private KeyCode _keyForward;
    [SerializeField] private KeyCode _keyRight;
    [SerializeField] private KeyCode _keyBackward;
    [SerializeField] private KeyCode _keyLeft;
    [SerializeField] private KeyCode _keyShoot;

    [SerializeField] private LayerMask _mouseRaycastLayer;

    [Header("Cache")]

    private Camera _camera;
    private Vector2 _movementInputCache;
    private Vector3 _shootInput;

    private void Start() {
        _camera = Camera.main;
    }

    private void Update()
    {
        PreparePack();
    }

    private void PreparePack()
    {
        // Could be set directly in cache
        _movementInputCache.Set((Input.GetKey(_keyLeft) ? -1 : 0) + (Input.GetKey(_keyRight) ? 1 : 0),
                                (Input.GetKey(_keyBackward) ? -1 : 0) + (Input.GetKey(_keyForward) ? 1 : 0));
        if(Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, Mathf.Infinity, _mouseRaycastLayer)) {
            _shootInput = raycastHit.point;
        }
        else _shootInput = Vector3.up * 256f;


        _dataPackCache.movementInput = PackingUtility.Vector2ToFloatArray(_movementInputCache);
        _dataPackCache.shootPoint = PackingUtility.Vector3ToFloatArray(_shootInput);
        _dataPackCache.shootTrigger = Input.GetKeyDown(_keyShoot);

        DataSendHandler.SendPack(_dataPackCache, ConnectionHandler.serverIpEp);
    }
}
