using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWordContainer : WordContainer
{
    [Header("Interface")]
    [SerializeField] private Storage _storage = null;
    [Header("Victory")]
    [SerializeField] private Vector3 _minionPosition = Vector3.zero;
    [SerializeField] private GameObject[] _confetties = null;

    private void Awake()
    {
        _container = new string[_containerSize];
        for (int i = 0; i < _container.Length; i++) _container[i] = string.Empty;

        _dictionary = new Dictionary<string, bool>();
        foreach (var word in _constructor.Data.Words)
            _dictionary.Add(word, false);
    }

    public override void AddLetter(string letter)
    {
        if (IsFull)
        {
            _storage.DropLetter(0);
            for (int i = 0; i < _container.Length - 1; i++)
                _container[i] = _container[i + 1];
            _container[_container.Length - 1] = letter;
            _storage.AddLetter(letter);
        }
        else
        {
            for (int i = 0; i < _container.Length; i++)
            {
                if (string.IsNullOrEmpty(_container[i]))
                {
                    _container[i] = letter;
                    _storage.AddLetter(letter);
                    break;
                }
            }
        }
        string word = CheckWords();
        if (!string.IsNullOrEmpty(word))
        {
            _dictionary[word] = true;
            _storage.SelectWord(word);
            _objectsController.Activate(word);
            foreach (var symbol in word)
                Remove(symbol.ToString());
        }
        if (_dictionary.Where((pair) => !pair.Value).Count() == 0)
        {
            StartCoroutine(Utils.DelayedCall(1f, () =>
            {
                foreach (var minion in FindObjectsOfType<Minion>())
                {
                    if (minion.tag.Equals("Player"))
                    {
                        minion.Victory = true;
                        minion.transform.position = _minionPosition;
                        minion.transform.LookAt(Vector3.back);
                    }
                    else minion.Defeat = true;
                }
                foreach (var confeti in _confetties)
                    confeti.SetActive(true);
            }));
        }
    }
}
