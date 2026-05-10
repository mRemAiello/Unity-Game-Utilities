using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameUtils
{
    public class SaveFileReader : SaveFileStoreBase
    {
        private SaveFileReader(string root, SaveEncryptionSettings settings) : base(root, settings)
        {
        }

        public static SaveFileReader Create(string root)
        {
            return Create(root, new SaveEncryptionSettings());
        }

        public static SaveFileReader Create(string root, SaveEncryptionSettings settings)
        {
            var reader = new SaveFileReader(root, settings);
            reader.Load();
            return reader;
        }

        public bool Exists(string key)
        {
            return _items.ContainsKey(key);
        }

        public T Read<T>(string key)
        {
            if (!TryRead(key, out T result))
            {
                throw new KeyNotFoundException($"Key not found: {key}");
            }

            return result;
        }

        public bool TryRead<T>(string key, out T result)
        {
            if (!_items.TryGetValue(key, out string json))
            {
                result = default;
                return false;
            }

            result = JsonConvert.DeserializeObject<T>(json);
            return true;
        }

        public IEnumerable<string> GetAllKeys()
        {
            return _items.Keys;
        }
    }
}
