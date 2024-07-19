using System.Collections.Generic;

namespace GameUtils
{
    public class DebugManager : Singleton<DebugManager>
    {
        private Dictionary<string, DebugCategory> _categories;

        protected override void OnPostAwake()
        {
            _categories = new Dictionary<string, DebugCategory>();
        }

        public DebugCategory GetCategory(string categoryName)
        {
            if (!_categories.ContainsKey(categoryName))
            {
                _categories[categoryName] = new DebugCategory(categoryName);
            }
            
            return _categories[categoryName];
        }
    }
}