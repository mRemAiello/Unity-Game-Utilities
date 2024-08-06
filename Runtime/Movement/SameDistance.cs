using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class SameDistance : MonoBehaviour
    {
        [Tab("Settings")]
        [SerializeField] private float _xDistance = 0;
        [SerializeField] private float _yDistance = 0;
        [SerializeField] private float _zDistance = 0;
        [Space]
        [SerializeField] private bool _updateEveryFrame = false;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private List<GameObject> _gameObjects;

        void Awake()
        {
            _gameObjects = new List<GameObject>();
            for (int i = 0; i < transform.childCount; i++)
            {
                _gameObjects.Add(transform.GetChild(i).gameObject);
            }
        }

        void Update()
        {
            if (_updateEveryFrame)
            {
                UpdateObjectPositions();
            }
        }

        [Button]
        public void UpdateObjectPositions()
        {
            int i = 0;
            foreach (var obj in _gameObjects)
            {
                if (!obj.activeInHierarchy)
                    continue;

                //
                Vector3 pos = new()
                {
                    x = _xDistance * i,
                    y = _yDistance * i,
                    z = _zDistance * i
                };
                obj.transform.position = pos;

                //
                i++;
            }
        }
    }
}