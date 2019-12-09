using System;
using System.Collections;
using System.Collections.Generic;
using Script.Global;
using UnityEngine;

namespace Script
{
    public class Agent : MonoBehaviour
    {
        internal Stack<Vector2> CurrentPath;

        internal List<GameObject> ObjectsInRange;

        public float speed;

        [SerializeField]
        private bool drawDebugPath;

        private bool _debugDrawStart = false;

        private void OnDrawGizmos()
        {
            if (!drawDebugPath) return;

            if (!_debugDrawStart) return;

            Stack<Vector2> pathCpy = new Stack<Vector2>(CurrentPath);

            Color baseColor = Gizmos.color;
            Gizmos.color = Color.cyan;

            while (pathCpy.Count != 0)
            {
                Vector3 point = pathCpy.Pop();

                Gizmos.DrawSphere(point, 0.1f);
            }

            Gizmos.color = baseColor;
        }

        private void Start()
        {
            _debugDrawStart = true;

            if (CurrentPath == null)
            {
                CurrentPath = new Stack<Vector2>();
            }

            ObjectsInRange = new List<GameObject>();
        }

        public void SetPath(Stack<Vector2> path)
        {
            CurrentPath = path;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!ObjectsInRange.Contains(other.gameObject)) { ObjectsInRange.Add(other.gameObject); }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            ObjectsInRange.Remove(other.gameObject);
        }
    }
}
