using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InterfaceLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label = null;

    public string Letter
    {
        get => _label.text;
        set
        {
            _label.text = value;
            if (_label.text.Length > 1) _label.text = _label.text.Substring(0, 1);
            _label.text = _label.text.ToUpper();
        }
    }

    private void Show()
    {
        StartCoroutine(Utils.CFLocalScale(Vector3.zero, Vector3.one, 0.5f, transform));
    }

    private void Hide()
    {
        StartCoroutine(Utils.CFLocalScale(Vector3.one, Vector3.zero, 0.5f, transform));
    }

    private bool _deleting = false;
    public void Delete()
    {
        if (_deleting) return;
        Hide();
        StartCoroutine(Utils.DelayedCall(0.55f, () => Destroy(gameObject)));
    }

    private void Start()
    {
        Show();
    }
}
