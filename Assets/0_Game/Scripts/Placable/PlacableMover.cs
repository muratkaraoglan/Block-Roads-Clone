using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacableMover : MonoBehaviour
{
    public GameObjectGameEvent GameObjectEvent;

    [SerializeField] private float _moveSpeed;

    private bool _isSelected;
    public bool _canSelect;
    private Vector3 _initialPosition;
    private Vector3 _movementStartPosition;

    private void Awake()
    {
        _canSelect = true;
        _initialPosition = transform.position;
        _movementStartPosition = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * .5f, Screen.height * .5f)).x, 3, -2);
    }

    private void Update()
    {
        if (!_isSelected) return;
        Vector2 moseDelta = InputHelper.Instance.MouseDelta;
        transform.position += new Vector3(moseDelta.x, 0, moseDelta.y) * Time.deltaTime * _moveSpeed;

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isSelected = false;
            transform.position = _initialPosition;
            GameObjectEvent.RaiseEvent(gameObject);
        }

    }

    private void OnMouseDown()
    {
        if (!_canSelect) return;
        transform.position = _movementStartPosition;
        _isSelected = true;
        GameObjectEvent.RaiseEvent(gameObject);
    }

    public void OnTileSelected(GameObject selectedTile)
    {
        if (selectedTile == gameObject) return;
        _canSelect = !_canSelect;
    }
}
