using System;
using System.Collections.Generic;
using System.Data;
using TriInspector;
using UnityEngine;

namespace GameUtils
{
    [CreateAssetMenu(menuName = GameUtilsMenuConstants.CONSTANTS_NAME + "Formula")]
    public class FloatFormulaConstant : NumericGameConstant
    {
        [SerializeField] private string _formula = "1 / {0}";
        [SerializeField] private List<NumericGameConstant> _values = new();

        [Header("Debug")]
        [SerializeField, ReadOnly] private string _debugValue = "";

        //
        public override float FloatValue
        {
            get
            {
                string formattedFormula = string.Format(_formula, _values.ToArray());
                return EvaluateExpression(formattedFormula);
            }
        }

        //
        public override int IntValue => (int)FloatValue;
        public override string StringValue => FloatValue.ToString();

        //
        [Button(ButtonSizes.Medium)]
        private void ShowDebugValue()
        {
            //
            List<float> values = new();
            foreach (var value in _values)
            {
                values.Add(value.FloatValue);
            }

            //
            string formattedFormula = string.Format(_formula, values.ToArray());
            _debugValue = EvaluateExpression(formattedFormula).ToString();
        }

        private float EvaluateExpression(string expression)
        {
            try
            {
                DataTable table = new();
                var result = table.Compute(expression, null);
                return Convert.ToSingle(result);
            }
            catch (Exception e)
            {
                Debug.LogError($"Errore nella valutazione dell'espressione: {expression}. Errore: {e.Message}");
                return float.NaN;
            }
        }
    }
}