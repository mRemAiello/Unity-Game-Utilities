using TriInspector;
using UnityEngine;

namespace GameUtils
{
    public abstract class BaseTrackerUI : MonoBehaviour, ILoggable
    {
        [SerializeField, Group("Debug"), PropertyOrder(99)] private bool _logEnabled;

        //
        public bool LogEnabled => _logEnabled;

        //
        private void Update()
        {
            if (!ValidateReferences())
                return;

            // If references are valid, refresh the UI.
            RefreshUI();
        }

        protected string FormatRuntimeAttribute(RuntimeAttribute runtime, bool showModifier = false)
        {
            string result = $"{runtime.Name}: {runtime.CurrentValue}\n";

            // If it's a vital attribute, also show the max value.
            if (runtime.IsVital && runtime is RuntimeVital vitalAttr)
            {
                result = $"{runtime.Name}: {vitalAttr.CurrentValue}/{vitalAttr.CurrentMaxValue}\n";
            }

            // If showModifier is true, also show the modifiers.
            if (showModifier && runtime.Modifiers.Count > 0)
            {
                for (int i = 0; i < runtime.Modifiers.Count; i++)
                {
                    var mod = runtime.Modifiers[i];
                    result += $"\t{mod}\n";
                }
            }

            return result;
        }

        //
        protected abstract bool ValidateReferences();
        protected abstract void RefreshUI();
    }
}