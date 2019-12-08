using System;
using System.Collections;
using System.Collections.Generic;
using Script.Global;
using UnityEngine;

namespace Script
{
    public class Agent : MonoBehaviour
    {
        internal Queue<Vector2> CurrentPath;

        public float speed;

        private bool _debugDrawStart = false;

        private void OnDrawGizmos()
        {
            if (!_debugDrawStart) return;

            Queue<Vector2> pathCpy = new Queue<Vector2>(CurrentPath);

            Color baseColor = Gizmos.color;
            Gizmos.color = Color.cyan;

            while (pathCpy.Count != 0)
            {
                Vector3 point = pathCpy.Dequeue();

                Gizmos.DrawSphere(point, 0.1f);
            }

            Gizmos.color = baseColor;
        }

        private void Start()
        {
            _debugDrawStart = true;

            CurrentPath = new Queue<Vector2>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Debug.Log("Moving");

                CurrentPath = LevelMap.Instance.GetPath(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }
}
