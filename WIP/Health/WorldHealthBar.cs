using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace GameUtils
{
    public class WorldHealthBar : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField, Required] private GameObject _uiRoot;
        [SerializeField, Required] private Slider _healthSlider;
        [SerializeField, Required] private Slider _fillHealthSlider;

        [Header("Settings")]
        [SerializeField] private bool _lookAtCamera = false;
        [SerializeField, Range(0.1f, 10)] private float _healthFillSpeed = 1;
        [SerializeField, Range(0.1f, 10)] private float _effectHealthFillSpeed = 1;

        [Header("Events")]
        [SerializeField, Required] private DamageInfoEventAsset _onDamage;

        //
        [Header("Debug")]
        [SerializeField, ReadOnly] private float _currentHP = 1;

        //
        void Awake()
        {
            //
            _healthSlider.minValue = 0;
            _fillHealthSlider.minValue = 0;
            _healthSlider.maxValue = 1;
            _fillHealthSlider.maxValue = 1;
            _healthSlider.value = 1;
            _fillHealthSlider.value = 1;
        }

        void OnEnable()
        {
            _onDamage.AddListener(OnDamage);
        }

        void OnDisable()
        {
            _onDamage.RemoveListener(OnDamage);
        }

        void Update()
        {
            if (_lookAtCamera)
            {
                // Face the camera
                transform.LookAt(Camera.main.transform);
                transform.Rotate(0, 180, 0); // Optionally rotate 180 degrees to face the camera properly
            }

            //
            float target = Mathf.Lerp(_healthSlider.value, _currentHP, _healthFillSpeed * Time.deltaTime);
            float targetEffect = Mathf.Lerp(_fillHealthSlider.value, _currentHP, _effectHealthFillSpeed * Time.deltaTime);

            //
            _healthSlider.value = target;
            _fillHealthSlider.value = targetEffect;
        }

        [Button]
        private void OnDamage(DamageInfo damageInfo)
        {
            _currentHP = damageInfo.Amount;
        }
    }
}