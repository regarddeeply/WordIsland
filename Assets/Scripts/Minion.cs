﻿using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [SerializeField] private Transform _target = null;
    [SerializeField] private NavMeshAgent _agent = null;
    [SerializeField] private Animator _animator = null;

    private Vector2 _direction = Vector2.zero;
    public Vector2 Direction
    {
        get => _direction;
        set
        {
            _direction = value;
            _direction.Normalize();
        }
    }

    public float Speed { get; set; } = 0f;

    private void Update()
    {
        _target.position = transform.position + new Vector3(_direction.x, 0f, _direction.y);
        _agent.speed = Speed;
        _agent.SetDestination(_target.position);

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }
}
