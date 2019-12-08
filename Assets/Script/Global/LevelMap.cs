using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Global
{
    public class LevelMap : MonoBehaviour
    {
        private static LevelMap _instance;

        public static LevelMap Instance => _instance;

        public enum MapItemType
        {
            Undefined,
            Path,
            Obstacle
        }

        public struct MapItem
        {
            public Vector2 Position;
            public MapItemType Type;

            public MapItem(Vector2 pos, MapItemType type)
            {
                Position = pos;
                Type = type;
            }
        }

        #region Editor

        [SerializeField]
        private bool showDebugPath;
        [SerializeField]
        private bool showDebugAStarPath;

        [SerializeField]
        private Tilemap pathMap;
        #endregion

        private List<List<MapItem>> _map;

        private int _xOffset, _yOffset;

        private bool _debugDrawStart = false;

        private Vector2 _debugPathStart, _debugPathEnd;

        private void OnDrawGizmos()
        {
            if (!_debugDrawStart) return;

            Color baseColor = Gizmos.color;

            if (showDebugPath)
            {
                for (int i = 0; i < _map.Count; i++)
                {
                    for (int j = 0; j < _map[i].Count; j++)
                    {
                        if (_map[i][j].Type == MapItemType.Obstacle)
                        {
                            Gizmos.color = Color.red;
                        }
                        else if (_map[i][j].Type == MapItemType.Path)
                        {
                            Gizmos.color = Color.green;
                        }
                        else if (_map[i][j].Type == MapItemType.Undefined)
                        {
                            continue;
                        }

                        Gizmos.DrawSphere(_map[i][j].Position, .1f);
                    }
                }
            }

            if (showDebugAStarPath)
            {
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(_debugPathStart, .1f);
                Gizmos.DrawSphere(_debugPathEnd, .1f);
            }

            Gizmos.color = baseColor;
        }

        private void Start()
        {
            // Singleton
            _instance = this;

            // Init map
            _map = new List<List<MapItem>>();
            for (int i = 0; i < pathMap.size.y; i++)
            {
                _map.Add(new List<MapItem>());
                for (int j = 0; j < pathMap.size.x; j++)
                {
                    _map[i].Add(new MapItem(Vector2.zero, MapItemType.Undefined));
                }
            }

            // Add to map
            AddToMap(pathMap, MapItemType.Path);

            _debugDrawStart = true;
        }

        private void AddToMap(Tilemap map, MapItemType type)
        {
            bool first = true;

            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = map.CellToWorld(localPlace);

                Vector3 size = map.layoutGrid.cellSize / 2;
                 size.z = 0f;

                if (first)
                {
                    _xOffset = Mathf.Abs(localPlace.x);
                    _yOffset = Mathf.Abs(localPlace.y);
                    first = false;
                }

                if (map.HasTile(localPlace))
                {
                    _map[_yOffset + localPlace.y][_xOffset + localPlace.x] = new MapItem(place + size, type);
                }
            }

        }

        public Queue<Vector2> GetPath(Vector2 start, Vector2 end)
        {
            Vector3Int tileStart = pathMap.WorldToCell(new Vector3(start.x, start.y, 0f));
            Vector3Int tileEnd = pathMap.WorldToCell(new Vector3(end.x, end.y, 0f));

            Vector2Int actualStart = new Vector2Int(tileStart.x + _xOffset, tileStart.y + _yOffset);
            Vector2Int actualEnd = new Vector2Int(tileEnd.x + _xOffset, tileEnd.y + _yOffset);

            if (actualEnd == Vector2Int.zero)
            {
                return new Queue<Vector2>();
            }

            _debugPathStart = _map[actualStart.y][actualStart.x].Position;
            _debugPathEnd = _map[actualEnd.y][actualEnd.x].Position;

            return AStar.GetAStarPath(_map, actualStart, actualEnd);
        }
    }
}
