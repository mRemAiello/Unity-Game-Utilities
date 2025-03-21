using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Light Settings")]
        [SerializeField] private Light _directionalLight;
        [SerializeField] private List<Color> _lightColors;
        [SerializeField] private AnimationCurve _lightIntensityCurve;

        [Header("Cycle Settings")]
        [SerializeField, Range(0f, 24f)] private float _timeOfDay = 0f;
        [SerializeField] private float _dayDurationInMinutes = 1f;
        [SerializeField] private float _transitionSpeed = 1f;
        [SerializeField] private float _rotationSpeed = 1f;

        [Header("Debug")]
        [ReadOnly] private float _currentTime = 0f;
        [ReadOnly] private int _currentColorIndex = 0;

        void Start()
        {
            // Inizializza il tempo del ciclo
            _currentTime = 0f;
            _currentColorIndex = 0;
        }

        void Update()
        {
            UpdateTimeOfDay();
            UpdateLighting();
            RotateSun();
        }

        // Aggiorna il tempo del ciclo giorno/notte
        private void UpdateTimeOfDay()
        {
            // Incrementa il tempo in base alla durata del ciclo
            _currentTime += Time.deltaTime / 60f * (24f / _dayDurationInMinutes);

            // Reset del tempo quando raggiunge 24 ore
            if (_currentTime >= 24f)
            {
                _currentTime = 0f;
            }

            _timeOfDay = _currentTime;
        }

        // Aggiorna il colore e l'intensità della luce
        private void UpdateLighting()
        {
            // Transizione tra i colori nella lista con Lerp
            if (_lightColors.Count > 1)
            {
                // Calcola l'indice successivo
                int nextColorIndex = (_currentColorIndex + 1) % _lightColors.Count;

                // Normalizza il tempo del ciclo in base alla lista di colori
                float normalizedTime = _timeOfDay / 24f;

                // Interpola il colore attuale tra quello corrente e il prossimo
                _directionalLight.color = Color.Lerp(_lightColors[_currentColorIndex], _lightColors[nextColorIndex], normalizedTime * _transitionSpeed);

                // Quando l'interpolazione è completa, passa al colore successivo
                if (normalizedTime >= 1f)
                {
                    _currentColorIndex = nextColorIndex;
                }
            }

            // Aggiorna l'intensità della luce usando la curva
            float intensityTime = _timeOfDay / 24f;
            _directionalLight.intensity = _lightIntensityCurve.Evaluate(intensityTime);
        }

        // Ruota la luce per simulare il movimento del sole
        private void RotateSun()
        {
            float sunRotation = (_timeOfDay / 24f) * 360f;
            _directionalLight.transform.rotation = Quaternion.Euler(new Vector3(sunRotation - 90f, 170f, 0f));
        }
    }
}