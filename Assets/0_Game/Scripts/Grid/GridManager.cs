using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField] private ScriptableGrid _scriptableGrid;


    [field: SerializeField] public GridPathEvent GridPathEvent { get; private set; }
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
        int rndX = Random.Range(0, _scriptableGrid.Width);

        _playerNodeBase = GetTileAtPosition(new Vector3(rndX, 0, 0));
        GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = _playerNodeBase.transform.position + Vector3.back ;

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

    public List<NodeBase> TryFindPath(NodeBase endNode) => Pathfinding.FindPath(_playerNodeBase, endNode);

    public NodeBase GetTileAtPosition(Vector3 pos) => Tiles.TryGetValue(pos, out var tile) ? tile : null;

    public NodeBase GetRandomEndNode()
    {
        int x = Random.Range(0, _scriptableGrid.Width);
        int z = 0;
        if (x > 0 && x < _scriptableGrid.Width - 1)//selected row is not the edge ones
        {
            z = _scriptableGrid.Height - 1;
        }
        else
        {
            z = Random.Range(1, _scriptableGrid.Height);
        }

        return GetTileAtPosition(new Vector3(x, 0, z));
    }
}
