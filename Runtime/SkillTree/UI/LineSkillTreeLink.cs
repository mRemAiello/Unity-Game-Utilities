using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Draws a straight line using a LineRenderer and updates in edit mode.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(LineRenderer))]
    public class LineSkillTreeLink : MonoBehaviour
    {
        [SerializeField] private Transform _start;
        [SerializeField] private Transform _end;

        // Cached reference to the LineRenderer.
        private LineRenderer _lineRenderer;

        private void OnEnable()
        {
            // Ensure the line is refreshed when the component is enabled.
            RefreshLine();
        }

        private void OnValidate()
        {
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

            // Ensure the LineRenderer uses the requested space.
            _lineRenderer.useWorldSpace = true;

            // Hide the line if start or end is missing.
            if (_start == null || _end == null)
            {
                _lineRenderer.positionCount = 0;
                return;
            }

            // Set the two positions for a straight line.
            _lineRenderer.positionCount = 2;
            _lineRenderer.SetPosition(0, _start.position);
            _lineRenderer.SetPosition(1, _end.position);
        }
    }
}