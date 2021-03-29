﻿using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour
{
    [SerializeField] private bool _usePointNavigation = false;
    [SerializeField] private Transform _target = null;
    [SerializeField] private NavMeshAgent _agent = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private WordContainer _wordContainer = null;
    [SerializeField] private GameObject _letteBlockPrefab = null;

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

    public Vector3 point = Vector3.zero;

    public float Speed
    {
        get => _agent.speed;
        set
        {
            _agent.speed = value;
            _agent.acceleration = value * 10f;
        }
    }

    private bool _victory = false;
    public bool Victory
    {
        get => _victory;
        set
        {
            _victory = value;
            _animator.SetBool("Victory", value);
        }
    }

    private bool _defeat = false;
    public bool Defeat
    {
        get => _defeat;
        set
        {
            _defeat = value;
            _animator.SetBool("Defeat", value);
        }
    }

    private void Start()
    {
        Speed = 0f;
    }

    private void Update()
    {
        if (Victory || Defeat)
        {
            _agent.Warp(transform.position);
            _agent.isStopped = true;
            _agent.speed = 0f;
        }
        else
        {

            if (_usePointNavigation)
            {
                _agent.SetDestination(point);
            }
            else
            {
                _target.position = transform.position + new Vector3(_direction.x, 0f, _direction.y);
                _agent.SetDestination(_target.position);
                if (Direction != Vector2.zero && Speed != 0f)
                    transform.LookAt(_target.position);
            }

            _animator.SetFloat("Speed", _agent.velocity.magnitude);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out LetterBlock letterBlock))
        {
            if (letterBlock.Active)
            {
                _wordContainer.AddLetter(letterBlock.Letter);
                letterBlock.Delete();
            }
        }
    }

    public void DropLetter(string letter)
    {
        LetterBlock block = Instantiate(_letteBlockPrefab, null).GetComponent<LetterBlock>();
        block.transform.position = transform.position - transform.forward;

        block.Letter = letter;
        _wordContainer.Remove(letter);
    }
}
