using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _tickFrequency;
    [SerializeField] private float _speed;
    //shoot sound
    [SerializeField] private SoundManager.SfxAudioData _audioData;

    [HideInInspector] public Transform owner;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void Shoot(Vector3 initialPos, Vector3 direction)
    {
        transform.position = initialPos;
        if (this.isActiveAndEnabled) _rb.velocity = direction * _speed;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.collider == _adversaryPlayerCol) _adversaryPlayerHealth.Die();
        PlayerHealth temp = collision.collider.GetComponent<PlayerHealth>();
        if (collision.transform != owner && temp)
        {
            temp.Die();
        }
        UpdateState(false);
    }

    public void UpdateState(bool isActive)
    {
        if (isActive) SoundManager.Instance.PlaySoundEffect(_audioData, transform.position);
        else _rb.velocity = Vector3.zero;
        gameObject.SetActive(isActive);
    }

}