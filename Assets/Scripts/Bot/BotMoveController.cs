using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Minion))]
public class BotMoveController : MonoBehaviour
{
    [SerializeField] private Minion _minion = null;
    [SerializeField] private WordConstructor _constructor = null;
    [SerializeField] private WordContainer _container = null;
    [SerializeField] private bool _active = true;
    [Range(0f, 1f)]
    [SerializeField] private float _chooseLetterChance = 0.2f;
    [SerializeField] private float _speed = 3f;

    public bool Active
    {
        get => _active;
        set => _active = value;
    }

    private Vector3 _targetPoint = Vector3.zero;
    private Transform _targetLetter = null;
    private bool _busy = false;
    private float _timer = 0f;
    private readonly float _delay = 1f;

    private void Update()
    {
        if (Active)
        {
            _minion.Speed = _speed;

            if (_busy)
            {
                if (_targetLetter == null || Vector3.Distance(transform.position, _targetLetter.position) < 0.5f)
                    ChoosePoint();
                _minion.point = _targetLetter.position;
            }
            else
            {
                _minion.point = _targetPoint;
                _timer += Time.deltaTime;
                if (_timer >= _delay || Vector3.Distance(transform.position, _targetPoint) < 0.5f)
                    ChoosePoint();
            }
        }
        else
        {
            _minion.Speed = 0f;
            _minion.point = transform.position;
        }
    }

    private void ChoosePoint()
    {
        if (Random.Range(0f, 1f) < _chooseLetterChance)
        {
            if (_container.LetterCount == 0)
            {
                _targetLetter = GetNearestLetter();
            }
            else
            {
                _targetLetter = null;
                string[] words = _constructor.Data.Words.Where((word) => word.Contains(_container.GetLetter(0)) && _container.HasWord(word)).ToArray();
                if (words != null && words.Length > 0)
                    _targetLetter = GetNearestLetter(words[0]);
            }
            _busy = true;
        }
        else
        {
            _targetPoint = GetRandomPoint();
            _timer = 0f;
            _busy = false;
        }
    }

    private Transform GetNearestLetter()
    {
        Transform[] letters = FindObjectsOfType<LetterBlock>().Select((lb) => lb.transform).ToArray();
        Transform target = null;
        float min = float.MaxValue;
        foreach (var letter in letters)
        {
            float distance = Vector3.Distance(transform.position, letter.position);
            if (distance < min)
            {
                min = distance;
                target = letter;
            }
        }

        return target;
    }

    private Transform GetNearestLetter(string word)
    {
        Transform[] letters = FindObjectsOfType<LetterBlock>().Where((lb) => word.Contains(lb.Letter)).Select((lb) => lb.transform).ToArray();
        Transform target = null;
        float min = float.MaxValue;
        foreach (var letter in letters)
        {
            float distance = Vector3.Distance(transform.position, letter.position);
            if (distance < min)
            {
                min = distance;
                target = letter;
            }
        }

        return target;
    }

    private Vector3 GetRandomPoint()
    {
        return transform.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized * 5f;
    }
}
