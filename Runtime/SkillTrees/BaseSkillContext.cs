using System;

namespace GameUtils
{
    public class BaseSkillContext : ISkillContext
    {
        private readonly SerializedDictionary<Type, object> _capabilities = new();

        //
        public void Add<T>(T capability) where T : class
        {
            var type = typeof(T);
            _capabilities[type] = capability;
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);

            if (_capabilities.TryGetValue(type, out var capability))
                return capability as T;

            throw new Exception($"Capability of type {type.Name} not found.");
        }

        public bool TryGet<T>(out T capability) where T : class
        {
            var type = typeof(T);
            if (_capabilities.TryGetValue(type, out var obj))
            {
                capability = obj as T;
                return capability != null;
            }

            capability = null;
            return false;
        }

        public bool Has<T>() where T : class => _capabilities.ContainsKey(typeof(T));
    }
}