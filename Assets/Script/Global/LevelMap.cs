using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Script.Global
{
    public class LevelMap : MonoBehaviour
    {
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
        private Tilemap pathMap;
        #endregion

        private List<List<MapItem>> _map;

        private bool _debugDrawStart = false;

        private void OnDrawGizmos()
        {
            if (!_debugDrawStart) return;

            Color baseColor = Gizmos.color;

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

            Gizmos.color = baseColor;
        }

        private void Start()
        {
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
            int xOffset = 0;
            int yOffset = 0;

            foreach (var pos in map.cellBounds.allPositionsWithin)
            {
                Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                Vector3 place = map.CellToWorld(localPlace);

                 Vector3 size = map.layoutGrid.cellSize / 2;
                 size.z = 0f;

                if (first)
                {
                    xOffset = Mathf.Abs(localPlace.x);
                    yOffset = Mathf.Abs(localPlace.y);
                    first = false;
                }

                if (map.HasTile(localPlace))
                {
                    _map[yOffset + localPlace.y][xOffset + localPlace.x] = new MapItem(place + size, type);
                }
            }

        }

        private void Update()
        {
        }
    }
}
