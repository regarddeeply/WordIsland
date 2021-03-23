using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWordContainer : WordContainer
{
    [Header("Settings")]
    [SerializeField] private int _containerSize = 6;
    [SerializeField] private WordConstructor _constructor = null;
    [Header("Interface")]
    [SerializeField] private GameObject _letterBlockPrefab = null;
    [SerializeField] private Transform _content = null;

    private Dictionary<string, bool> _dictionary = null;
    private List<InterfaceLetter> _interfaceLetters = new List<InterfaceLetter>();

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
                InterfaceLetter il = Instantiate(_letterBlockPrefab, _content).GetComponent<InterfaceLetter>();
                il.Letter = letter;
                _interfaceLetters.Add(il);
                break;
            }
        }
        string word = CheckWords();
        if (!string.IsNullOrEmpty(word))
        {
            foreach (var l in word) Remove(l.ToString());
            _dictionary[word] = true;
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

    public override void Remove(string letter)
    {
        for (int i = 0; i < _container.Length; i++)
        {
            if (_container[i].ToUpper() == letter.ToUpper())
            {
                _interfaceLetters.First((il) => il.Letter == _container[i].ToUpper()).Delete();
                _container[i] = string.Empty;
                break;
            }
        }
    }

    public override string RemoveAt(int id)
    {
        string letter = string.Empty;
        if (id < 0 || id >= _container.Length) return letter;
        letter = _container[id];
        _container[id] = string.Empty;
        return letter;
    }
}
