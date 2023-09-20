using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsUpdate : MonoBehaviour
{
    [SerializeField] private BoneModificationData[] _bonesToUpdate;
    [SerializeField] private ClientInputReader _inputReader;
    [SerializeField] private float _tickFrequency;
    [SerializeField, Min(1f)] private float _sensitivity;
    [SerializeField, Tooltip("the diference that the current direction and the target direction can be ignored")] private float _minDiferenceToUpdate;
    private float[] _previousValues;
    private WaitForSeconds _delay;
    private bool _canUpdate = true;
    private Vector3 _direction;
    private Vector3 _mousePosition;

    [System.Serializable]
    private struct BoneModificationData
    {
        public GameObject Bone;
        public CalculationMethod BodyPartType;
        public bool ConstrainXAxis;
        public bool ConstrainYAxis;
        public bool ConstrainZAxis;
    }

    private enum CalculationMethod
    {
        DirectionalInput,
        MouseInput
    }

    private void Awake()
    {
        _delay = new WaitForSeconds(_tickFrequency);
        _previousValues = new float[_bonesToUpdate.Length];
        if (_canUpdate) StartCoroutine(UpdateAnimations());
    }

    IEnumerator UpdateAnimations()
    {
        while (_canUpdate)
        {
            Vector3 direction;
            float dotProduct;
            Vector3 axisLocks;

            if (_inputReader)
            {
                SetMousePosition(_inputReader.MousePosition);
                SetDirection(_inputReader.CurrenMovment);
            }

            for (int i = 0; i < _bonesToUpdate.Length; i++)
            {
                axisLocks = new Vector3(_bonesToUpdate[i].ConstrainXAxis ? 0 : 1, _bonesToUpdate[i].ConstrainYAxis ? 0 : 1, _bonesToUpdate[i].ConstrainZAxis ? 0 : 1);
                switch (_bonesToUpdate[i].BodyPartType)
                {
                    case CalculationMethod.DirectionalInput:
                        //direction = new Vector3(_inputReader.CurrenMovment.x, 0, _inputReader.CurrenMovment.y).normalized;
                        direction = new Vector3(_direction.x, 0, _direction.y).normalized;
                        dotProduct = Vector3.Dot(_bonesToUpdate[i].Bone.transform.right, direction);
                        if (Mathf.Abs(dotProduct) > _minDiferenceToUpdate || _previousValues[i] == -dotProduct)
                        {
                            Vector3 result = _bonesToUpdate[i].Bone.transform.eulerAngles + _sensitivity * dotProduct * axisLocks;
                            if (result.y < -90 || result.y > 90)
                            {
                                _bonesToUpdate[i].Bone.transform.root.transform.eulerAngles = new Vector3(
                                    _bonesToUpdate[i].Bone.transform.root.transform.eulerAngles.x, 
                                    180,
                                    _bonesToUpdate[i].Bone.transform.root.transform.eulerAngles.z);
                            }
                            else
                            {
                                _bonesToUpdate[i].Bone.transform.root.transform.eulerAngles = new Vector3(
                                   _bonesToUpdate[i].Bone.transform.root.transform.eulerAngles.x,
                                   0,
                                   _bonesToUpdate[i].Bone.transform.root.transform.eulerAngles.z);
                            }
                            _bonesToUpdate[i].Bone.transform.eulerAngles = result;
                        }
                        //Debug.Log($"previous: {_previousValues[i]}, current {dotProduct}");
                        if (dotProduct != 0) _previousValues[i] = dotProduct;
                        //Debug.Log($"direction: {direction}, dot: {dotProduct}");
                        break;
                    case CalculationMethod.MouseInput:
                        //direction = _inputReader.MousePosition;
                        //dotProduct = Vector3.Dot(_bonesToUpdate[i].Bone.transform.right, direction);
                        //if (Mathf.Abs(dotProduct) > _minDiferenceToUpdate || _previousValues[i] == -dotProduct)
                        //{
                        //    _bonesToUpdate[i].Bone.transform.eulerAngles += _sensitivity * dotProduct * axisLocks;
                        //}
                        ////Debug.Log($"previous: {_previousValues[i]}, current {dotProduct}");
                        //if (dotProduct != 0) _previousValues[i] = dotProduct;
                        ////Debug.Log($"direction: {direction}, dot: {dotProduct}");
                        //direction = _inputReader.MousePosition - _bonesToUpdate[i].Bone.transform.position;
                        direction = _mousePosition - _bonesToUpdate[i].Bone.transform.position;
                        _bonesToUpdate[i].Bone.transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg, axisLocks);
                        //Debug.Log($"direction: {direction}, dot: {dotProduct}");
                        break;
                }
            }
            yield return _delay;
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

    public void UpdateState(bool canUpdate)
    {
        _canUpdate = canUpdate;
    }
}
