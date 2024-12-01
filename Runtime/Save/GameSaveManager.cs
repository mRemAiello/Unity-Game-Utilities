using CI.QuickSave;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using UnityEngine;
using VInspector;

namespace GameUtils
{
    public class GameSaveManager : Singleton<GameSaveManager>
    {
        [SerializeField, ReadOnly] private int _currentSaveSlot;
        [SerializeField, ReadOnly] private SerializedDictionary<string, string> _keys;

        protected override void OnPostAwake()
        {
            DebugCurrentFileSave();
        }

        public bool Exists<T>(string key, string suffix = "")
        {
            CheckFileSave();

            //
            var id = GetID<T>(key, suffix);

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            if (saveReader.Exists(id))
            {
                return true;
            }

            //
            return false;
        }

        public bool TryLoad<T>(string key, out T result, T defaultValue = default, string suffix = "")
        {
            CheckFileSave();

            //
            var id = GetID<T>(key, suffix);

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            if (saveReader.Exists(id))
            {
                result = saveReader.Read<T>(id);
                return true;
            }

            //
            result = defaultValue;
            return false;
        }

        public void Save<T>(string key, T amount, string suffix = "")
        {
            CheckFileSave();

            //
            var id = GetID<T>(key, suffix);

            //
            var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);
            saveWriter.Write(id, amount);
            saveWriter.Commit();

            //
            _keys[id] = amount.ToString();
        }

        public T Load<T>(string key, T defaultValue = default, string suffix = "")
        {
            CheckFileSave();

            //
            var id = GetID<T>(key, suffix);

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            if (saveReader.TryRead(id, out T result))
            {
                return result;
            }

            return defaultValue;
        }

        public void RemoveKey<T>(string key, string suffix = "")
        {
            CheckFileSave();

            //
            var id = GetID<T>(key, suffix);
            var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);

            //
            if (saveWriter.Exists(id))
            {
                saveWriter.Delete(id);
                saveWriter.Commit();

                //
                _keys.Remove(id);
            }
        }

        public void Clear()
        {
            CheckFileSave();

            //
            var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);
            var saveKeys = saveWriter.GetAllKeys().ToList();
            foreach (var key in saveKeys)
            {
                saveWriter.Delete(key);
            }
            saveWriter.Commit();

            //
            _keys.Clear();
        }

        [Button]
        private void DebugCurrentFileSave()
        {
            CheckFileSave();

            //
            var saveReader = QuickSaveReader.Create("Save" + _currentSaveSlot);
            var saveKeys = saveReader.GetAllKeys().ToList();

            //
            _keys.Clear();
            foreach (var key in saveKeys)
            {
                if (!_keys.ContainsKey(key))
                {
                    if (saveReader.TryRead(key, out JObject jObj))
                    {
                        _keys.Add(key, CleanJObjectString(jObj.ToString(Formatting.None)));
                    }
                    else
                    {
                        if (saveReader.TryRead(key, out object obj))
                        {
                            _keys.Add(key, obj.ToString());
                        }
                    }
                }
            }
        }

        private string GetID<T>(string key, string suffix = "")
        {
            var id = string.Format("{0}-{1}", key, typeof(T).Name);
            if (!string.IsNullOrEmpty(suffix))
            {
                id += $"-{suffix}";
            }

            return id;
        }

        private void CheckFileSave()
        {
            //
            if (!QuickSaveBase.RootExists("Save" + _currentSaveSlot))
            {
                var saveWriter = QuickSaveWriter.Create("Save" + _currentSaveSlot);
                saveWriter.Commit();
            }
        }

        private string CleanJObjectString(string str)
        {
            var ret = str.Replace("{", "");
            ret = ret.Replace("}", "");
            ret = ret.Replace(":", ": ");
            ret = ret.Replace(",", ", ");
            ret = ret.Replace("\"", "");

            return ret;
        }

        // 
        public int GetActiveSaveSlot() => _currentSaveSlot;
        [Button] public void SetActiveSaveSlot(int slot) => _currentSaveSlot = slot;
    }
}