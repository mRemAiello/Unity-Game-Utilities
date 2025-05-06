using System.Collections.Generic;
using System.Linq;
using TriInspector;
using UnityEditor;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    [DeclareBoxGroup("events", Title = "Events")]
    [DefaultExecutionOrder(-100)]
    public class CurrencyManager : Singleton<CurrencyManager>, ISaveable, ILoggable
    {
        [SerializeField] private string _currencyFolderPath = "Assets/Currencies/";
        [SerializeField] private bool _logEnabled = true;
        [SerializeField, Group("events")] private CurrencyChangeEvent _onChangeEvent;

        //
        [SerializeField, ReadOnly, Group("debug")] private List<CurrencyData> _currencies = new();
        [SerializeField, ReadOnly, Group("debug")] private SerializedDictionary<string, int> _savedCurrencies = new();

        //
        public string SaveContext => "Currency";
        public bool LogEnabled => _logEnabled;

        //
        protected override void OnPostAwake()
        {
            base.OnPostAwake();

#if UNITY_EDITOR
            InitCurrencies();
#endif

            //
            LoadAllCurrencies();
        }

        [Button]
        public void AddCurrency(CurrencyData currency, int amount)
        {
            if (!_savedCurrencies.ContainsKey(currency.ID))
                _savedCurrencies[currency.ID] = 0;

            //
            int currentAmount = Mathf.Clamp(_savedCurrencies[currency.ID] + amount, 0, currency.MaxAmount);
            _savedCurrencies[currency.ID] = currentAmount;
            _onChangeEvent?.Invoke(new CurrencyChangeEventArgs(currency, currentAmount));

            //
            SaveCurrency(currency.ID);
        }

        [Button]
        public bool TryRemoveCurrency(CurrencyData currency, int amount)
        {
            if (!_savedCurrencies.ContainsKey(currency.ID) || _savedCurrencies[currency.ID] < amount)
            {
                this.LogWarning($"Not enough currency: {currency.ID} - {amount}");
                return false;
            }

            //
            _savedCurrencies[currency.ID] -= amount;
            _onChangeEvent?.Invoke(new CurrencyChangeEventArgs(currency, amount));

            //
            SaveCurrency(currency.ID);
            return true;
        }

        [Button]
        public void SetCurrencyAmount(CurrencyData currency, int amount)
        {
            int currentAmount = Mathf.Clamp(amount, 0, currency.MaxAmount);
            _savedCurrencies[currency.ID] = currentAmount;
            _onChangeEvent?.Invoke(new CurrencyChangeEventArgs(currency, currentAmount));

            //
            SaveCurrency(currency.ID);
        }

        [Button]
        public bool TryExchangeCurrency(CurrencyData fromCurrency, CurrencyData toCurrency, int amount)
        {
            if (fromCurrency.TryGetCurrencyConversionRate(toCurrency, out var conversionRate))
            {
                int toAmount = Mathf.FloorToInt(amount * conversionRate.ConversionRate);
                if (TryRemoveCurrency(fromCurrency, amount))
                {
                    AddCurrency(toCurrency, toAmount);
                    return true;
                }
            }

            // Log error if conversion rate is not found
            Debug.LogWarning($"Conversion rate not found for {fromCurrency.ID} to {toCurrency.ID}");
            return false;
        }

        [Button]
        public bool TryExchangeCurrency(CurrencyData fromCurrency, CurrencyData toCurrency, int amount, float conversionRate)
        {
            int toAmount = Mathf.FloorToInt(amount * conversionRate);
            if (TryRemoveCurrency(fromCurrency, amount))
            {
                AddCurrency(toCurrency, toAmount);
                return true;
            }

            //
            this.LogWarning($"Not enough currency to exchange: {fromCurrency.ID} - {amount} - {toCurrency.ID} - {toAmount}");
            return false;
        }

        [Button]
        public void ResetCurrencies()
        {
            foreach (var currency in _currencies)
            {
                SetCurrencyAmount(currency, 0);
            }
        }

        private void SaveCurrency(string currencyID)
        {
            GameSaveManager.Instance.Save(SaveContext, currencyID, _savedCurrencies[currencyID]);
        }

        private void LoadCurrency(string currencyID)
        {
            _savedCurrencies[currencyID] = GameSaveManager.Instance.Load(SaveContext, currencyID, 0);
        }

        private void LoadAllCurrencies()
        {
            foreach (var key in GameSaveManager.Instance.GetKeys())
            {
                if (key.StartsWith(SaveContext))
                {
                    string currencyID = key.Replace($"{SaveContext}-", "").Replace("-Int32", "");
                    LoadCurrency(currencyID);
                }
            }
        }

#if UNITY_EDITOR
        [Button]
        private void InitCurrencies()
        {
            var assetsGuid = AssetDatabase.FindAssets($"t:{typeof(CurrencyData)}", new string[] { _currencyFolderPath });
            var assetPaths = assetsGuid.Select(guid => AssetDatabase.GUIDToAssetPath(guid));

            //
            _currencies.Clear();
            foreach (var path in assetPaths)
            {
                _currencies.Add(AssetDatabase.LoadAssetAtPath<CurrencyData>(path));
            }
        }
#endif

        //
        public Dictionary<string, int> GetAllCurrencies() => new(_savedCurrencies);
        public int GetCurrencyAmount(CurrencyData currency) => _savedCurrencies.TryGetValue(currency.ID, out int amount) ? amount : 0;
        public bool HasEnoughCurrency(CurrencyData currency, int amount) => GetCurrencyAmount(currency) >= amount;
    }
}