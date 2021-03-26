using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private WordConstructor _constructor = null;
    [SerializeField] private Slider _slider = null;
    [SerializeField] private Animator _animator = null;
    [SerializeField] private WordContainer _container = null;
    [SerializeField] private float _speed = 1f;
    private void Start()
    {
        _slider.maxValue = _constructor.Data.Words.Sum((word) => word.Length);
        _slider.value = 0f;
    }

    private void Update()
    {
        float value = _container.AllLetters;
        if(_slider.value != value)
        {
            _slider.value += Mathf.Sign(value - _slider.value) * _speed * Time.deltaTime;
            if (Mathf.Abs(_slider.value - value) < 0.05f) _slider.value = value;
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }
    }
}
