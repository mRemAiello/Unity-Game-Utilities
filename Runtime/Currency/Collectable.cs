using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameUtils
{
    public class Collectable : MonoBehaviour, ILoggable, IPointerClickHandler
    {
        [SerializeField] private CurrencyData _currencyData;
        [SerializeField] private int _incrementValue;
        [SerializeField] private bool _logEnabled;
        [SerializeField] private bool _collectOnTriggerEnter = false;
        [SerializeField] private bool _collectOnCollisionEnter = false;
        [SerializeField] private bool _collectOnClick = false;

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
            if (!_collectOnTriggerEnter)
            {
                return;
            }

            // 
            Collect();
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!_collectOnCollisionEnter)
            {
                return;
            }

            // 
            Collect();
        }

        private void OnMouseDown()
        {
            if (!_collectOnClick)
            {
                return;
            }

            // 
            Collect();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_collectOnClick)
            {
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