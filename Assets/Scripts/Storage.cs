using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _size = 6;
    [SerializeField] private GameObject _slotPrefab = null;
    [SerializeField] private GameObject _interfaceLetterPrefab = null;
    [SerializeField] private Transform _unselectedContent = null;
    [SerializeField] private Transform _selectedContent = null;
    [SerializeField] private float _smoothness = 6f;
    [SerializeField] private Minion _minion = null;

    private List<InterfaceLetter> _letters = new List<InterfaceLetter>();
    private Transform[] _unselectedSlots = null;
    private Transform[] _selectedSlots = null;

    private void Start()
    {
        _unselectedSlots = new Transform[_size];
        for (int i = 0; i < _size; i++)
            _unselectedSlots[i] = Instantiate(_slotPrefab, _unselectedContent).transform;
    }

    private void Update()
    {
        for (int i = 0; i < _letters.Count; i++)
        {
            InterfaceLetter letter = _letters[i];
            Vector3 from = letter.transform.position;
            Vector3 to = _unselectedSlots[i].position;

            if (_letters[i].Outlined)
                to = _selectedSlots[letter.SelectionIndex].position;

            letter.transform.position = Vector3.Lerp(from, to, _smoothness * Time.deltaTime);
        }
    }

    public void AddLetter(string letter)
    {
        InterfaceLetter interfaceLetter = Instantiate(_interfaceLetterPrefab, transform).GetComponent<InterfaceLetter>();
        interfaceLetter.Letter = letter;
        _letters.Add(interfaceLetter);
        int index = _letters.IndexOf(interfaceLetter);
        interfaceLetter.transform.position = _unselectedSlots[index].position;
        interfaceLetter.Button.onClick.AddListener(() => DropLetter(interfaceLetter));
    }

    public void DropLetter(int index)
    {
        InterfaceLetter letter = _letters[index];
        DropLetter(letter);
    }

    private void DropLetter(InterfaceLetter letter)
    {
        _minion.DropLetter(letter.Letter);
        letter.Delete();
        _letters.Remove(letter);
    }

    public void SelectWord(string word)
    {
        _selectedSlots = CreateSelectedContentSlots(word.Length);

        List<InterfaceLetter> letters = new List<InterfaceLetter>();

        for (int i = 0; i < word.Length; i++)
        {
            InterfaceLetter letter = _letters.First((l) => l.Letter == word[i].ToString());
            letter.Outlined = true;
            letter.SelectionIndex = i;
            letters.Add(letter);
            StartCoroutine(Utils.DelayedCall(i * 0.1f + 1f, () => letter.Delete()));
            StartCoroutine(Utils.DelayedCall(i * 0.1f + 1.2f, () => _letters.Remove(letter)));
        }

        StartCoroutine(Utils.DelayedCall(letters.Count * 0.1f + 1.2f, DeleteSelectedContentSlots));
    }

    private Transform[] CreateSelectedContentSlots(int count)
    {
        Transform[] content = new Transform[count];
        for (int i = 0; i < count; i++)
            content[i] = Instantiate(_slotPrefab, _selectedContent).transform;
        return content;
    }

    private void DeleteSelectedContentSlots()
    {
        for (int i = _selectedSlots.Length - 1; i >= 0; i--)
        {
            Transform slot = _selectedSlots[i];
            StartCoroutine(Utils.CFLocalScale(slot.localScale, Vector3.zero, 0.5f, slot));
            StartCoroutine(Utils.DelayedCall(0.6f, () => Destroy(slot.gameObject)));
        }

        _selectedSlots = null;
    }
}
