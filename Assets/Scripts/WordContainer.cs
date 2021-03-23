using System.Linq;
using UnityEngine;

public class WordContainer : MonoBehaviour, IWordContainer
{
    protected string[] _container = new string[0];
    public virtual bool IsFull => _container.Where((letter) => string.IsNullOrEmpty(letter)).Count() == 0;

    public virtual void AddLetter(string letter)
    {
        if (IsFull) return;
        for(int i = 0; i < _container.Length; i++)
        {
            if (string.IsNullOrEmpty(_container[i]))
            {
                _container[i] = letter;
                return;
            }
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
}
