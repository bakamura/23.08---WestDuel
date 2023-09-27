using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

    [Header("Parameters")]

    [SerializeField] private byte _healthMax;
    private byte _healthCurrent;
    [SerializeField] private float _delayRespawn;
    [Tooltip("Invencibility duration starts after respawning")]
    [SerializeField] private float _invencibilityDuration;
    private bool _isInvincible = false;

    [Header("Cache")]

    private PlayerMovement _movementScript;
    private PlayerShoot _shootScript;
    private SpawnPlayer _spawnPlayer;
    
    private Transform _oponentTransform;
    private Hud _hud;
    private ServerGameStateSender _gameStateSender;

    private WaitForSeconds _respawnWait;
    private WaitForSeconds _invencibilityWait;

    private void Awake() {
        _movementScript = GetComponent<PlayerMovement>();
        _shootScript = GetComponent<PlayerShoot>();
        _spawnPlayer = FindObjectOfType<SpawnPlayer>();

        PlayerHealth[] players = FindObjectsOfType<PlayerHealth>();
        foreach (PlayerHealth player in players) if (player != this) _oponentTransform = player.transform;
        _hud = FindObjectOfType<Hud>();

        _respawnWait = new WaitForSeconds(_delayRespawn);
        _invencibilityWait = new WaitForSeconds(_invencibilityDuration);

        _healthCurrent = _healthMax;
    }

    public void Die() {
        if (!_isInvincible) StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine() {
        _isInvincible = true;
        _movementScript.SetActive(false);
        _shootScript.SetActive(false);

        _healthCurrent--;
        _hud.UpdateHealth();
        _gameStateSender.UpdateHealth();
        if (_healthCurrent <= 0) {
            // Call game manager admiting defeat

            yield break;
        }

        yield return _respawnWait;

        transform.position = _spawnPlayer.GetPointFurthestFromOponent(_oponentTransform.position);
        _movementScript.SetActive(true);
        _shootScript.SetActive(true);

        yield return _invencibilityWait;

        _isInvincible = false;

    }

    public byte GetCurrentHealth() { return _healthCurrent; }

    public void ResetHealth() {
        _healthCurrent = _healthMax;
    }
}
