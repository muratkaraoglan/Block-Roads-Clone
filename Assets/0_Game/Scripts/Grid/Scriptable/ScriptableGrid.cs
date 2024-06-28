using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableGrid : ScriptableObject
{
    [SerializeField] protected NodeBase nodeBasePrefab;
    public abstract Dictionary<Vector3, NodeBase> GenerateGrid();
}


