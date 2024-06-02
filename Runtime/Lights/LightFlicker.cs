using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class LightFlicker : MonoBehaviour
    {
        [Tab("Settings")]
        [SerializeField] private float _flickerIntensity = 0.2f;
        [SerializeField] private float _flickersPerSecond = 3.0f;
        [SerializeField] private float _speedRandomness = 1.0f;

        // private fields
        private float _time;
        private float _random;
        private float _startingIntensity;
        private Light _light;

        void Start()
        {
            _light = GetComponent<Light>();
            _startingIntensity = _light.intensity;
        }

        void Update()
        {
            _random = (1 - Random.Range(-_speedRandomness, _speedRandomness));
            _time += Time.deltaTime * _random * Mathf.PI;
            _light.intensity = _startingIntensity + Mathf.Sin(_time * _flickersPerSecond) * _flickerIntensity;
        }
    }
}