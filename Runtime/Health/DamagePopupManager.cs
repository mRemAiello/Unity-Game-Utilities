using System.Collections;
using TMPro;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("Colors")]
    [DeclareBoxGroup("Popup")]
    [DeclareBoxGroup("Animations")]
    [DeclareBoxGroup("Events")]
    public class DamagePopupManager : Singleton<DamagePopupManager>
    {
        [SerializeField, Group("Popup")] private Canvas _damageTextCanvas;
        [SerializeField, Group("Popup")] private float _textFontSize = 20f;
        [SerializeField, Group("Popup")] private TMP_FontAsset _textFont;
        [SerializeField, Group("Popup"), ReadOnly] private Camera _referenceCamera;

        [SerializeField, Group("Colors")] private Color _damageColor = Color.red;
        [SerializeField, Group("Colors")] private Color _healColor = Color.green;

        //
        [SerializeField, Group("Animations")] private float _duration = 1f;
        [SerializeField, Group("Animations")] private float _speed = 50f;

        //
        [SerializeField, Group("Events")] private DamageEventAsset _damageEvent;
        [SerializeField, Group("Events")] private HealEventAsset _healEvent;

        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

            //
            _referenceCamera = Camera.main;

            //
            _damageEvent.AddListener(this, OnDamageEventRaised);
            _healEvent.AddListener(this, OnHealEventRaised);
        }

        private void OnHealEventRaised(string text, Transform transform)
        {
            StartCoroutine(GenerateFloatingTextCoroutine(text, transform, _healColor));
        }

        private void OnDamageEventRaised(string text, Transform transform)
        {
            StartCoroutine(GenerateFloatingTextCoroutine(text, transform, _damageColor));
        }

        private IEnumerator GenerateFloatingTextCoroutine(string text, Transform target, Color color)
        {
            // Create a new TextMeshPro object
            GameObject textObj = new("DamagePopup");
            RectTransform rect = textObj.AddComponent<RectTransform>();
            TextMeshProUGUI tmPro = textObj.AddComponent<TextMeshProUGUI>();
            tmPro.text = text;
            tmPro.fontSize = _textFontSize;
            tmPro.font = _textFont;
            tmPro.horizontalAlignment = HorizontalAlignmentOptions.Center;
            tmPro.verticalAlignment = VerticalAlignmentOptions.Middle;
            rect.position = _referenceCamera.WorldToScreenPoint(target.position);

            // Destroy
            Destroy(textObj, _duration);

            // Set the parent to the canvas
            textObj.transform.SetParent(_damageTextCanvas.transform);

            //
            WaitForEndOfFrame wait = new();
            float time = 0f;
            float yOffset = 0f;
            while (time < _duration)
            {
                // Wait
                yield return wait;
                time += Time.deltaTime;

                //
                tmPro.color = new Color(color.r, color.g, color.b, 1 - time / _duration);

                // Move the text upwards
                yOffset += _speed * Time.deltaTime;
                if (rect != null)
                    rect.position = _referenceCamera.WorldToScreenPoint(target.position + new Vector3(0f, yOffset, 0f));       
            }
        }
    }
}