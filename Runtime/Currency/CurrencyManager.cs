using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [DeclareBoxGroup("debug", Title = "Debug")]
    public class CurrencyManager : Singleton<CurrencyManager>, ISaveable, ILoggable
    {
        [SerializeField, ReadOnly, Group("debug")] private SerializedDictionary<string, int> _currencies = new();

        //
        public string SaveContext => "Currency";
        public bool LogEnabled => true;

        //
        protected override void OnPostAwake()
        {
            LoadAllCurrencies();
        }

        [Button]
        public void AddCurrency(string currencyId, int amount)
        {
            if (!_currencies.ContainsKey(currencyId))
                _currencies[currencyId] = 0;

            _currencies[currencyId] += amount;
            SaveCurrency(currencyId);
        }

        [Button]
        public bool TryRemoveCurrency(string currencyId, int amount)
        {
            if (!_currencies.ContainsKey(currencyId) || _currencies[currencyId] < amount)
                return false;

            _currencies[currencyId] -= amount;
            SaveCurrency(currencyId);
            return true;
        }

        public int GetCurrencyAmount(string currencyId)
        {
            return _currencies.TryGetValue(currencyId, out int amount) ? amount : 0;
        }

        [Button]
        public void SetCurrencyAmount(string currencyId, int amount)
        {
            _currencies[currencyId] = Mathf.Max(0, amount);
            SaveCurrency(currencyId);
        }

        public bool HasEnoughCurrency(string currencyId, int amount)
        {
            return GetCurrencyAmount(currencyId) >= amount;
        }

        [Button]
        public void ResetCurrencies()
        {
            _currencies.Clear();
            SaveAllCurrencies();
        }

        [Button]
        private void SaveCurrency(string currencyId)
        {
            GameSaveManager.Instance.Save(SaveContext, currencyId, _currencies[currencyId]);
        }

        [Button]
        private void LoadCurrency(string currencyId)
        {
            _currencies[currencyId] = GameSaveManager.Instance.Load(SaveContext, currencyId, 0);
        }

        [Button]
        private void LoadAllCurrencies()
        {
            foreach (var key in GameSaveManager.Instance.GetKeys())
            {
                if (key.StartsWith(SaveContext))
                {
                    string currencyId = key.Replace($"{SaveContext}-", "").Replace("-Int32", "");
                    LoadCurrency(currencyId);
                }
            }
        }

        private void SaveAllCurrencies()
        {
            foreach (var currency in _currencies)
            {
                SaveCurrency(currency.Key);
            }
        }

        //
        public Dictionary<string, int> GetAllCurrencies() => new(_currencies);
        [Button] public void AddCurrency(CurrencyData currencyData, int amount) => AddCurrency(currencyData.ID, amount);
        [Button] public bool TryRemoveCurrency(CurrencyData currencyData, int amount) => TryRemoveCurrency(currencyData.ID, amount);
        [Button] public int GetCurrencyAmount(CurrencyData currencyData) => GetCurrencyAmount(currencyData.ID);
        [Button] public void SetCurrencyAmount(CurrencyData currencyData, int amount) => SetCurrencyAmount(currencyData.ID, amount);
        [Button] public bool HasEnoughCurrency(CurrencyData currencyData, int amount) => HasEnoughCurrency(currencyData.ID, amount);
    }
}