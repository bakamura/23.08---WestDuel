using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsUpdate : MonoBehaviour
{
    [SerializeField] private BoneModificationData[] _bonesToUpdate;
    [SerializeField] private ServerInputReader _inputReader;
    [SerializeField] private float _tickFrequency;
    [SerializeField, Min(1f)] private float _sensitivity;
    [SerializeField, Tooltip("the diference that the current direction and the target direction can be ignored")] private float _minDiferenceToUpdate;
    private WaitForSeconds _delay;
    private bool _canUpdate = true;
    private Vector3 _direction;
    private Vector3 _mousePosition;
    private Animator _animator;
    private bool _canShoot;
    private Coroutine _updateCoroutine;
    bool _fliped = false;

    [System.Serializable]
    private struct BoneModificationData
    {
        public GameObject Bone;
        public CalculationMethod BodyPartType;
        public bool ConstrainXAxis;
        public bool ConstrainYAxis;
        public bool ConstrainZAxis;
        [Range(0f, 360f)] public float MinAngleToFlip;
        [Range(0f, 360f)] public float MaxAngleToFlip;
    }

    private enum CalculationMethod
    {
        DirectionalInput,
        MouseInput
    }

    private void Awake()
    {
        _delay = new WaitForSeconds(_tickFrequency);
        _animator = GetComponent<Animator>();
        if (_inputReader)
        {
            PlayerShootServer temp = GetComponentInChildren<PlayerShootServer>();
            if (temp)
            {
                temp.OnShoot += TriggerShootAnim;
            }
            else
            {
                Debug.LogWarning("there is no PlayerShootServer script to callback the Shoot animation");
            }
        }
        if (_canUpdate) _updateCoroutine = StartCoroutine(UpdateAnimations());
    }

    IEnumerator UpdateAnimations()
    {
        while (_canUpdate)
        {
            Vector3 direction = Vector3.zero;
            float dotProduct;
            Vector3 axisLocks;
            Vector3 result;

            if (_inputReader)
            {
                SetMousePosition(_inputReader.MousePosition);
                SetDirection(_inputReader.Direction);
            }

            for (int i = 0; i < _bonesToUpdate.Length; i++)
            {
                axisLocks = new Vector3(_bonesToUpdate[i].ConstrainXAxis ? 0 : 1, _bonesToUpdate[i].ConstrainYAxis ? 0 : 1, _bonesToUpdate[i].ConstrainZAxis ? 0 : 1);
                switch (_bonesToUpdate[i].BodyPartType)
                {
                    case CalculationMethod.DirectionalInput:
                        direction = new Vector3(_direction.x, 0, _direction.y).normalized;
                        dotProduct = Vector3.Dot(_bonesToUpdate[i].Bone.transform.right, direction);
                        if (Mathf.Abs(dotProduct) > _minDiferenceToUpdate)
                        {
                            result = _bonesToUpdate[i].Bone.transform.eulerAngles + _sensitivity * dotProduct * axisLocks;

                            if (result.y > _bonesToUpdate[i].MinAngleToFlip && result.y < _bonesToUpdate[i].MaxAngleToFlip)
                            {
                                _fliped = true;
                                Debug.Log($"true {result.y}");
                            }
                            else
                            {
                                _fliped = false;
                                Debug.Log($"false {result.y}");
                            }
                            _bonesToUpdate[i].Bone.transform.eulerAngles = result;
                        }
                        //Debug.Log($"previous: {_previousValues[i]}, current {dotProduct}");
                        //Debug.Log($"direction: {direction}, dot: {dotProduct}, rigth: {_bonesToUpdate[i].Bone.transform.right}");
                        break;
                    case CalculationMethod.MouseInput:
                        direction = (_mousePosition - _bonesToUpdate[i].Bone.transform.position).normalized;
                        dotProduct = Vector3.Dot(_bonesToUpdate[i].Bone.transform.right, direction);
                        result = _bonesToUpdate[i].Bone.transform.eulerAngles + _sensitivity * dotProduct * axisLocks;
                        _bonesToUpdate[i].Bone.transform.eulerAngles = result;
                        //direction = _mousePosition - _bonesToUpdate[i].Bone.transform.position;
                        //angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                        //_bonesToUpdate[i].Bone.transform.rotation = Quaternion.AngleAxis(Mathf.Clamp(angle * Mathf.Rad2Deg, -90, 90), axisLocks);
                        //Debug.Log($"direction: {direction}, dot: {dotProduct}");
                        break;
                }
            }
            if (_fliped)
            {
                //_bonesToUpdate[0].Bone.transform.root.transform.localScale = new Vector3(_bonesToUpdate[0].Bone.transform.root.transform.localScale.x, _bonesToUpdate[0].Bone.transform.root.transform.localScale.y, -1);
                _bonesToUpdate[0].Bone.transform.root.transform.eulerAngles = new Vector3(
                                    _bonesToUpdate[0].Bone.transform.root.transform.eulerAngles.x,
                                    180,
                                    _bonesToUpdate[0].Bone.transform.root.transform.eulerAngles.z);
            }
            else
            {
                //_bonesToUpdate[0].Bone.transform.root.transform.localScale = new Vector3(_bonesToUpdate[0].Bone.transform.root.transform.localScale.x, _bonesToUpdate[0].Bone.transform.root.transform.localScale.y, 1);
                _bonesToUpdate[0].Bone.transform.root.transform.eulerAngles = new Vector3(
                                    _bonesToUpdate[0].Bone.transform.root.transform.eulerAngles.x,
                                    0,
                                    _bonesToUpdate[0].Bone.transform.root.transform.eulerAngles.z);
            }
            _animator.SetFloat("VELOCITY", direction.sqrMagnitude);
            if (_canShoot)
            {
                _animator.SetTrigger("SHOOT");
                _canShoot = false;
            }
            yield return _delay;
        }
        _updateCoroutine = null;
    }

    private float GetAngleInsideCircule(float val)
    {
        float temp = val + 180;
        if (temp > 360)
        {
            return temp - 360;
        }
        else
        {
            return temp;
        }
    }

    public void SetDirection(Vector3 pos)
    {
        _direction = pos;
    }

    public void SetMousePosition(Vector3 pos)
    {
        _mousePosition = pos;
    }

    public void TriggerShootAnim()
    {
        _canShoot = true;
    }

    public Vector3 GetMousePosition()
    {
        return _mousePosition;
    }

    public Vector3 GetDirection()
    {
        return _direction;
    }

    public void UpdateState(bool canUpdate)
    {
        _canUpdate = canUpdate;
        if (_updateCoroutine == null && canUpdate)
        {
            _updateCoroutine = StartCoroutine(UpdateAnimations());
        }
    }
}
