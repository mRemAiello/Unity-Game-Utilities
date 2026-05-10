using System.Collections.Generic;
using System.Linq;

namespace GameUtils
{
    public class SaveFileStorage
    {
        private const string DefaultRootPrefix = "Save";

        private readonly string _rootPrefix;
        private readonly bool _useEncryption;
        private readonly string _encryptionPassword;
        private readonly SaveEncryptionMode _encryptionMode;

        public SaveFileStorage(bool useEncryption, string encryptionPassword, SaveEncryptionMode encryptionMode = SaveEncryptionMode.Aes, string rootPrefix = DefaultRootPrefix)
        {
            _rootPrefix = rootPrefix;
            _useEncryption = useEncryption;
            _encryptionPassword = encryptionPassword;
            _encryptionMode = encryptionMode;
        }

        public bool Exists(int slot, string key)
        {
            EnsureRoot(slot);

            var reader = CreateReader(slot);
            return reader.Exists(key);
        }

        public bool TryRead<T>(int slot, string key, out T result)
        {
            EnsureRoot(slot);

            var reader = CreateReader(slot);
            return reader.TryRead(key, out result);
        }

        public T Read<T>(int slot, string key)
        {
            EnsureRoot(slot);

            var reader = CreateReader(slot);
            return reader.Read<T>(key);
        }

        public void Write<T>(int slot, string key, T value)
        {
            EnsureRoot(slot);

            var writer = CreateWriter(slot);
            writer.Write(key, value);
            writer.Commit();
        }

        public bool Delete(int slot, string key)
        {
            EnsureRoot(slot);

            var writer = CreateWriter(slot);
            if (!writer.Exists(key))
            {
                return false;
            }

            writer.Delete(key);
            writer.Commit();
            return true;
        }

        public IReadOnlyList<string> GetAllKeys(int slot)
        {
            EnsureRoot(slot);

            var reader = CreateReader(slot);
            return reader.GetAllKeys().ToList();
        }

        public void Clear(int slot)
        {
            EnsureRoot(slot);

            var writer = CreateWriter(slot);
            var keys = writer.GetAllKeys().ToList();
            foreach (var key in keys)
            {
                writer.Delete(key);
            }

            writer.Commit();
        }

        public void EnsureRoot(int slot)
        {
            var root = GetRoot(slot);
            if (!SaveFileStoreBase.RootExists(root))
            {
                var writer = SaveFileWriter.Create(root, BuildSettings());
                writer.Commit();
            }
        }

        private SaveFileReader CreateReader(int slot)
        {
            return SaveFileReader.Create(GetRoot(slot), BuildSettings());
        }

        private SaveFileWriter CreateWriter(int slot)
        {
            return SaveFileWriter.Create(GetRoot(slot), BuildSettings());
        }

        private string GetRoot(int slot) => _rootPrefix + slot;

        private SaveEncryptionSettings BuildSettings()
        {
            var settings = new SaveEncryptionSettings
            {
                EncryptionMode = _useEncryption ? _encryptionMode : SaveEncryptionMode.None
            };

            if (_useEncryption)
            {
                settings.Password = _encryptionPassword;
            }

            return settings;
        }
    }
}
