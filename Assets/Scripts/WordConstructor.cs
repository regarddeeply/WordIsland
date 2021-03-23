using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordConstructor : MonoBehaviour
{
    [SerializeField] private WordAsset _data = null;

    public WordAsset Data => _data;

    public string[] CheckWords(string[] letters)
    {
        List<string> result = new List<string>();
        List<string> list;
        foreach(var word in Data.Words)
        {
            list = letters.Select((letter) => letter.ToUpper()).ToList();
            bool found = true;

            foreach (var wordLetter in word)
            {
                if (list.Contains(wordLetter.ToString()))
                    list.Remove(wordLetter.ToString());
                else
                {
                    found = false;
                    break;
                }
            }

            if (found) result.Add(word);
        }
        return result.ToArray();
    }
}
