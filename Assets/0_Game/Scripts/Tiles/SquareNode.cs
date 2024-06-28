using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SquareNode : NodeBase
{
    private static readonly List<Vector3> Dirs = new List<Vector3>()
    {
        new Vector3(0,0,1),//forward
        new Vector3(-1,0,0),//left
        new Vector3(0,0,-1),//back
        new Vector3(1,0,0)//right
    };

    public override void CacheNeighbors()
    {
        Neighbors = new List<NodeBase>();

        foreach (var tile in Dirs.Select(dir => GridManager.Instance.GetTileAtPosition(Coords.Position + dir)).Where(tile
            => tile != null))
        {
            Neighbors.Add(tile);
        }
    }


}

public struct SquareCoords : ICoord
{
    public Vector3 Position { get; set; }

    public float GetDistance(ICoord other)
    {
        var dist = new Vector3Int(Mathf.Abs((int)Position.x - (int)other.Position.x),
            Mathf.Abs((int)Position.y - (int)other.Position.y)
            , Mathf.Abs((int)Position.z - (int)Position.z));

        var lowest = Mathf.Min(dist.x, dist.y);
        var highest = Mathf.Max(dist.x, dist.y);

        var horizontalMovesRequired = highest - lowest;

        return lowest * 14 + horizontalMovesRequired * 10;
    }
}
