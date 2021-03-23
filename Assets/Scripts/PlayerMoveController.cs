using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Minion _minion = null;
    [Range(0f, 10f)]
    [SerializeField] private float _maxLength = 2f;
    [Range(0f, 10f)]
    [SerializeField] private float _maxSpeed = 3.5f;

    public bool Holded { get; private set; } = false;

    private Vector2 _startPosition = Vector2.zero;

    public void OnPointerDown(PointerEventData eventData)
    {
        Holded = true;
        _startPosition = Input.mousePosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Holded = false;
        _minion.Direction = Vector2.zero;
        _minion.Speed = 0f;
    }

    private void Update()
    {
        if (Holded)
        {
            Vector2 dir = Vector2.ClampMagnitude(((Vector2)Input.mousePosition - _startPosition) / 100f, _maxLength);
            _minion.Direction = dir;
            _minion.Speed = dir.magnitude / _maxLength * _maxSpeed;
        }
    }
}
