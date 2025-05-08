using System;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public class Collectable : MonoBehaviour, ILoggable
    {
        [SerializeField] private CurrencyData _currencyData;
        [SerializeField] private int _incrementValue;
        [SerializeField] private bool _logEnabled;

        //
        public bool LogEnabled => _logEnabled;

        //
        [Button]
        public void Collect()
        {
            this.Log($"Collectable collected! Increment value: {_incrementValue}");

            // 
            CurrencyManager.Instance.AddCurrency(_currencyData, _incrementValue);

            // 
            OnPostCollect(_currencyData, _incrementValue);
        }

        public virtual void OnPostCollect(CurrencyData currencyData, int incrementValue)
        {
        }
    }
}