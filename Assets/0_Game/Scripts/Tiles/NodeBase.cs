using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NodeBase : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] private MeshRenderer _meshRenderer;
    public ICoord Coords;
    public float GetDistance(NodeBase otherNode) => Coords.GetDistance(otherNode.Coords);

    [field: SerializeField] public bool IsEmpty { get; set; }

    public virtual void Init(bool isEmpty, ICoord coords)
    {
        IsEmpty = isEmpty;
        Coords = coords;
        transform.position = Coords.Position;
        _renderer.color = isEmpty ? Color.white : Color.black;
    }

    public void ChangeMaterial(MaterialState state)
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();

        _meshRenderer.GetPropertyBlock(materialPropertyBlock);
        switch (state)
        {

            case MaterialState.Empty:
                {
                    materialPropertyBlock.SetColor("_Color", GridManager.Instance.EmptyColor);
                    _meshRenderer.SetPropertyBlock(materialPropertyBlock);
                    break;
                }
            case MaterialState.Filled:
                {
                    //_meshRenderer.materials[0] = GridManager.Instance.FilledMaterial;
                    materialPropertyBlock.SetColor("_Color", GridManager.Instance.FilledColor);
                    _meshRenderer.SetPropertyBlock(materialPropertyBlock);
                    break;
                }
            default:
                {
                    //_meshRenderer.materials[0] = GridManager.Instance.BlankMaterial;
                    materialPropertyBlock.SetColor("_Color", GridManager.Instance.BlankColor);
                    _meshRenderer.SetPropertyBlock(materialPropertyBlock);
                    break;
                }
        }
    }


    //public static event Action<NodeBase> OnHoverTile;
    //private void OnEnable()
    //{
    //    OnHoverTile += OnOnHoverTile;
    //}
    //private void OnDisable()
    //{
    //    OnHoverTile -= OnOnHoverTile;
    //}

    //private void OnOnHoverTile(NodeBase obj)
    //{

    //}

    //protected virtual void OnMouseDown()
    //{
    //    if (!IsEmpty) return;
    //    OnHoverTile?.Invoke(this);
    //}

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
        if (!IsEmpty) return;
        _renderer.color = Color.white;
    }

    #endregion
}

public interface ICoord
{
    public float GetDistance(ICoord other);
    public Vector3 Position { get; set; }
}

public enum MaterialState
{
    Blank,
    Empty,
    Filled
}