using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Draws a curved line using a LineRenderer and updates in edit mode.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class CurveLineRenderer : MonoBehaviour
    {
        // Control points used to build the curve in order.
        [SerializeField] private Transform[] _controlPoints;
        // Number of segments between each pair of points.
        [SerializeField] private int _segmentsPerCurve = 12;
        // When true, the curve will loop back to the start.
        [SerializeField] private bool _loop;
        // When true, positions are set in world space.
        [SerializeField] private bool _useWorldSpace = true;

        // Cached reference to the LineRenderer.
        private LineRenderer _lineRenderer;

        private void OnEnable()
        {
            // Ensure the line is refreshed when the component is enabled.
            RefreshLine();
        }

        private void OnValidate()
        {
            // Clamp segments to avoid invalid values in the inspector.
            _segmentsPerCurve = Mathf.Max(1, _segmentsPerCurve);
            // Refresh line immediately when inspector values change.
            RefreshLine();
        }

        private void Update()
        {
            // Update every frame so movement is visible in the editor.
            RefreshLine();
        }

        private void RefreshLine()
        {
            // Lazily fetch the LineRenderer component.
            if (_lineRenderer == null)
            {
                _lineRenderer = GetComponent<LineRenderer>();
            }

            // Early-out if there are not enough control points.
            if (_controlPoints == null || _controlPoints.Length < 2)
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            // Configure the LineRenderer settings.
            _lineRenderer.useWorldSpace = _useWorldSpace;
            _lineRenderer.loop = _loop;

            // Compute the total number of positions needed.
            int curveCount = _loop ? _controlPoints.Length : _controlPoints.Length - 1;
            int totalPositions = (_segmentsPerCurve * curveCount) + 1;
            Vector3[] positions = new Vector3[totalPositions];

            int positionIndex = 0;
            for (int curveIndex = 0; curveIndex < curveCount; curveIndex++)
            {
                // Sample each curve segment with a Catmull-Rom interpolation.
                for (int segmentIndex = 0; segmentIndex < _segmentsPerCurve; segmentIndex++)
                {
                    float t = segmentIndex / (float)_segmentsPerCurve;
                    positions[positionIndex++] = SampleCurve(curveIndex, t);
                }
            }

            // Ensure the last position lands exactly on the final control point.
            positions[positionIndex] = GetPointPosition(_loop ? 0 : _controlPoints.Length - 1);

            _lineRenderer.positionCount = positions.Length;
            _lineRenderer.SetPositions(positions);
        }

        private Vector3 SampleCurve(int index, float t)
        {
            // Gather the four points needed for Catmull-Rom interpolation.
            Vector3 p0 = GetPointPosition(index - 1);
            Vector3 p1 = GetPointPosition(index);
            Vector3 p2 = GetPointPosition(index + 1);
            Vector3 p3 = GetPointPosition(index + 2);

            // Apply the Catmull-Rom spline formula.
            return 0.5f * ((2f * p1) +
                (-p0 + p2) * t +
                (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
                (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t);
        }

        private Vector3 GetPointPosition(int index)
        {
            // Handle looping or clamp to valid range when not looping.
            int safeIndex = _loop
                ? Mod(index, _controlPoints.Length)
                : Mathf.Clamp(index, 0, _controlPoints.Length - 1);

            // Return world or local space position based on configuration.
            return _useWorldSpace ? _controlPoints[safeIndex].position : _controlPoints[safeIndex].localPosition;
        }

        private static int Mod(int value, int modulus)
        {
            // Custom modulus to keep indices positive.
            return (value % modulus + modulus) % modulus;
        }
    }
}
