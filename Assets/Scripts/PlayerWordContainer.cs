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
    [SerializeField] private GameObject _letterBlockPrefab = null;
    [SerializeField] private Transform _content = null;
    [SerializeField] private UILineRenderer _line = null;
    [Header("Game")]
    [SerializeField] private ObjectsController _objectsController = null;

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
            StartCoroutine(GatherWord(word));
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
                InterfaceLetter interfaceLetter = _interfaceLetters.First((il) => il.Letter == _container[i].ToUpper());
                _interfaceLetters.Remove(interfaceLetter);
                interfaceLetter.Delete();
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

        InterfaceLetter interfaceLetter = _interfaceLetters.First((il) => il.Letter == letter.ToUpper());
        _interfaceLetters.Remove(interfaceLetter);
        interfaceLetter.Delete();

        return letter;
    }

    private IEnumerator GatherWord(string word)
    {
        _dictionary[word] = true;

        List<InterfaceLetter> letters = new List<InterfaceLetter>();

        {
            List<InterfaceLetter> interfaceLetters = new List<InterfaceLetter>(_interfaceLetters);

            foreach (var letter in word)
            {
                InterfaceLetter interfaceLetter = interfaceLetters.First((il) => il.Letter == letter.ToString());
                letters.Add(interfaceLetter);
                interfaceLetters.Remove(interfaceLetter);
            }
        }

        yield return null;

        _objectsController.Activate(word);

        _line.enabled = true;
        _line.points = new Vector2[word.Length];
        for (int i = 0; i < _line.points.Length; i++)
            _line.points[i] = letters[0].rectTransform.localPosition;

        for (int i = 1; i < word.Length; i++)
        {
            letters[i - 1].Outlined = true;
            yield return StartCoroutine(
                Utils.CrossFading(
                   letters[i - 1].rectTransform.localPosition,
                   letters[i].rectTransform.localPosition,
                   .1f,
                   (pos) =>
                   {
                       for (int j = i; j < _line.points.Length; j++)
                           _line.points[j] = pos;
                   },
                   (a, b, c) => Vector2.Lerp(a, b, c)
                ));
        }
        letters[letters.Count - 1].Outlined = true;

        yield return new WaitForSeconds(0.5f);

        for (int i = 1; i < word.Length; i++)
        {
            StartCoroutine(Utils.DelayedCall(0.08f, () => Remove(word[i - 1].ToString())));
            yield return StartCoroutine(
                Utils.CrossFading(
                   letters[i - 1].rectTransform.localPosition,
                   letters[i].rectTransform.localPosition,
                   .1f,
                   (pos) =>
                   {
                       for (int j = 0; j < i; j++)
                           _line.points[j] = pos;
                   },
                   (a, b, c) => Vector2.Lerp(a, b, c)
                ));
        }

        yield return StartCoroutine(Utils.DelayedCall(0.08f, () => Remove(word[word.Length - 1].ToString())));

        _line.enabled = false;
    }
}
