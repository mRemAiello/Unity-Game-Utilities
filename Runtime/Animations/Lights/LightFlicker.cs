using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class LightFlicker : MonoBehaviour
    {
        [Tab("Variables")]
        [SerializeField] private float _flickerIntensity = 0.2f;
        [SerializeField] private float _flickersPerSecond = 3.0f;
        [SerializeField] private float _speedRandomness = 1.0f;

        [Tab("Debug")]
        [SerializeField, ReadOnly] private float _time;
        [SerializeField, ReadOnly] private float _random;
        [SerializeField, ReadOnly] private float _startingIntensity;
        [SerializeField, ReadOnly] private Light _light;

        void Start()
        {
            //
            _time = 0;
            _random = 0;
            _startingIntensity = 0;

            //
            _light = GetComponent<Light>();
            _startingIntensity = _light.intensity;
        }

        void Update()
        {
            _random = 1 - Random.Range(-_speedRandomness, _speedRandomness);
            _time += Time.deltaTime * _random * Mathf.PI;
            _light.intensity = _startingIntensity + Mathf.Sin(_time * _flickersPerSecond) * _flickerIntensity;
        }
    }
}