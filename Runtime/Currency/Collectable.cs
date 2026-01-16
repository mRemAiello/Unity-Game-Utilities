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
        [SerializeField] private bool _collectOnTriggerEnter = true;
        [SerializeField] private bool _collectOnCollisionEnter = true;

        //
        public bool LogEnabled => _logEnabled;

        //
        [Button(ButtonSizes.Medium)]
        public void Collect()
        {
            this.Log($"Collectable collected! Increment value: {_incrementValue}");

            // 
            CurrencyManager.Instance.AddCurrency(_currencyData, _incrementValue);

            // 
            OnPostCollect(_currencyData, _incrementValue);
        }

        private void OnTriggerEnter(Collider other)
        {
            // 
            if (!_collectOnTriggerEnter)
            {
                // 
                return;
            }

            // 
            Collect();
        }

        private void OnCollisionEnter(Collision collision)
        {
            // 
            if (!_collectOnCollisionEnter)
            {
                // 
                return;
            }

            // 
            Collect();
        }

        public virtual void OnPostCollect(CurrencyData currencyData, int incrementValue)
        {
        }
    }
}
