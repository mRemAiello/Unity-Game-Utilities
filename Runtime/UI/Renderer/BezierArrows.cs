using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class BezierArrows : MonoBehaviour
    {
        [SerializeField] private GameObject _arrowHeadPrefab;
        [SerializeField] private GameObject _arrowNodePrefab;
        [SerializeField] private int _arrowNodeNum;
        [SerializeField] private float _scaleFactor = 1f;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private RectTransform _origin;
        [SerializeField, ReadOnly] private List<RectTransform> _arrowNodes = new();
        [SerializeField, ReadOnly] private List<Vector2> _controlPoints = new();
        [SerializeField, ReadOnly] private List<Vector2> _controlPointFactors;

        //
        void Awake()
        {
            //
            _controlPointFactors = new() { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) };

            //
            _origin = GetComponent<RectTransform>();

            // Instantiate arrow nodes and head
            for (int i = 0; i < _arrowNodeNum; i++)
            {
                _arrowNodes.Add(Instantiate(_arrowNodePrefab, transform).GetComponent<RectTransform>());
            }
            _arrowNodes.Add(Instantiate(_arrowHeadPrefab, transform).GetComponent<RectTransform>());
        
            // Hide arrow nodes
            _arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, 1000));

            // Initialize control points
            for (int i = 0; i < 4; i++)
            {
                _controlPoints.Add(Vector2.zero);
            }
        }

        void Update()
        {
            if (!InputManager.InstanceExists)
            {
                return;
            }

            // P0 arrow start
            _controlPoints[0] = new Vector2(_origin.position.x, _origin.position.y);

            // P3 mouse position
            _controlPoints[3] = new Vector2(InputManager.Instance.CurrentPositionVector2.x, InputManager.Instance.CurrentPositionVector2.y);
            
            // P1, P2 determinated by P0 / P3 position
            _controlPoints[1] = _controlPoints[0] + (_controlPoints[3] - _controlPoints[0]) * _controlPointFactors[0];
            _controlPoints[2] = _controlPoints[0] + (_controlPoints[3] - _controlPoints[0]) * _controlPointFactors[1];

            //
            for (int i = 0; i < _arrowNodes.Count; i++)
            {
                var t = Mathf.Log(1f * i / (_arrowNodes.Count - 1) + 1f, 2f);

                // Cubic Bezier curves
                _arrowNodes[i].position = Mathf.Pow(1 - t, 3) * _controlPoints[0] + 
                                            3 * Mathf.Pow(1 - t, 2) * t * _controlPoints[1] +
                                            3 * (1 - t) * Mathf.Pow(t, 2) * _controlPoints[2] +
                                            Mathf.Pow(t, 3) * _controlPoints[3];

                // Rotation
                if (i > 0)
                {
                    var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, _arrowNodes[i].position - _arrowNodes[i - 1].position));
                    _arrowNodes[i].rotation = Quaternion.Euler(euler);
                }

                // Scale
                var scale = _scaleFactor * (1f - 0.03f * (_arrowNodes.Count - i - 1));
                _arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
            }

            //
            _arrowNodes[0].transform.rotation = _arrowNodes[1].transform.rotation;
        }
    }
}