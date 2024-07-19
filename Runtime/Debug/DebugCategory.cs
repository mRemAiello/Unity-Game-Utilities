using UnityEngine;

namespace GameUtils
{
    public class DebugCategory
    {
        private readonly string _categoryName;

        public DebugCategory(string name)
        {
            _categoryName = name;
        }

        public void Log(string message)
        {
            Debug.Log($"[{_categoryName}] {message}");
        }

        public void LogWarning(string message)
        {
            Debug.LogWarning($"[{_categoryName}] {message}");
        }

        public void LogError(string message)
        {
            Debug.LogError($"[{_categoryName}] {message}");
        }
    }
}