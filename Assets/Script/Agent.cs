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

        private void Start()
        {
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
