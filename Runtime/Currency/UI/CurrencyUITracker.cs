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
        [SerializeField, Required, Group("events")] private CurrencyChangeEvent _onChangeEvent;

        //
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            _onChangeEvent?.AddListener(OnCurrencyChangeEvent);

            // Avvia il caricamento asincrono dell'icona se è presente un AssetReference.
            if (_currencyData.AssetReferenceIcon != null)
            {
                AssetLoader.LoadAssetAsync<Sprite>(_currencyData.AssetReferenceIcon, OnIconLoaded);
            }

            // Imposta subito la UI con l'icona di fallback e il valore corrente.
            UpdateUI();
        }

        private void OnIconLoaded(AsyncOperationHandle<Sprite> handle)
        {
            if (handle.Status == AsyncOperationStatus.Succeeded && handle.Result != null)
            {
                _currencyIcon.sprite = handle.Result;
            }
            else
            {
                Debug.LogWarning($"Failed to load icon for currency: {_currencyData.ID}");
                _currencyIcon.sprite = _currencyData.Icon;
            }

            Addressables.Release(handle);
        }

        private void OnCurrencyChangeEvent(CurrencyChangeEventArgs input) => UpdateUI();

        private void UpdateUI()
        {
            // Usa l'icona attualmente disponibile (fallback se il caricamento asincrono fallisce)
            _currencyIcon.sprite = _currencyData.Icon;
            _currencyText.text = CurrencyManager.Instance.GetCurrencyAmount(_currencyData).ToString();
        }
    }
}