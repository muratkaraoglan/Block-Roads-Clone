namespace TestScene
{
    using System.Collections.Generic;
    using UnityEngine;

    public class GridCreator : MonoBehaviour
    {
        public static GridCreator Instance;

        private GridCreator _inputProvider;

        [Header("Grid Settings")] public int gridWidth = 16;
        public int gridHeight = 16;
        public float cellSize = 1f;

        [Header("Cell Visual")] public GameObject cellPrefab;

        private Dictionary<Vector2, (Vector3, Tile)> gridDictionary
            = new Dictionary<Vector2, (Vector3, Tile)>();

        private List<Tile> _highlightedTiles = new List<Tile>();

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            CreateGrid();
        }

        void CreateGrid()
        {
            gridDictionary.Clear();

            for (int x = 0; x < gridWidth; x++)
            {
                for (int y = 0; y < gridHeight; y++)
                {
                    Vector3 worldPos = new Vector3(
                        x * cellSize,
                        0,
                        y * cellSize
                    );

                    Vector2Int key = new Vector2Int(x, y);

                    GameObject cellObj = null;

                    if (cellPrefab != null)
                    {
                        cellObj = Instantiate(cellPrefab, worldPos, Quaternion.identity, transform);
                        cellObj.name = $"Cell_{key.x}_{key.y}";

                        // PREFAB SCALE – cellSize'a göre ayarla
                        ScalePrefabToCell(cellObj);
                    }

                    gridDictionary[key] = (worldPos, cellObj.GetComponent<Tile>());

                    var t = gridDictionary[key];
                    t.Item2.text.SetText(t.Item1.ToString());
                }
            }

            Debug.Log($"Grid created with {gridDictionary.Count} cells.");
        }

        private void ScalePrefabToCell(GameObject obj)
        {
            obj.transform.localScale = new Vector3(cellSize, obj.transform.localScale.y, cellSize);
        }

        public Vector2Int? WorldToGridPosition(Vector3 worldPos)
        {
            int x = Mathf.RoundToInt(worldPos.x / cellSize);
            int y = Mathf.RoundToInt(worldPos.z / cellSize);

            Vector2Int gridPos = new Vector2Int(x, y);

            if (gridDictionary.ContainsKey(gridPos))
                return gridPos;

            return null;
        }

        public void Highlight(Transform[] children)
        {
            ClearHighlights();

            bool canPlace = true;

            foreach (var child in children)
            {
                Vector2Int? gridPos = WorldToGridPosition(child.position);

                if (gridPos.HasValue && gridDictionary.TryGetValue(gridPos.Value, out var cellData))
                {
                    if (!cellData.Item2.isOccupied)
                        _highlightedTiles.Add(cellData.Item2);
                }
            }

            foreach (var tile in _highlightedTiles)
            {
                tile.sprite.color = Color.green;
            }
        }

        public void ClearHighlights()
        {
            foreach (var tile in _highlightedTiles)
            {
                tile.sprite.color = Color.white;
            }

            _highlightedTiles.Clear();
        }

        public bool CanPlaceShape(Transform[] children, out Vector3 snapPosition)
        {
            snapPosition = Vector3.zero;
            List<Vector2Int> positions = new List<Vector2Int>();
            foreach (var child in children)
            {
                Vector2Int? gridPos = WorldToGridPosition(child.position);

                if (!gridPos.HasValue || gridDictionary[gridPos.Value].Item2.isOccupied)
                {
                    return false;
                }

                snapPosition += gridDictionary[gridPos.Value].Item1;
                positions.Add(gridPos.Value);
            }

            foreach (var key in positions)
            {
                gridDictionary[key].Item2.isOccupied = true;
            }

            snapPosition /= children.Length;

            return true;
        }
    }
}