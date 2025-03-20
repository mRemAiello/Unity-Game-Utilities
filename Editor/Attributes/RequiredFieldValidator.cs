using System.Reflection;
using GameUtils;
using UnityEngine;

namespace UnityEditor.GameUtils
{
    /*public static class RequiredFieldValidator
    {
        static RequiredFieldValidator()
        {
            EditorApplication.playModeStateChanged += (state) =>
            {
                if (state == PlayModeStateChange.ExitingPlayMode)
                {
                    ValidateRequiredFields();
                }
            };
        }

        private static void ValidateRequiredFields()
        {
            bool shouldPauseGame = false;
            MonoBehaviour[] monoBehaviours = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (MonoBehaviour monoBehaviour in monoBehaviours)
            {
                FieldInfo[] fields = monoBehaviour.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    var requiredAttribute = field.GetCustomAttribute<Required>();
                    if (requiredAttribute == null)
                        continue;

                    object value = field.GetValue(monoBehaviour);
                    if (value.Equals(null) && requiredAttribute.WarningType == WarningType.Error)
                    {
                        Debug.LogError($"Required field {field.Name} is not set in {monoBehaviour.name}.");
                        shouldPauseGame = true;
                    }
                }
            }

            //
            if (shouldPauseGame)
            {
                EditorApplication.ExitPlaymode();
            }
        }
    }*/
}