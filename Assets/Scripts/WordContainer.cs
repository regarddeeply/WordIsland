using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordContainer : MonoBehaviour, IWordContainer
{
    [Header("Settings")]
    [SerializeField] protected int _containerSize = 6;
    [SerializeField] protected WordConstructor _constructor = null;

    [Header("Game")]
    [SerializeField] protected ObjectsController _objectsController = null;

    protected string[] _container = new string[0];
    protected Dictionary<string, bool> _dictionary = null;
    public virtual bool IsFull => _container.Where((letter) => string.IsNullOrEmpty(letter)).Count() == 0;

    public int LetterCount => _container.Length;

    public int AllLetters => LetterCount + _dictionary.Where((pair) => pair.Value).Sum((pair) => pair.Key.Length);

    public string GetLetter(int id) => _container[id];

    public bool HasWord(string word)
    {
        if (_dictionary.ContainsKey(word)) return !_dictionary[word];
        return false;
    }

    private void Awake()
    {
        _container = new string[_containerSize];
        for (int i = 0; i < _container.Length; i++) _container[i] = string.Empty;

        _dictionary = new Dictionary<string, bool>();
        foreach (var word in _constructor.Data.Words)
            _dictionary.Add(word, false);
    }

    public virtual void AddLetter(string letter)
    {
        if (IsFull) return;
        for (int i = 0; i < _container.Length; i++)
        {
            if (string.IsNullOrEmpty(_container[i]))
            {
                _container[i] = letter;
                break;
            }
        }
        string word = CheckWords();
        if (!string.IsNullOrEmpty(word))
        {
            _dictionary[word] = true;
            _objectsController.Activate(word);
            foreach (var symbol in word)
                Remove(symbol.ToString());
        }
    }

    public virtual void Remove(string letter)
    {
        for (int i = 0; i < _container.Length; i++)
        {
            if (_container[i].ToUpper() == letter.ToUpper())
            {
                _container[i] = string.Empty;
                break;
            }
        }
    }

    public virtual string RemoveAt(int id)
    {
        string letter = string.Empty;
        if (id < 0 || id >= _container.Length) return letter;
        letter = _container[id];
        _container[id] = string.Empty;
        return letter;
    }

    protected string CheckWords()
    {
        string[] letters = _container.Where((letter) => !string.IsNullOrEmpty(letter)).ToArray();
        string[] words = _constructor.CheckWords(letters);

        foreach (var word in words)
            if (!_dictionary[word]) return word;
        return string.Empty;
    }
}
