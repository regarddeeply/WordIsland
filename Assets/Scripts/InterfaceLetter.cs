using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class InterfaceLetter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _label = null;
    [SerializeField] private Image _outline = null;
    [SerializeField] private Button _button = null;

    public RectTransform rectTransform { get; private set; } = null;

    public Button Button => _button;

    public int SelectionIndex { get; set; } = 0;

    public bool Outlined
    {
        get => _outline.enabled;
        set
        {
            _button.interactable = !value;
            if (value)
            {
                _outline.enabled = value;
                StartCoroutine(Utils.CFAlpha(0f, 0.8f, 0.2f, _outline));
            }
            else
            {
                StartCoroutine(Utils.CFAlpha(0.8f, 0f, 0.2f, _outline));
                StartCoroutine(Utils.DelayedCall(0.3f, () => _outline.enabled = value));
            }
        }
    }

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
        StartCoroutine(Utils.CFLocalScale(Vector3.zero, Vector3.one, 0.2f, transform));
    }

    private void Hide()
    {
        StartCoroutine(Utils.CFLocalScale(Vector3.one, Vector3.zero, 0.2f, transform));
    }

    private bool _deleting = false;
    public void Delete()
    {
        if (_deleting) return;
        Hide();
        StartCoroutine(Utils.DelayedCall(0.25f, () => Destroy(gameObject)));
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Show();
    }
}
