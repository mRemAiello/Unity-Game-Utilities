using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class ArcRenderer : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private GameObject _dotPrefab;
        [SerializeField] private int _poolSize = 50;
        [SerializeField] private float _spacing = 50;
        [SerializeField] private float _arrowAngleAdjustment = 0;
        [SerializeField] private int _dotsToSkip = 1;

        //
        private List<GameObject> _dotsPool = new();
        private GameObject _arrowInstance;
        private Vector3 _arrowDirection;

        void Start()
        {
            _arrowInstance = Instantiate(_arrowPrefab, transform);
            _arrowInstance.transform.localPosition = Vector3.zero;

            //
            InitilizeDotPool(_poolSize);
        }

        void Update()
        {
            // TODO: Mouse Position
            
        }

        void InitilizeDotPool(int count)
        {
            for (int i = 0; i < count; i++)
            {
                GameObject dot = Instantiate(_dotPrefab, Vector3.zero, Quaternion.identity, transform);
                dot.SetActive(false);
                _dotsPool.Add(dot);
            }
        }
    }
}
