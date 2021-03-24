using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CircleLayoutGroup : MonoBehaviour
{
    [SerializeField] private int _size = 6;
    [SerializeField] private float _range = 50f;
    [SerializeField] private Vector2 _positionOffset = Vector2.zero;
    [SerializeField] private float _angleOffset = 0f;

    private RectTransform _rect = null;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            RectTransform child = transform.GetChild(i).GetComponent<RectTransform>();
            child.position = transform.position + (Vector3)GetPosition(i % _size);
        }
    }

    private Vector2 GetPosition(int index)
    {
        float angle = 360f / _size * index + _angleOffset;
        angle = angle / 180f * Mathf.PI;
        Vector2 direction = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
        return direction * _range + _positionOffset;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < _size; i++)
        {
            Vector3 center = transform.position + (Vector3)GetPosition(i);
            Gizmos.DrawWireSphere(center, 25f);
            Gizmos.color = Color.green;
        }
    }
}
