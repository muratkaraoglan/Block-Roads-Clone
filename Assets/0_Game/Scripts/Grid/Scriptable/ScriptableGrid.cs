using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableGrid : ScriptableObject
{
    [SerializeField] protected NodeBase nodeBasePrefab;
    [SerializeField, Range(3, 50)] protected int _gridWidth = 10;
    [SerializeField, Range(3, 50)] protected int _gridHeight = 10;
    public abstract Dictionary<Vector3, NodeBase> GenerateGrid();


    public int Width => _gridWidth;
    public int Height => _gridHeight;
}


