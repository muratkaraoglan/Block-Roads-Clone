using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyTileDedector : MonoBehaviour
{
    [field: SerializeField] public bool IsTileEmpty { get; private set; }

    [SerializeField] private LayerMask _targetLayer;
    private NodeBase _interactedNode;
    private bool _stopDetection;
    private PlacableMover _mover;
    private void Awake()
    {
        _mover = transform.root.GetComponent<PlacableMover>();
        _mover.OnTilePlaced += OnTilePlaced;
        _mover.OnTileCantPlaced += OnTileCantPlaced;
    }

    private void OnTileCantPlaced()
    {
        SetDefault();
    }

    private void OnTilePlaced()
    {
        _interactedNode.IsEmpty = false;
        _mover.OnTilePlaced -= OnTilePlaced;
        _mover.OnTileCantPlaced -= OnTileCantPlaced;
    }

    private void Update()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10f, _targetLayer))
        {
            if (!hit.transform.TryGetComponent(out NodeBase node)) return;
            _interactedNode = node;
            IsTileEmpty = node.IsEmpty;
            _interactedNode.ChangeMaterial(_mover.IsValidPlacement() ? MaterialState.Empty : MaterialState.Filled);
        }
        else
        {
            SetDefault();
        }
    }

    private void SetDefault()
    {
        if (_interactedNode != null)
        {
            _interactedNode.ChangeMaterial(MaterialState.Blank);
            _interactedNode = null;
            IsTileEmpty = false;
        }
    }

    public Vector3 InteractedTilePosition => _interactedNode != null ? _interactedNode.transform.position : Vector3.zero;


}
