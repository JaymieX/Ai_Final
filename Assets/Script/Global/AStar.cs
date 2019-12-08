using System;
using System.Collections.Generic;
using UnityEngine;
using static Script.Global.LevelMap;

namespace Script.Global
{
    public class AStar
    {
        private static List<MapItem> _visited = new List<MapItem>();
        private static List<MapItem> _unvisited = new List<MapItem>();

        private static Dictionary<MapItem, MapItem> _predecessorDict = new Dictionary<MapItem, MapItem>();
        private static Dictionary<MapItem, float> _fDistanceDict = new Dictionary<MapItem, float>();
        private static Dictionary<MapItem, float> _gDistanceDict = new Dictionary<MapItem, float>();

        public static Stack<Vector2> GetAStarPath(List<List<LevelMap.MapItem>> graph, Vector2 start, Vector2 end)
        {
            // Clear old data
            _visited.Clear();
            _unvisited.Clear();

            _predecessorDict.Clear();
            _fDistanceDict.Clear();
            _gDistanceDict.Clear();

            Stack<Vector2> result = new Stack<Vector2>();

            MapItem mapNodeStart = graph[(int)start.y][(int)start.x];
            MapItem mapNodeEnd = graph[(int)end.y][(int)end.x];

            // Add all nodes
            for (int y = 0; y < graph.Count; y++)
            {
                for (int x = 0; x < graph[y].Count; x++)
                {
                    MapItem node = graph[y][x];

                    if (node.Type == MapItemType.Path)
                    {
                        _fDistanceDict[node] = float.MaxValue;
                        _gDistanceDict[node] = float.MaxValue;
                        _unvisited.Add(node);
                    }
                }
            }

            // Set start to 0
            _fDistanceDict[mapNodeStart] = 0;
            _gDistanceDict[mapNodeStart] = 0;

            int debugCount = 0;

            // Search loop
            while (_unvisited.Count > 0)
            {
                if (debugCount > 5000)
                {
                    Debug.LogError("AStar error!");
                    return result;
                }

                MapItem u = GetClosestFromUnvisited();

                // Break if reaching goal
                if (u.Equals(mapNodeEnd)) break;

                // Add to visited
                _visited.Add(u);

                foreach (MapItem v in GetNeighbors(u, graph))
                {
                    if (_visited.Contains(v))
                        continue;

                    if (_fDistanceDict[v] > _gDistanceDict[u] + GetEstimatedDistance(v, mapNodeEnd))
                    {
                        _fDistanceDict[v] = _gDistanceDict[u] + GetEstimatedDistance(v, mapNodeEnd);
                        _gDistanceDict[v] = _gDistanceDict[u];

                        _predecessorDict[v] = u;
                    }
                }

                debugCount++;
            }

            // Add result
            MapItem p = _predecessorDict[mapNodeEnd];
            int count = 200;

            while (!p.Equals(mapNodeStart))
            {
                p = _predecessorDict[p];
                result.Push(p.Position);

                if (count < 0) break;

                count--;
            }

            return result;
        }

        private static MapItem GetClosestFromUnvisited()
        {
            float shortest = float.MaxValue;
            MapItem shortestNode = new MapItem();

            foreach (var node in _unvisited)
            {
                if (shortest > _fDistanceDict[node])
                {
                    shortest = _fDistanceDict[node];
                    shortestNode = node;
                }
            }

            _unvisited.Remove(shortestNode);
            return shortestNode;
        }

        private static List<MapItem> GetNeighbors(MapItem item, List<List<LevelMap.MapItem>> graph)
        {
            List<MapItem> result = new List<MapItem>();

            void AddItem(Vector2Int pos)
            {
                if (pos.y < graph.Count && pos.y >= 0)
                {
                    if (pos.x < graph[pos.y].Count && pos.x >= 0)
                    {
                        if (graph[pos.y][pos.x].Type == MapItemType.Path)
                        {
                            result.Add(graph[pos.y][pos.x]);
                        }
                    }
                }
            }

            AddItem(new Vector2Int(item.MapPosition.x, item.MapPosition.y + 1));
            AddItem(new Vector2Int(item.MapPosition.x, item.MapPosition.y - 1));
            AddItem(new Vector2Int(item.MapPosition.x + 1, item.MapPosition.y));
            AddItem(new Vector2Int(item.MapPosition.x - 1, item.MapPosition.y));

            return result;
        }

        private static float GetEstimatedDistance(MapItem a, MapItem b)
        {
            return Vector2.Distance(a.Position, b.Position);
        }
    }
}
