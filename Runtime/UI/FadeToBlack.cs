using QFSW.QC;
using System.Collections;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class FadeToBlack : Singleton<FadeToBlack>
    {
        [Tab("Settings")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 1.0f;

        [Tab("Events")]
        [SerializeField] private VoidGameEventAsset _onPreAnimationEvent;
        [SerializeField] private VoidGameEventAsset _onMiddleAnimationEvent;
        [SerializeField] private VoidGameEventAsset _onEndAnimationEvent;

        [Tab("Debug")]
        [SerializeField, ReadOnlyField] private bool _isAnimating = false;

        [Command]
        public void StartFade()
        {
            // Eseguo solo se non c'è nessuna animazione in corso
            if (_isAnimating)
            {
                return;
            }

            // Lock del fade
            _isAnimating = true;

            // Evento di avvio animazione
            _onPreAnimationEvent.Invoke();

            // Avvio il fade
            StartCoroutine(StartLoad());
        }

        private IEnumerator StartLoad()
        {
            // Fade In
            yield return StartCoroutine(FadeScreen(0, 1, _duration));

            // Invoco il metodo di metà animazione
            _onMiddleAnimationEvent.Invoke();

            // Fade out
            yield return StartCoroutine(FadeScreen(1, 0, _duration));

            // Evento di fine animazione
            _onEndAnimationEvent.Invoke();

            // Lock del fade
            _isAnimating = false;
        }

        private IEnumerator FadeScreen(float startValue, float targetValue, float duration)
        {
            // Imposto l'opacità iniziale del Canvas
            _canvasGroup.alpha = startValue;

            float time = 0;
            while (time < duration)
            {
                // Funzione di smoothing
                float t = time / duration;
                t = MathUtility.SmoothTime(t);

                // Imposto il fade del pannello
                _canvasGroup.alpha = Mathf.Lerp(startValue, targetValue, t);

                // Aumento il tempo e ritorno
                time += Time.deltaTime;
                yield return null;
            }

            // Imposto il fade al massimo
            _canvasGroup.alpha = targetValue;
        }
    }
}