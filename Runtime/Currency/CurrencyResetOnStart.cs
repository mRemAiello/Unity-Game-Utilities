using System.Collections.Generic;
using UnityEngine;

namespace GameUtils
{
    public class CurrencyResetOnStart : MonoBehaviour
    {
        [SerializeField] private List<CurrencyData> _currenciesToReset = new();

        private void Start()
        {
            if (CurrencyManager.Instance == null)
            {
                return;
            }

            foreach (var currency in _currenciesToReset)
            {
                if (currency == null)
                {
                    continue;
                }

                CurrencyManager.Instance.SetCurrencyAmount(currency, 0);
            }
        }
    }
}