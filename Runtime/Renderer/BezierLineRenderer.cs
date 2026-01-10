using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class BezierLineRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        [SerializeField] private int _vertexCount = 200;
        [SerializeField] private Transform _point0;
        [SerializeField] private Transform _point1;
        [SerializeField] private Transform _point2;

        //
        void Start()
        {
            _lineRenderer.positionCount = _vertexCount;
        }

        void Update()
        {
            DrawQuadraticBezierCurve();
        }

        [Button(ButtonSizes.Medium)]
        private void DrawQuadraticBezierCurve()
        {
            float t = 0f;
            Vector3 B = new(0, 0, 0);
            for (int i = 0; i < _lineRenderer.positionCount; i++)
            {
                B = (1 - t) * (1 - t) * _point0.position + 2 * (1 - t) * t * _point1.position + t * t * _point2.position;
                _lineRenderer.SetPosition(i, B);
                t += 1 / (float)_lineRenderer.positionCount;
            }
        }
    }
}