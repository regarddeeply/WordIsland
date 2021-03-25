using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private Transform _body = null;
    [SerializeField] private TextMeshPro _frontLabel = null;
    [SerializeField] private TextMeshPro _backLabel = null;
    [SerializeField] private float _height = 1f;
    [SerializeField] private float _amplitude = 0.3f;
    [SerializeField] private float _frequency = 0.3f;

    private float _offset = 0f;

    public string Letter
    {
        get => _frontLabel.text;
        set
        {
            string letter = value;
            if (letter.Length > 1) letter = letter.Substring(0, 1);
            letter = letter.ToUpper();

            _frontLabel.text = letter;
            _backLabel.text = letter;
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
        _offset = Random.Range(0f, Mathf.PI);
    }

    private void Update()
    {
        _body.localPosition = Vector3.up * (_height + _amplitude * Mathf.Sin(_offset + Time.realtimeSinceStartup * _frequency));
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
