using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private TextMeshPro _label = null;

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

    public bool Active { get; private set; } = false;

    public bool IsEmpty => string.IsNullOrEmpty(Letter);

    private Vector3 _startScale = Vector3.one;

    private void Awake()
    {
        _startScale = transform.localScale;
    }

    private void Start()
    {
        Show();
    }

    private void OnEnable()
    {
        Show();
    }

    private void Show()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        StartCoroutine(Utils.CFLocalScale(Vector3.zero, _startScale, 0.5f, transform));
        StartCoroutine(Utils.DelayedCall(0.55f, () => Active = true));
    }

    public void Hide()
    {
        StartCoroutine(Utils.CFLocalScale(_startScale, Vector3.zero, 0.5f, transform));
        StartCoroutine(Utils.DelayedCall(0.55f, () => gameObject.SetActive(false)));
    }

    public void Delete()
    {
        Active = false;
        StartCoroutine(Utils.CFLocalScale(_startScale, Vector3.zero, 0.5f, transform));
        StartCoroutine(Utils.DelayedCall(0.55f, () => Destroy(gameObject)));
    }
}
