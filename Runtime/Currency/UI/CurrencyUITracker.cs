using System;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace GameUtils
{
    [DeclareBoxGroup("currency", Title = "Currency")]
    [DeclareBoxGroup("events", Title = "Events")]
    public class CurrencyUITracker : MonoBehaviour, ILoggable
    {
        [SerializeField] private bool _logEnabled = true;
        [SerializeField, Required, Group("currency")] private CurrencyData _currencyData;
        [SerializeField, Required, Group("currency")] private TextMeshProUGUI _currencyText;
        [SerializeField, Required, Group("currency")] private Image _currencyIcon;
        [SerializeField, Group("currency")] private Sprite _fallbackIcon;
        [SerializeField, Required, Group("events")] private CurrencyChangeEvent _onChangeEvent;

        //
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            _onChangeEvent?.AddListener(OnCurrencyChangeEvent);

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
