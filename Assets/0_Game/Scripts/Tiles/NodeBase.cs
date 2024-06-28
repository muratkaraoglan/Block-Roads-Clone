using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{

    [SerializeField] protected SpriteRenderer _renderer;
    public ICoord Coords;

    public float GetDistance(NodeBase otherNode) => Coords.GetDistance(otherNode.Coords);

    [field: SerializeField] public bool IsWalkable { get; set; }

    public virtual void Init(bool isWalkable, ICoord coords)
    {
        IsWalkable = isWalkable;
        Coords = coords;
        transform.position = Coords.Position;
        _renderer.color = isWalkable ? Color.white : Color.black;
    }

    public static event Action<NodeBase> OnHoverTile;
    private void OnEnable()
    {
        OnHoverTile += OnOnHoverTile;
    }
    private void OnDisable()
    {
        OnHoverTile -= OnOnHoverTile;
    }

    private void OnOnHoverTile(NodeBase obj)
    {

    }

    protected virtual void OnMouseDown()
    {
        if (!IsWalkable) return;
        OnHoverTile?.Invoke(this);
    }

    #region Pathfinding
    public List<NodeBase> Neighbors { get; protected set; }
    public NodeBase Connection { get; private set; }
    public float G { get; private set; }
    public float H { get; private set; }
    public float F => G + H;

    public abstract void CacheNeighbors();

    public void SetConnection(NodeBase nodeBase)
    {
        Connection = nodeBase;
    }

    public void SetG(float g) => G = g;
    public void SetH(float h) => H = h;

    public void SetColor(Color color) => _renderer.color = color;

    public void ReseTTile()
    {
        if (!IsWalkable) return;
        _renderer.color = Color.white;
    }

    #endregion
}

public interface ICoord
{
    public float GetDistance(ICoord other);
    public Vector3 Position { get; set; }
}
