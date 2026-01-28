using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace GameUtils
{
    /// <summary>
    /// Displays rendering performance statistics in UI TextMeshPro elements.
    /// </summary>
    public class PerformanceStatsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _fpsText;
        [SerializeField] private TextMeshProUGUI _drawCallsText;
        [SerializeField] private TextMeshProUGUI _batchesText;
        [SerializeField] private TextMeshProUGUI _setPassCallsText;
        [SerializeField] private TextMeshProUGUI _trianglesText;
        [SerializeField] private TextMeshProUGUI _verticesText;
        [SerializeField] private float _updateInterval = 0.5f;
        [SerializeField] private int _highFpsThreshold = 55;
        [SerializeField] private int _mediumFpsThreshold = 30;

        private ProfilerRecorder _drawCallsRecorder;
        private ProfilerRecorder _batchesRecorder;
        private ProfilerRecorder _setPassCallsRecorder;
        private ProfilerRecorder _trianglesRecorder;
        private ProfilerRecorder _verticesRecorder;
        private float _timeAccumulator;
        private int _frameCount;
        private float _currentFps;

        /// <summary>
        /// Initializes the profiler recorders when the component becomes active.
        /// </summary>
        private void OnEnable()
        {
            // Create recorders for the render statistics we want to show in the HUD.
            _drawCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Draw Calls Count");
            _batchesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Batches Count");
            _setPassCallsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count");
            _trianglesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Triangles Count");
            _verticesRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count");

            // Reset counters to avoid stale values after re-enable.
            _timeAccumulator = 0f;
            _frameCount = 0;
            _currentFps = 0f;

            // Force an initial refresh so the UI is populated immediately.
            UpdateTexts();
        }

        /// <summary>
        /// Releases profiler recorders when the component is disabled.
        /// </summary>
        private void OnDisable()
        {
            // Dispose recorders to avoid leaking profiler resources.
            _drawCallsRecorder.Dispose();
            _batchesRecorder.Dispose();
            _setPassCallsRecorder.Dispose();
            _trianglesRecorder.Dispose();
            _verticesRecorder.Dispose();
        }

        /// <summary>
        /// Updates the FPS counters and refreshes the UI at the chosen interval.
        /// </summary>
        private void Update()
        {
            // Accumulate frame data using unscaled time for accurate FPS.
            _timeAccumulator += Time.unscaledDeltaTime;
            _frameCount++;

            if (_timeAccumulator < Mathf.Max(_updateInterval, 0.1f))
            {
                return;
            }

            // Compute FPS and reset counters for the next interval.
            _currentFps = _frameCount / _timeAccumulator;
            _timeAccumulator = 0f;
            _frameCount = 0;

            // Push new values to the UI.
            UpdateTexts();
        }

        /// <summary>
        /// Updates all UI labels with the latest performance stats.
        /// </summary>
        private void UpdateTexts()
        {
            // Update FPS with threshold-based color feedback.
            if (_fpsText != null)
            {
                _fpsText.text = $"FPS: {Mathf.RoundToInt(_currentFps)}";
                _fpsText.color = GetFpsColor(_currentFps);
            }

            // Update rendering statistics from profiler counters.
            SetText(_drawCallsText, "Draw Calls", _drawCallsRecorder.LastValue);
            SetText(_batchesText, "Batches", _batchesRecorder.LastValue);
            SetText(_setPassCallsText, "SetPass Calls", _setPassCallsRecorder.LastValue);
            SetText(_trianglesText, "Triangles", _trianglesRecorder.LastValue);
            SetText(_verticesText, "Vertices", _verticesRecorder.LastValue);
        }

        /// <summary>
        /// Selects a color based on the measured FPS value.
        /// </summary>
        private Color GetFpsColor(float fps)
        {
            // Return green for high FPS, yellow for mid, and red for low.
            if (fps >= _highFpsThreshold)
            {
                return Color.green;
            }

            if (fps >= _mediumFpsThreshold)
            {
                return Color.yellow;
            }

            return Color.red;
        }

        /// <summary>
        /// Writes a formatted metric to a TMP label if it exists.
        /// </summary>
        private void SetText(TMP_Text label, string title, long value)
        {
            // Skip missing references to avoid null reference errors.
            if (label == null)
            {
                return;
            }

            label.text = $"{title}: {value}";
        }
    }
}
