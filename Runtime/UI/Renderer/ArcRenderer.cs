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
            if (InputManager.Instance == null)
            {
                return;
            }
            
            //
            var mousePosition = InputManager.Instance.CurrentPositionVector3;
            mousePosition.z = 0;

            //
            Vector3 startPos = transform.position;
            Vector3 midPoint = CalculateMidPoint(startPos, mousePosition);

            //
            UpdateArc(startPos, midPoint, mousePosition);
            PositionAndRotateArrow(mousePosition);
        }

        private void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
        {
            int numDots = Mathf.CeilToInt(Vector3.Distance(start, end) / _spacing);

            //
            for (int i = 0; i< numDots && i < _dotsPool.Count; i++)
            {
                float t = i / (float)numDots;
                t = Mathf.Clamp(t, 0f, 1f);

                //
                Vector3 position = QuadraticBezierPoint(start, mid, end, t);

                //
                if (i != numDots - _dotsToSkip)
                {
                    _dotsPool[i].transform.position = position;
                    _dotsPool[i].SetActive(true);
                }

                //
                if (i == numDots - (_dotsToSkip + 1 ) && i - _dotsToSkip + 1 >= 0)
                {
                    _arrowDirection = _dotsPool[i].transform.position;
                }
            }

            //
            for (int i = numDots - _dotsToSkip; i < _dotsPool.Count; i++)
            {
                if (i > 0)
                {
                    _dotsPool[i].SetActive(false);
                }
            }
        }

        private Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;

            //
            Vector3 point = uu * start;
            point += 2 * u * t * control;
            point += tt * end;
            
            //
            return point;
        }

        private void PositionAndRotateArrow(Vector3 position)
        {
            _arrowInstance.transform.position = position;
            Vector3 direction = _arrowDirection - position;

            //
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += _arrowAngleAdjustment;

            //
            _arrowInstance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private Vector3 CalculateMidPoint(Vector3 start, Vector3 end)
        {
            var midPoint = (start + end) / 2;
            float arcHeight = Vector3.Distance(start, end) / 3;
            midPoint.y += arcHeight;

            //
            return midPoint;
        }

        private void InitilizeDotPool(int count)
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
