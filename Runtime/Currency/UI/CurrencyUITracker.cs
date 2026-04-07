using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace GameUtils
{
    [DeclareBoxGroup("Currency")]
    [DeclareBoxGroup("Events")]
    [DeclareBoxGroup("Debug")]
    public class CurrencyUITracker : MonoBehaviour, ILoggable
    {
        [SerializeField, Required, Group("Currency")] private CurrencyData _currencyData;
        [SerializeField, Required, Group("Currency")] private TextMeshProUGUI _currencyText;
        [SerializeField, Required, Group("Currency")] private Image _currencyIcon;
        [SerializeField, Group("Currency"), PreviewObject] private Sprite _fallbackIcon;
        [SerializeField, Group("Events")] private CurrencyChangeEvent _onChangeEvent;
        [SerializeField, Group("Events")] private bool _logEnabled = true;

        //
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            _onChangeEvent?.AddListener(this, OnCurrencyChangeEvent);

            // Prime the UI
            UpdateUI();
        }

        private void OnCurrencyChangeEvent(CurrencyChangeEventArgs input) => UpdateUI();

        /// <summary>
        /// Refreshes text and loads the currency icon using Addressables. The handle is
        /// released in <see cref="OnIconLoaded"/>. A fallback icon is shown if loading fails.
        /// </summary>
        private void UpdateUI()
        {
            if (_currencyData.AssetReferenceIcon != null)
            {
                var handle = Addressables.LoadAssetAsync<Sprite>(_currencyData.AssetReferenceIcon);
                handle.Completed += OnIconLoaded;
            }
            else if (_fallbackIcon != null)
            {
                _currencyIcon.sprite = _fallbackIcon;
            }

            _currencyText.text = CurrencyManager.Instance.GetCurrencyAmount(_currencyData).ToString();
        }

        private void OnIconLoaded(AsyncOperationHandle<Sprite> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                _currencyIcon.sprite = handle.Result;
            }
            else
            {
                Debug.LogWarning("Failed to load currency icon; using fallback sprite.");
                if (_fallbackIcon != null)
                    _currencyIcon.sprite = _fallbackIcon;
            }

            Addressables.Release(handle);
        }
    }
}
