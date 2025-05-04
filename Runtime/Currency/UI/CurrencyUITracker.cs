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
        [SerializeField, Group("currency")] private CurrencyData _currencyData;
        [SerializeField, Group("currency")] private TextMeshProUGUI _currencyText;
        [SerializeField, Group("currency")] private Image _currencyIcon;
        [SerializeField, Group("events")] private CurrencyChangeEvent _onChangeEvent;

        //
        public bool LogEnabled => _logEnabled;

        //
        void Start()
        {
            _onChangeEvent?.AddListener(OnCurrencyChangeEvent);

            //
            UpdateUI();
        }

        private void OnCurrencyChangeEvent(CurrencyChangeEventArgs input)
        {
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_currencyText == null)
            {
                this.LogError("Currency Text is not assigned in the inspector.");
                return;
            }

            //
            if (_currencyIcon == null)
            {
                this.LogError("Currency Icon is not assigned in the inspector.");
                return;
            }

            //
            if (_currencyData == null)
            {
                this.LogError("Currency Data is not assigned in the inspector.");
                return;
            }

            //
            _currencyIcon.sprite = _currencyData.Icon;
            _currencyText.text = CurrencyManager.Instance.GetCurrencyAmount(_currencyData).ToString();  
        }
    }
}