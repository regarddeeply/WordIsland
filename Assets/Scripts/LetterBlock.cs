using TMPro;
using UnityEngine;

public class LetterBlock : MonoBehaviour
{
    [SerializeField] private Transform _body = null;
    [SerializeField] private Transform _shadow = null;
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
        if (Active)
        {
            _body.localPosition = Vector3.up * (_height + _amplitude * Mathf.Sin(_offset + Time.realtimeSinceStartup * _frequency));
        }
        float scale = 1f - (_body.localPosition.y - _height + _amplitude) / (4f * _amplitude);
        scale = Mathf.Clamp(scale, 0f, 1f);
        _shadow.localScale = Vector3.one * scale;
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

    public void Delete()
    {
        Active = false;
        float from = _body.localPosition.y;
        float to = from + 8f;
        float duration = 0.4f;

        StartCoroutine(Utils.CrossFading(from, to, duration,
            (y) =>
            {
                Vector3 pos = _body.localPosition;
                pos.y = y;
                pos.x = Mathf.Pow(y, 2f) / (20f * y) * Mathf.Sin(y - from);
                _body.localPosition = pos;
            },
            (a, b, c) => Mathf.Lerp(a, b, Mathf.Pow(c, 2f))));

        StartCoroutine(Utils.DelayedCall(duration * 1.05f, () => Destroy(gameObject)));
    }
}
