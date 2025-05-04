using System;
using TMPro;
using TriInspector;
using UnityEngine;
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

            //
            if (_currencyData.AssetReferenceIcon != null)
            {
                // TODO: Fix
                //AssetLoader.LoadAssetAsync<Sprite>(_currencyData.AssetReferenceIcon, OnIconLoaded);
            }

            //
            UpdateUI();
        }

        private void OnIconLoaded(AsyncOperationHandle<Sprite> handle)
        {
            _currencyIcon.sprite = handle.Result;
        }

        private void OnCurrencyChangeEvent(CurrencyChangeEventArgs input) => UpdateUI();

        private void UpdateUI()
        {
            //
            _currencyIcon.sprite = _currencyData.Icon;
            _currencyText.text = CurrencyManager.Instance.GetCurrencyAmount(_currencyData).ToString();
        }
    }
}