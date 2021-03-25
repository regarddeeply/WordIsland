using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsController : MonoBehaviour
{
    [SerializeField] private WordConstructor _constructor = null;
    [SerializeField] private GameObject[] _objects = null;

    private Dictionary<string, GameObject> _dictionary = new Dictionary<string, GameObject>();

    private void Start()
    {
        string[] words = _constructor.Data.Words;
        for (int i = 0; i < words.Length; i++)
            _dictionary.Add(words[i], _objects[i]);
    }

    public void Activate(string word)
    {
        GameObject obj = _dictionary[word];
        obj.SetActive(true);

        Vector3 scale = obj.transform.localScale;
        StartCoroutine(Utils.CFLocalScale(Vector3.zero, scale * 1.1f, 0.5f, obj.transform));
        StartCoroutine(Utils.DelayedCall(0.5f, () => StartCoroutine(Utils.CFLocalScale(obj.transform.localScale, scale, 0.15f, obj.transform))));
    }
}
