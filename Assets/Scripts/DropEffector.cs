using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEffector : MonoBehaviour
{
    [SerializeField] private GameObject[] _effects = null;
    [SerializeField] private float _delay = 2f;

    private bool _done = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (_done) return;
        _done = true;
        foreach (var effect in _effects)
            effect.SetActive(true);
        StartCoroutine(Utils.DelayedCall(_delay, () =>
        {
            foreach (var effect in _effects)
                effect.SetActive(false);
        }));
    }
}
