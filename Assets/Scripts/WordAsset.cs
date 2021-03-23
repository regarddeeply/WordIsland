using UnityEngine;

[CreateAssetMenu(fileName = "NewWordAsset", menuName = "Custom/WordAsset", order = 1)]
public class WordAsset : ScriptableObject
{
    [SerializeField] private string _name = null;
    [SerializeField] private string[] _words = null;

    public string Name => _name;
    public string[] Words
    {
        get
        {
            string[] words = new string[_words.Length];
            for (int i = 0; i < _words.Length; i++)
                words[i] = _words[i].ToUpper();
            return words;
        }
    }
}
