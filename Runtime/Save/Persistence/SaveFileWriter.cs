using Newtonsoft.Json;
using System.Collections.Generic;

namespace GameUtils
{
    public class SaveFileWriter : SaveFileStoreBase
    {
        private SaveFileWriter(string root, SaveEncryptionSettings settings) : base(root, settings)
        {
        }

        public static SaveFileWriter Create(string root)
        {
            return Create(root, new SaveEncryptionSettings());
        }

        public static SaveFileWriter Create(string root, SaveEncryptionSettings settings)
        {
            var writer = new SaveFileWriter(root, settings);
            writer.Load();
            return writer;
        }

        public bool Exists(string key)
        {
            return _items.ContainsKey(key);
        }

        public SaveFileWriter Write<T>(string key, T value)
        {
            _items[key] = JsonConvert.SerializeObject(value);
            return this;
        }

        public void Delete(string key)
        {
            _items.Remove(key);
        }

        public IEnumerable<string> GetAllKeys()
        {
            return _items.Keys;
        }

        public void Commit()
        {
            Save();
        }
    }
}
