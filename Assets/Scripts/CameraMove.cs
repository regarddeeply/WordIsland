using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private bool _active = true;
    [SerializeField] private Transform _target = null;
    [SerializeField] private float _borderRange = 2f;
    [SerializeField] private float _smoothness = 1f;

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    private void Update()
    {
        if (Active)
        {
            float distance = Vector3.Distance(_target.position, transform.position);
            if (distance > _borderRange)
            {
                float myltiplier = Mathf.Clamp01((distance - _borderRange) / 2f);
                transform.position = Vector3.Lerp(transform.position, _target.position, _smoothness * myltiplier * Time.deltaTime);
            }
        }
    }
}
