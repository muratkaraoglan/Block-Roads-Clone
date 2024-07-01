using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacableMover : MonoBehaviour
{
    public PlacableTileGameObjectEvent GameObjectEvent;
    public event Action OnTilePlaced = () => { };
    public event Action OnTileCantPlaced = () => { };
    [SerializeField] private float _moveSpeed;

    private bool _isSelected;
    private bool _canSelectable;
    private Vector3 _initialPosition;
    private Vector3 _movementStartPosition;
    private EmptyTileDedector[] _tileDedectors;
    private void Awake()
    {
        _canSelectable = true;
        _initialPosition = transform.position;
        _movementStartPosition = new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * .5f, Screen.height * .5f)).x, 3, -2);

        _tileDedectors = transform.GetComponentsInChildren<EmptyTileDedector>();
    }

    private void Update()
    {
        if (!_isSelected) return;
        Vector2 moseDelta = InputHelper.Instance.MouseDelta;
        transform.position += new Vector3(moseDelta.x, 0, moseDelta.y) * Time.deltaTime * _moveSpeed;

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            GameObjectEvent.RaiseEvent(gameObject);
            int emptyTileCount = 0;
            foreach (EmptyTileDedector tileDedector in _tileDedectors)
            {
                if (tileDedector.IsTileEmpty) emptyTileCount++;
                tileDedector.enabled = false;
            }
            if (emptyTileCount == _tileDedectors.Length)
            {
                transform.position = FindAvgPositionForPlacement();
                OnTilePlaced.Invoke();
                Destroy(this);
            }
            else
            {
                _isSelected = false;
                OnTileCantPlaced.Invoke();
                transform.position = _initialPosition;
            }

        }

    }

    private void OnMouseDown()
    {
        if (!_canSelectable) return;
        transform.position = _movementStartPosition;
        _isSelected = true;
        GameObjectEvent.RaiseEvent(gameObject);
        foreach (EmptyTileDedector tileDedector in _tileDedectors) tileDedector.enabled = true;
    }

    public void OnTileSelected(GameObject selectedTile)
    {
        if (selectedTile == gameObject) return;
        _canSelectable = !_canSelectable;
    }

    private Vector3 FindAvgPositionForPlacement()
    {
        Vector3 avgPos = Vector3.zero;

        foreach (EmptyTileDedector emptyTileDedector in _tileDedectors)
        {
            avgPos += emptyTileDedector.InteractedTilePosition;
        }
        avgPos = avgPos / _tileDedectors.Length;
        return avgPos;
    }

    public bool IsValidPlacement()
    {
        int emptyTileCount = 0;
        foreach (EmptyTileDedector tileDedector in _tileDedectors)
        {
            if (tileDedector.IsTileEmpty) emptyTileCount++;
        }
        return emptyTileCount == _tileDedectors.Length;
    }
}
