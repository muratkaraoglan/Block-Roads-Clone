using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private ScriptableGrid _scriptableGrid;

    public static GridManager Instance;

    [field: SerializeField] public Color BlankColor { get; private set; }
    [field: SerializeField] public Color EmptyColor { get; private set; }
    [field: SerializeField] public Color FilledColor { get; private set; }

    public Dictionary<Vector3, NodeBase> Tiles { get; private set; }

    private NodeBase _playerNodeBase, _goalNodeBase;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Tiles = _scriptableGrid.GenerateGrid();
        foreach (var tile in Tiles.Values) tile.CacheNeighbors();

        //_playerNodeBase = Tiles.Where(t => t.Value.IsEmpty).OrderBy(t => Random.value).First().Value;
        //Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), Vector3.up + _playerNodeBase.transform.position, Quaternion.identity);

        //NodeBase.OnHoverTile += OnHoverTile;
    }
    private void OnDestroy()
    {
        // NodeBase.OnHoverTile -= OnHoverTile;
    }
    private void OnHoverTile(NodeBase nodeBase)
    {
        _goalNodeBase = nodeBase;

        foreach (var tile in Tiles.Values) tile.ReseTTile();

        var path = Pathfinding.FindPath(_playerNodeBase, _goalNodeBase);
    }

    public NodeBase GetTileAtPosition(Vector3 pos) => Tiles.TryGetValue(pos, out var tile) ? tile : null;
}
