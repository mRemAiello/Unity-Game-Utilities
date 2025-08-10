using System;
using TMPro;
using TriInspector;
using UnityEngine;
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

            // Prime the UI
            UpdateUI();
        }

        private void OnCurrencyChangeEvent(CurrencyChangeEventArgs input) => UpdateUI();

        private async void UpdateUI()
        {
            try
            {
                _currencyIcon.sprite = await _currencyData.Icon;
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load currency icon: {e}");
            }

            _currencyText.text = CurrencyManager.Instance.GetCurrencyAmount(_currencyData).ToString();
        }
    }
}
