using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Script.Global
{
    [Serializable]
    public struct PrefabNode
    {
        public string objectName;
        public GameObject prefab;
    }

    public class Spawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject waveStart, waveEnd;

        [SerializeField]
        private List<PrefabNode> spawnableObjects;

        private Dictionary<string, GameObject> _objects;

        private static Spawner _instance;

        public static Spawner Instance => _instance;

        private void Start()
        {
            // Singleton
            _instance = this;

            _objects = new Dictionary<string, GameObject>();

            foreach (var spawnableObject in spawnableObjects)
            {
                _objects.Add(spawnableObject.objectName, spawnableObject.prefab);
            }
        }

        public void Spawn(string objectName, int count, float interval)
        {
            if (_objects.ContainsKey(objectName))
            {
                StartCoroutine(SpawnObjects(_objects[objectName], count, interval));
            }
            else
            {
                Debug.LogError("Object does not exists");
            }
        }

        private IEnumerator SpawnObjects(GameObject spawnObject, int count, float interval)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject spawned =
                    Instantiate(spawnObject, waveStart.transform.position, Quaternion.identity);

                var path = LevelMap.Instance.GetPath(waveStart.transform.position, waveEnd.transform.position);
                spawned.GetComponent<Agent>().SetPath(path);

                yield return new WaitForSeconds(interval);
            }
        }
    }
}
