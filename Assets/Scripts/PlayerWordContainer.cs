using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWordContainer : WordContainer
{
    [Header("Settings")]
    [SerializeField] private int _containerSize = 6;
    [SerializeField] private WordConstructor _constructor = null;
    [Header("Interface")]
    [SerializeField] private Storage _storage = null;
    [Header("Game")]
    [SerializeField] private ObjectsController _objectsController = null;

    private Dictionary<string, bool> _dictionary = null;

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
        if (IsFull) return;
        for (int i = 0; i < _container.Length; i++)
        {
            if (string.IsNullOrEmpty(_container[i]))
            {
                _container[i] = letter;
                _storage.AddLetter(letter);
                break;
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
    }

    private string CheckWords()
    {
        string[] letters = _container.Where((letter) => !string.IsNullOrEmpty(letter)).ToArray();
        string[] words = _constructor.CheckWords(letters);

        foreach (var word in words)
            if (!_dictionary[word]) return word;
        return string.Empty;
    }
}
