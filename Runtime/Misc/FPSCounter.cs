using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class FPSCounter : Singleton<FPSCounter>
    {
        [Tab("Timing")]
        [SerializeField] private float _resetTime;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private bool _trackFPS = false;

        [Space]
        [SerializeField, ReadOnly] private int _frameCount = 0;
        [SerializeField, ReadOnly] private float _totalDeltaTime = 0.0f;

        [Space]
        [SerializeField, ReadOnly] private float _minFPS = float.MaxValue;
        [SerializeField, ReadOnly] private float _maxFPS = float.MinValue;
        [SerializeField, ReadOnly] private float _averageFPS = 0.0f;

        //
        public float MinFPS => _minFPS;
        public float MaxFPS => _maxFPS;
        public float AverageFPS => _averageFPS;

        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _trackFPS = false;
            _frameCount = 0;
            _totalDeltaTime = 0.0f;
            _minFPS = float.MaxValue;
            _maxFPS = float.MinValue;
            _averageFPS = 0.0f;
        }

        void Update()
        {
            //
            if (!_trackFPS)
                return;

            // 
            _totalDeltaTime += Time.deltaTime;
            _frameCount++;
            float currentFPS = 1.0f / Time.deltaTime;

            // 
            if (currentFPS < _minFPS)
                _minFPS = currentFPS;

            if (currentFPS > _maxFPS)
                _maxFPS = currentFPS;

            // 
            _averageFPS = _frameCount / _totalDeltaTime;

            //
            if (_totalDeltaTime > _resetTime)
            {
                _totalDeltaTime = 0;
                _frameCount = 0;
            }
        }

        public void EnableTracking() => _trackFPS = true;
        public void DisableTracking() => _trackFPS = false;
    }
}