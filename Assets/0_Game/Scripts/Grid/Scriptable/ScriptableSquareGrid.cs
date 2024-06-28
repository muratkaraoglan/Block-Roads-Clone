using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Square Grid", menuName = "Grid")]
public class ScriptableSquareGrid : ScriptableGrid
{
    [SerializeField, Range(3, 50)] private int _gridWidth = 10;
    [SerializeField, Range(3, 50)] private int _gridHeight = 10;
    public override Dictionary<Vector3, NodeBase> GenerateGrid()
    {
        var tiles = new Dictionary<Vector3, NodeBase>();
        var grid = new GameObject { name = "Grid" };

        for (int x = 0; x < _gridWidth; x++)
        {
            for (int z = 0; z < _gridHeight; z++)
            {
                var tile = Instantiate(nodeBasePrefab, grid.transform);
                tile.Init(Random.Range(1, 20) > 3, new SquareCoords { Position = new Vector3(x, 0, z) });
                tiles.Add(new Vector3(x, 0, z), tile);
            }
        }

        return tiles;
    }
}
